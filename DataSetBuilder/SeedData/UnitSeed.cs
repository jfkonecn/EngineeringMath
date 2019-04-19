using EngineeringMath;
using static EngineeringMath.Resources.LibraryResources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataSetBuilder.SeedData
{
    public class UnitSeed : SeedBase
    {

        public UnitSeed(EngineeringMathDS ds) : base(ds)
        {
            USCS = GetSysByName(nameof(USCSFullName));
            SI = GetSysByName(nameof(SIFullName));
            Metric = GetSysByName(nameof(MetricFullName));
            Imp = GetSysByName(nameof(ImperialFullName));
        }

        public override void SeedData()
        {
            AddLength();

        }

        private void AddLength()
        {
            var sys = GetCatByName(nameof(Length));

            AddToSI(Ds.Unit.AddUnitRow(sys, nameof(Meters), "m", "$0", "$0", false, false));

        }


        private EngineeringMathDS.UnitCategoryRow GetCatByName(string name)
        {
            return Ds.UnitCategory.Where(x => x.Name == name).Single();
        }


        private readonly EngineeringMathDS.UnitSystemRow USCS;
        private readonly EngineeringMathDS.UnitSystemRow SI;
        private readonly EngineeringMathDS.UnitSystemRow Metric;
        private readonly EngineeringMathDS.UnitSystemRow Imp;

        enum UnitSystem
        {
            USCS,
            SI,
            Metric,
            Imp,
        }


        private void AddToSI(EngineeringMathDS.UnitRow unit)
        {
            AddToUnitUnitSystem(unit, UnitSystem.Metric, UnitSystem.SI);
        }

        private void AddToUSCS(EngineeringMathDS.UnitRow unit)
        {
            AddToUnitUnitSystem(unit, UnitSystem.Imp, UnitSystem.USCS);
        }

        private void AddToMetric(EngineeringMathDS.UnitRow unit)
        {
            AddToUnitUnitSystem(unit, UnitSystem.Metric);
        }

        private void AddToImp(EngineeringMathDS.UnitRow unit)
        {
            AddToUnitUnitSystem(unit, UnitSystem.Imp);
        }

        private void AddToAll(EngineeringMathDS.UnitRow unit)
        {
            AddToUnitUnitSystem(unit, UnitSystem.Metric, UnitSystem.SI, UnitSystem.Imp, UnitSystem.USCS);
        }

        private void AddToUnitUnitSystem(EngineeringMathDS.UnitRow unit, params UnitSystem[] unitSystems)
        {
            foreach (UnitSystem system in unitSystems)
            {
                switch (system)
                {
                    case UnitSystem.USCS:
                        Ds.Unit_UnitSystem.AddUnit_UnitSystemRow(unit, USCS);
                        break;
                    case UnitSystem.SI:
                        Ds.Unit_UnitSystem.AddUnit_UnitSystemRow(unit, SI);
                        break;
                    case UnitSystem.Metric:
                        Ds.Unit_UnitSystem.AddUnit_UnitSystemRow(unit, Metric);
                        break;
                    case UnitSystem.Imp:
                        Ds.Unit_UnitSystem.AddUnit_UnitSystemRow(unit, Imp);
                        break;
                    default:
                        throw new ArgumentException($"I don't know {system.ToString()}");
                }
            }
        }

        private EngineeringMathDS.UnitSystemRow GetSysByName(string name)
        {
            return Ds.UnitSystem.Where(x => x.Name == name).Single();
        }
    }
}
