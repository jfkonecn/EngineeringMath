﻿using EngineeringMath.EngineeringModel;
using EngineeringMath.Factories;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Resources;
using EngineeringMath.Results;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class EngineeringUnitCategoryRepository : ReadonlyCacheRepositoryBase<string, EngineeringUnitCategory, UnitCategory>
    {
        public EngineeringUnitCategoryRepository(
            IReadonlyRepository<UnitCategory> unitCategoryRepository,
            IReadonlyRepository<Unit> unitRepository,
            IResultFactory resultFactory,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(unitCategoryRepository, resultFactory, logger)
        {
            UnitCategoryRepository = unitCategoryRepository;
            UnitRepository = unitRepository;
            ResultFactory = resultFactory;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }



        protected override string GetKey(EngineeringUnitCategory obj)
        {
            return obj.Name;
        }

        protected override string GetKey(UnitCategory obj)
        {
            return obj.Name;
        }


        protected override IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> BuildT(IEnumerable<UnitCategory> blueprints)
        {
            Stack<EngineeringUnitCategory> unitCategories = new Stack<EngineeringUnitCategory>();
            foreach (UnitCategory blueprint in blueprints)
            {
                var newUnits = new List<EngineeringUnit>();
                IResult<RepositoryStatusCode, IEnumerable<EngineeringUnit>> compositeResult = CreateCompositeUnits(blueprint);
                if (compositeResult.StatusCode != RepositoryStatusCode.success)
                {
                    return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                            compositeResult.StatusCode, RepositoryAction.Get, null);
                }
                newUnits.AddRange(compositeResult.ResultObject);
                 
                foreach (Unit unit in blueprint.Units)
                {
                    var unitSys = new List<string>();
                    foreach (var system in unit.UnitSystems)
                    {
                        unitSys.Add(system.Name);
                    }
                    try
                    {
                        newUnits.Add(new EngineeringUnit()
                        {
                            Name = unit.Name,
                            Symbol = unit.Symbol,
                            ConvertFromSi = StringEquationFactory.CreateStringEquation(unit.ConvertFromSi),
                            ConvertToSi = StringEquationFactory.CreateStringEquation(unit.ConvertToSi),
                            OwnerName = unit.Owner.Name,
                            UnitSystems = unitSys,
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error(nameof(BuildT), e.Message);
                        return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                            RepositoryStatusCode.internalError, RepositoryAction.Get, null);
                    }
                    unitCategories.Push(new EngineeringUnitCategory()
                    {
                        Name = blueprint.Name,
                        Units = newUnits,
                        OwnerName = blueprint.Owner.Name
                    });
                }

            }
            return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                RepositoryStatusCode.success, RepositoryAction.Get, unitCategories);
        }

        private IResult<RepositoryStatusCode, IEnumerable<EngineeringUnit>> CreateCompositeUnits(UnitCategory blueprint)
        {
            Stack<EngineeringUnit> compositeUnits = new Stack<EngineeringUnit>();
            if (!string.IsNullOrEmpty(blueprint.CompositeEquation))
            {
                IStringEquation stringEquation = StringEquationFactory.CreateStringEquation(blueprint.CompositeEquation);
                IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> compositeResult = GetById(stringEquation.EquationArguments);
                if (compositeResult.StatusCode != RepositoryStatusCode.success)
                {
                    return ResultFactory
                        .BuilderResult<IEnumerable<EngineeringUnit>>(compositeResult.StatusCode, RepositoryAction.Get, null);
                }
                Dictionary<string, IEnumerable<EngineeringUnit>> dic = compositeResult
                    .ResultObject
                    .ToDictionary(cat => cat.Name, cat => cat.Units);

                var unitSystems = new Dictionary<string, Dictionary<string, EngineeringUnit>>();

                foreach (KeyValuePair<string, IEnumerable<EngineeringUnit>> item in dic)
                {
                    foreach (var unit in item.Value)
                    {
                        foreach (var system in unit.UnitSystems)
                        {
                            if (system == nameof(LibraryResources.ImperialFullName) ||
                                system == nameof(LibraryResources.MetricFullName))
                            {
                                // SI or USC good systems to use, but we can't inner join all the units
                                // since that would cost too much memory
                                continue;
                            }

                            if (!unitSystems.ContainsKey(system))
                            {
                                unitSystems.Add(system, new Dictionary<string, EngineeringUnit>());
                            }

                            if (unitSystems[system].ContainsKey(item.Key))
                            {
                                Logger.Error("CreateCompositeUnits", $"More than one unit using {system} as a system!");
                                return ResultFactory.BuilderResult<IEnumerable<EngineeringUnit>>(
                                    RepositoryStatusCode.internalError, RepositoryAction.Get, null);

                            }
                            unitSystems[system].Add(item.Key, unit);
                        }
                    }
                }

                Dictionary<string, double> exponents = GetExponents(stringEquation);

                foreach (var system in unitSystems.Keys)
                {
                    try
                    {
                        compositeUnits.Push(new EngineeringUnit()
                        {
                            Name = CreateCompositeName(exponents, unitSystems[system]),
                            Symbol = CreateCompositeSymbol(exponents, unitSystems[system]),
                            ConvertToSi = CreateConvertToSi(exponents, unitSystems[system]),
                            ConvertFromSi = CreateConvertFromSi(exponents, unitSystems[system]),
                            UnitSystems = new string[] { system },
                            OwnerName = blueprint.Owner.Name
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error(nameof(CreateCompositeUnits), e.Message);
                        return ResultFactory.BuilderResult<IEnumerable<EngineeringUnit>>(
                                        RepositoryStatusCode.internalError, RepositoryAction.Get, null);
                    }
                }
            }
            return ResultFactory.BuilderResult<IEnumerable<EngineeringUnit>>(
                RepositoryStatusCode.success, RepositoryAction.Get, compositeUnits);

        }

        private string SuperscriptNumber(double number)
        {
            string numStr = number.ToString();
            StringBuilder builder = new StringBuilder(7);
            foreach (char c in numStr)
            {
                char uniChar;
                switch (c)
                {
                    case '0':
                        uniChar = '\u2090';
                        break;
                    case '1':
                        uniChar = '\u2091';
                        break;
                    case '2':
                        uniChar = '\u2092';
                        break;
                    case '3':
                        uniChar = '\u2093';
                        break;
                    case '4':
                        uniChar = '\u2094';
                        break;
                    case '5':
                        uniChar = '\u2095';
                        break;
                    case '6':
                        uniChar = '\u2096';
                        break;
                    case '7':
                        uniChar = '\u2097';
                        break;
                    case '8':
                        uniChar = '\u2098';
                        break;
                    case '9':
                        uniChar = '\u2099';
                        break;
                    case '.':
                        uniChar = '\u22C5';
                        break;
                    default:
                        uniChar = '\0';
                        break;
                }
                builder.Append(uniChar);
            }
            return builder.ToString();
        }

        private string CreateCompositeName(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in exponents)
            {
                builder.Append($"{ units[item.Key].Name }{SuperscriptNumber(item.Value)}");
            }
            return builder.ToString();
        }

        private string CreateCompositeSymbol(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in exponents)
            {
                builder.Append($"{ units[item.Key].Symbol }{SuperscriptNumber(item.Value)}");
            }
            return builder.ToString();
        }

        private IStringEquation CreateConvertToSi(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= units[system].ConvertToSi.Evaluate(1);
            }
            return StringEquationFactory.CreateStringEquation($"$0 * { factor }");
        }

        private IStringEquation CreateConvertFromSi(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= units[system].ConvertFromSi.Evaluate(1);
            }
            return StringEquationFactory.CreateStringEquation($"$0 * { factor }");
        }

        private Dictionary<string, double> GetExponents(IStringEquation stringEquation)
        {
            Dictionary<string, double> exponents = new Dictionary<string, double>();

            var nums = new double[stringEquation.EquationArguments.Count()];

            for (int i = 0; i < stringEquation.EquationArguments.Count(); i++)
            {
                for (int j = 0; j < nums.Length; j++)
                {
                    if (j == i)
                    {
                        nums[j] = 10;
                    }
                    else
                    {
                        nums[j] = 1;
                    }
                }
                exponents.Add(stringEquation.EquationArguments[i], Math.Log10(stringEquation.Evaluate(nums)));
            }

            return exponents;
        }



        private IReadonlyRepository<UnitCategory> UnitCategoryRepository { get; }
        private IReadonlyRepository<Unit> UnitRepository { get; }
        public IResultFactory ResultFactory { get; }
        public IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }
    }
}