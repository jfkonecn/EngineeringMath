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
        }

        public override void SeedData()
        {
            AddLength();

        }

        private void AddLength()
        {
            var sys = GetCatByName(nameof(Length));
            Ds.Unit.AddUnitRow(sys, nameof(Meters), "m", "$0", "$0", false, false);
        }


        private EngineeringMathDS.UnitCategoryRow GetCatByName(string name)
        {
            return Ds.UnitCategory.Where(x => x.Name == name).Single();
        }
    }
}
