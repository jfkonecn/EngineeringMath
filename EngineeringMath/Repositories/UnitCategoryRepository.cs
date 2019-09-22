using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Resources;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class UnitCategoryRepository : ReadonlyCacheRepositoryBase<string, UnitCategory, UnitCategoryDB>
    {
        public UnitCategoryRepository(
            IReadonlyRepository<UnitCategoryDB> unitCategoryRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(unitCategoryRepository, logger)
        {
            UnitCategoryDBRepository = unitCategoryRepository;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }



        protected override string GetKey(UnitCategory obj)
        {
            return obj.Name;
        }

        protected override string GetKey(UnitCategoryDB obj)
        {
            return obj.Name;
        }

        protected async override Task<IEnumerable<UnitCategory>> BuildTAsync(IEnumerable<UnitCategoryDB> blueprints)
        {
            Stack<UnitCategory> unitCategories = new Stack<UnitCategory>();
            foreach (UnitCategoryDB blueprint in blueprints)
            {
                var newUnits = new List<Unit>();
                IEnumerable<Unit> compositeResult = await CreateCompositeUnitsAsync(blueprint);
                if(compositeResult != null)
                {
                    newUnits.AddRange(compositeResult);
                }


                foreach (UnitDB unit in blueprint.Units ?? new List<UnitDB>())
                {
                    var unitSys = new List<UnitSystem>();
                    foreach (var system in unit.UnitSystems)
                    {
                        unitSys.Add(new UnitSystem()
                        {
                            Abbreviation = system.Abbreviation.TryToFindStringInLibraryResources(),
                            Name = system.Name.TryToFindStringInLibraryResources(),
                            OwnerName = system.Owner.Name,
                        });
                    }
                    try
                    {
                        newUnits.Add(new Unit()
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
                unitCategories.Push(new UnitCategory()
                {
                    Name = blueprint.Name,
                    Units = newUnits,
                    OwnerName = blueprint.Owner.Name
                });
            }
            return unitCategories;
        }

        private async Task<IEnumerable<Unit>> CreateCompositeUnitsAsync(UnitCategoryDB blueprint)
        {
            Stack<Unit> compositeUnits = null;
            if (!string.IsNullOrEmpty(blueprint.CompositeEquation))
            {
                IStringEquation stringEquation = CreateCompositeUnitEquation(blueprint);
                Dictionary<string, IEnumerable<Unit>> unitCategoryToUnitsMap = await GetUnitsTheCompositeUnitIsBuiltOn(stringEquation);

                Dictionary<UnitSystem, Dictionary<string, Unit>> unitSystems = FindAllCompositeUnitPermutations(unitCategoryToUnitsMap);

                Dictionary<string, double> exponents = GetExponents(stringEquation);

                compositeUnits = MakeCompositeUnitsFromBaseUnits(blueprint, unitSystems, exponents);
            }
            return compositeUnits;

        }

        private async Task<Dictionary<string, IEnumerable<Unit>>> GetUnitsTheCompositeUnitIsBuiltOn(IStringEquation stringEquation)
        {
            IEnumerable<UnitCategory> compositeResult = await GetByIdAsync(stringEquation.EquationArguments);
            if (compositeResult == null || compositeResult.Count() != stringEquation.EquationArguments.Count())
            {
                LogNotFoundUnits(stringEquation, compositeResult);
            }
            return compositeResult
                .ToDictionary(cat => cat.Name, cat => cat.Units);
        }

        private IStringEquation CreateCompositeUnitEquation(UnitCategoryDB blueprint)
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

        private Stack<Unit> MakeCompositeUnitsFromBaseUnits(UnitCategoryDB blueprint, Dictionary<UnitSystem, Dictionary<string, Unit>> unitSystems, Dictionary<string, double> exponents)
        {
            Stack<Unit> compositeUnits = new Stack<Unit>();
            foreach (var system in unitSystems.Keys)
            {
                try
                {
                    compositeUnits.Push(new Unit()
                    {
                        Name = CreateCompositeName(exponents, unitSystems[system]),
                        Symbol = CreateCompositeSymbol(exponents, unitSystems[system]),
                        ConvertToSi = CreateConvertToSi(exponents, unitSystems[system]),
                        ConvertFromSi = CreateConvertFromSi(exponents, unitSystems[system]),
                        UnitSystems = new UnitSystem[] { system },
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

        private Dictionary<UnitSystem, Dictionary<string, Unit>> FindAllCompositeUnitPermutations(Dictionary<string, IEnumerable<Unit>> unitCategoryToUnitsMap)
        {
            var unitSystems = new Dictionary<UnitSystem, Dictionary<string, Unit>>();

            foreach (KeyValuePair<string, IEnumerable<Unit>> item in unitCategoryToUnitsMap)
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
                            unitSystems.Add(system, new Dictionary<string, Unit>());
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

        private void LogNotFoundUnits(IStringEquation stringEquation, IEnumerable<UnitCategory> compositeResult)
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



        private string CreateCompositeName(Dictionary<string, double> exponents, Dictionary<string, Unit> units)
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


        private string CreateCompositeSymbol(Dictionary<string, double> exponents, Dictionary<string, Unit> units)
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



        private IStringEquation CreateConvertToSi(Dictionary<string, double> exponents, Dictionary<string, Unit> units)
        {
            double factor = 1;
            foreach (var system in exponents.Keys)
            {
                factor *= Math.Pow(units[system].ConvertToSi.Evaluate(1), exponents[system]);
            }
            return StringEquationFactory.CreateStringEquation($"$0 * { factor }");
        }

        private IStringEquation CreateConvertFromSi(Dictionary<string, double> exponents, Dictionary<string, Unit> units)
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



        private IReadonlyRepository<UnitCategoryDB> UnitCategoryDBRepository { get; }
        public IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }
    }
}
