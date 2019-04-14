using EngineeringMath;
using static EngineeringMath.Resources.LibraryResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetBuilder.SeedData
{
    public class UnitSeed : ISeed
    {
        private EngineeringMathDS Ds { get; }

        public UnitSeed(EngineeringMathDS ds)
        {
            Ds = ds;
        }

        public void SeedData()
        {
            AddLength();

        }

        private void AddLength()
        {
            var sys = Ds.UnitSystem.AddUnitSystemRow(nameof(Length), null);
            Ds.Unit.AddUnitRow(,)
        }
    }
}
