using EngineeringMath;
using System;
using System.Collections.Generic;
using System.Text;
using static EngineeringMath.Resources.LibraryResources;

namespace DataSetBuilder.SeedData
{
    class UnitSystemSeed : SeedBase
    {

        public UnitSystemSeed(EngineeringMathDS ds) : base(ds)
        {
        }

        public override void SeedData()
        {
            Ds.UnitSystem.AddUnitSystemRow(nameof(MetricFullName), nameof(MetricAbbrev));
            Ds.UnitSystem.AddUnitSystemRow(nameof(ImperialFullName), nameof(ImperialAbbrev));
            Ds.UnitSystem.AddUnitSystemRow(nameof(SIFullName), nameof(SIAbbrev));
            Ds.UnitSystem.AddUnitSystemRow(nameof(USCSFullName), nameof(USCSAbbrev));
        }

  



    }
}
