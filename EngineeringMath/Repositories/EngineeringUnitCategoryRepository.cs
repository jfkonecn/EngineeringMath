using EngineeringMath.EngineeringModel;
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
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(unitCategoryRepository, logger)
        {
            UnitCategoryRepository = unitCategoryRepository;
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
                    return new RepositoryResult<IEnumerable<EngineeringUnitCategory>>(compositeResult.StatusCode, null);
                }
                newUnits.AddRange(compositeResult.ResultObject);


                foreach (Unit unit in blueprint.Units ?? new List<Unit>())
                {
                    var unitSys = new List<EngineeringUnitSystem>();
                    foreach (var system in unit.UnitSystems)
                    {
                        unitSys.Add(new EngineeringUnitSystem()
                        {
                            Abbreviation = system.Abbreviation.TryToFindStringInLibraryResources(),
                            Name = system.Name.TryToFindStringInLibraryResources(),
                            OwnerName = system.Owner.Name,
                        });
                    }
                    try
                    {
                        newUnits.Add(new EngineeringUnit()
                        {
                            Name = unit.Name.TryToFindStringInLibraryResources(),
                            Symbol = unit.Symbol.TryToFindStringInLibraryResources(),
                            ConvertFromSi = StringEquationFactory.CreateStringEquation(unit.ConvertFromSi),
                            ConvertToSi = StringEquationFactory.CreateStringEquation(unit.ConvertToSi),
                            OwnerName = unit.Owner.Name,
                            UnitSystems = unitSys,
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error(nameof(BuildT), e.ToString());
                        return new RepositoryResult<IEnumerable<EngineeringUnitCategory>>(RepositoryStatusCode.internalError, null);
                    }
                }
                unitCategories.Push(new EngineeringUnitCategory()
                {
                    Name = blueprint.Name,
                    Units = newUnits,
                    OwnerName = blueprint.Owner.Name
                });
            }
            return new RepositoryResult<IEnumerable<EngineeringUnitCategory>>(RepositoryStatusCode.success, unitCategories);
        }

        private IResult<RepositoryStatusCode, IEnumerable<EngineeringUnit>> CreateCompositeUnits(UnitCategory blueprint)
        {
            Stack<EngineeringUnit> compositeUnits = new Stack<EngineeringUnit>();
            if (!string.IsNullOrEmpty(blueprint.CompositeEquation))
            {
                IStringEquation stringEquation;
                try
                {
                    stringEquation = StringEquationFactory.CreateStringEquation(blueprint.CompositeEquation);
                }
                catch (Exception ex)
                {
                    Logger.Error(nameof(CreateCompositeUnits), "Failed to build composite equation!");
                    Logger.Error("Inner Exception", ex.ToString());
                    return new RepositoryResult<IEnumerable<EngineeringUnit>>(RepositoryStatusCode.internalError, null);
                }
                IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> compositeResult = GetById(stringEquation.EquationArguments);
                if (compositeResult.ResultObject == null || compositeResult.ResultObject.Count() != stringEquation.EquationArguments.Count())
                {
                    LogNotFoundUnits(stringEquation, compositeResult);
                    return new RepositoryResult<IEnumerable<EngineeringUnit>>(RepositoryStatusCode.objectNotFound, null);
                }
                Dictionary<string, IEnumerable<EngineeringUnit>> dic = compositeResult
                    .ResultObject
                    .ToDictionary(cat => cat.Name, cat => cat.Units);

                var unitSystems = new Dictionary<EngineeringUnitSystem, Dictionary<string, EngineeringUnit>>();

                foreach (KeyValuePair<string, IEnumerable<EngineeringUnit>> item in dic)
                {
                    foreach (var unit in item.Value)
                    {
                        foreach (var system in unit.UnitSystems)
                        {
                            if (system.Name == LibraryResources.ImperialFullName ||
                                system.Name == LibraryResources.MetricFullName)
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
                                Logger.Error("CreateCompositeUnits", $"More than one unit using {system.Name} as a system!");
                                return new RepositoryResult<IEnumerable<EngineeringUnit>>(RepositoryStatusCode.internalError, null);

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
                            UnitSystems = new EngineeringUnitSystem[] { system },
                            OwnerName = blueprint.Owner.Name
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.Error(nameof(CreateCompositeUnits), e.ToString());
                        return new RepositoryResult<IEnumerable<EngineeringUnit>>(RepositoryStatusCode.internalError, null);
                    }
                }
            }
            return new RepositoryResult<IEnumerable<EngineeringUnit>>(RepositoryStatusCode.success, compositeUnits);

        }

        private void LogNotFoundUnits(IStringEquation stringEquation, IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> compositeResult)
        {
            StringBuilder sb = new StringBuilder(128);
            sb.Append("We were looking for ");
            foreach (string arg in stringEquation.EquationArguments)
            {
                sb.Append($"[{arg}] ");
            }

            if (compositeResult.ResultObject == null || compositeResult.ResultObject.Count() == 0)
            {
                sb.Append(" but we found nothing!");
            }
            else
            {
                sb.Append(" but we only found ");
                foreach (var obj in compositeResult.ResultObject)
                {
                    sb.Append($"[{obj.Name}] ");
                }
            }
            Logger.Error(nameof(CreateCompositeUnits), sb.ToString());
        }



        private string CreateCompositeName(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in exponents)
            {
                string name = units[item.Key].Name.TryToFindStringInLibraryResources();
                builder.Append($"{ name.RaiseToTheNPower(item.Value) }");
                if (i < exponents.Count() - 1)
                {
                    builder.Append(" * ");
                }
                i++;
            }
            return builder.ToString();
        }


        private string CreateCompositeSymbol(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in exponents)
            {
                string symbol = units[item.Key].Symbol.TryToFindStringInLibraryResources();
                builder.Append(symbol.RaiseToTheNPower(item.Value));
                if(i < exponents.Count() - 1)
                {
                    builder.Append(" * ");
                }
                i++;
            }
            return builder.ToString();
        }



        private IStringEquation CreateConvertToSi(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= Math.Pow(units[system].ConvertToSi.Evaluate(1), exponents[system]);
            }
            return StringEquationFactory.CreateStringEquation($"$0 * { factor }");
        }

        private IStringEquation CreateConvertFromSi(Dictionary<string, double> exponents, Dictionary<string, EngineeringUnit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= Math.Pow(units[system].ConvertFromSi.Evaluate(1), exponents[system]);
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
        public IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }
    }
}
