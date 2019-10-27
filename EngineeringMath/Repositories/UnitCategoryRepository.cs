using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class UnitCategoryRepository : ReadonlyCacheRepositoryBase<string, BuiltUnitCategory, UnitCategory>
    {
        public UnitCategoryRepository(
            EngineeringMathContext dbContext,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(logger)
        {
            DbContext = dbContext;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }



        protected override string GetKey(BuiltUnitCategory obj)
        {
            return obj.Name;
        }

        protected override string GetKey(UnitCategory obj)
        {
            return obj.Name;
        }

        protected async override Task<IEnumerable<BuiltUnitCategory>> BuildTAsync(Func<UnitCategory, bool> whereCondition)
        {
            Stack<BuiltUnitCategory> unitCategories = new Stack<BuiltUnitCategory>();
            var blueprints = await DbContext
                .UnitCategories
                .Include(x => x.Units)
                .Include(x => x.Owner)
                .ToListAsync();
            foreach (UnitCategory blueprint in blueprints)
            {
                var newUnits = new List<BuiltUnit>();
                IEnumerable<BuiltUnit> compositeResult = await CreateCompositeUnitsAsync(blueprint);
                if(compositeResult != null)
                {
                    newUnits.AddRange(compositeResult);
                }


                foreach (Unit unit in blueprint.Units ?? new List<Unit>())
                {
                    var unitSys = new List<BuiltUnitSystem>();
                    foreach (var system in unit.UnitSystemUnits.Select(x => x.UnitSystem))
                    {
                        unitSys.Add(new BuiltUnitSystem()
                        {
                            Abbreviation = system.Abbreviation.TryToFindStringInLibraryResources(),
                            Name = system.Name.TryToFindStringInLibraryResources(),
                            OwnerName = system.Owner.Name,
                        });
                    }
                    try
                    {
                        newUnits.Add(new BuiltUnit()
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
                        Logger.LogError(nameof(BuildTAsync), e.ToString());
                        throw;
                    }
                }
                unitCategories.Push(new BuiltUnitCategory()
                {
                    Name = blueprint.Name,
                    Units = newUnits,
                    OwnerName = blueprint.Owner.Name
                });
            }
            return unitCategories;
        }

        private async Task<IEnumerable<BuiltUnit>> CreateCompositeUnitsAsync(UnitCategory blueprint)
        {
            Stack<BuiltUnit> compositeUnits = null;
            if (!string.IsNullOrEmpty(blueprint.CompositeEquation))
            {
                IStringEquation stringEquation = CreateCompositeUnitEquation(blueprint);
                Dictionary<string, IEnumerable<BuiltUnit>> unitCategoryToUnitsMap = await GetUnitsTheCompositeUnitIsBuiltOn(stringEquation);

                Dictionary<BuiltUnitSystem, Dictionary<string, BuiltUnit>> unitSystems = FindAllCompositeUnitPermutations(unitCategoryToUnitsMap);

                Dictionary<string, double> exponents = GetExponents(stringEquation);

                compositeUnits = MakeCompositeUnitsFromBaseUnits(blueprint, unitSystems, exponents);
            }
            return compositeUnits;

        }

        private async Task<Dictionary<string, IEnumerable<BuiltUnit>>> GetUnitsTheCompositeUnitIsBuiltOn(IStringEquation stringEquation)
        {
            IEnumerable<BuiltUnitCategory> compositeResult = await GetByIdAsync(stringEquation.EquationArguments);
            if (compositeResult == null || compositeResult.Count() != stringEquation.EquationArguments.Count())
            {
                LogNotFoundUnits(stringEquation, compositeResult);
            }
            return compositeResult
                .ToDictionary(cat => cat.Name, cat => cat.Units);
        }

        private IStringEquation CreateCompositeUnitEquation(UnitCategory blueprint)
        {
            IStringEquation stringEquation;
            try
            {
                stringEquation = StringEquationFactory.CreateStringEquation(blueprint.CompositeEquation);
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(CreateCompositeUnitsAsync), "Failed to build composite equation!");
                Logger.LogError("Inner Exception", ex.ToString());
                throw;
            }

            return stringEquation;
        }

        private Stack<BuiltUnit> MakeCompositeUnitsFromBaseUnits(UnitCategory blueprint, Dictionary<BuiltUnitSystem, Dictionary<string, BuiltUnit>> unitSystems, Dictionary<string, double> exponents)
        {
            Stack<BuiltUnit> compositeUnits = new Stack<BuiltUnit>();
            foreach (var system in unitSystems.Keys)
            {
                try
                {
                    compositeUnits.Push(new BuiltUnit()
                    {
                        Name = CreateCompositeName(exponents, unitSystems[system]),
                        Symbol = CreateCompositeSymbol(exponents, unitSystems[system]),
                        ConvertToSi = CreateConvertToSi(exponents, unitSystems[system]),
                        ConvertFromSi = CreateConvertFromSi(exponents, unitSystems[system]),
                        UnitSystems = new BuiltUnitSystem[] { system },
                        OwnerName = blueprint.Owner.Name
                    });
                }
                catch (Exception e)
                {
                    Logger.LogError(nameof(CreateCompositeUnitsAsync), e.ToString());
                    throw;
                }
            }

            return compositeUnits;
        }

        private Dictionary<BuiltUnitSystem, Dictionary<string, BuiltUnit>> FindAllCompositeUnitPermutations(Dictionary<string, IEnumerable<BuiltUnit>> unitCategoryToUnitsMap)
        {
            var unitSystems = new Dictionary<BuiltUnitSystem, Dictionary<string, BuiltUnit>>();

            foreach (KeyValuePair<string, IEnumerable<BuiltUnit>> item in unitCategoryToUnitsMap)
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
                            unitSystems.Add(system, new Dictionary<string, BuiltUnit>());
                        }

                        if (unitSystems[system].ContainsKey(item.Key))
                        {
                            Logger.LogError("CreateCompositeUnits", $"More than one unit using {system.Name} as a system!");
                            throw new UnitCategoryException(string.Format(LibraryResources.MoreOneUnitUsingSystem, system.Name));

                        }
                        unitSystems[system].Add(item.Key, unit);
                    }
                }
            }

            return unitSystems;
        }

        private void LogNotFoundUnits(IStringEquation stringEquation, IEnumerable<BuiltUnitCategory> compositeResult)
        {
            StringBuilder sb = new StringBuilder(128);
            sb.Append("We were looking for ");
            foreach (string arg in stringEquation.EquationArguments)
            {
                sb.Append($"[{arg}] ");
            }

            if (compositeResult == null || compositeResult.Count() == 0)
            {
                sb.Append(" but we found nothing!");
            }
            else
            {
                sb.Append(" but we only found ");
                foreach (var obj in compositeResult)
                {
                    sb.Append($"[{obj.Name}] ");
                }
            }
            Logger.LogError(nameof(CreateCompositeUnitsAsync), sb.ToString());
        }



        private string CreateCompositeName(Dictionary<string, double> exponents, Dictionary<string, BuiltUnit> units)
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


        private string CreateCompositeSymbol(Dictionary<string, double> exponents, Dictionary<string, BuiltUnit> units)
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



        private IStringEquation CreateConvertToSi(Dictionary<string, double> exponents, Dictionary<string, BuiltUnit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= Math.Pow(units[system].ConvertToSi.Evaluate(1), exponents[system]);
            }
            return StringEquationFactory.CreateStringEquation($"$0 * { factor }");
        }

        private IStringEquation CreateConvertFromSi(Dictionary<string, double> exponents, Dictionary<string, BuiltUnit> units)
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

        private EngineeringMathContext DbContext { get; }


        private IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }
    }
}
