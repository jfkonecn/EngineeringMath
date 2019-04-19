using EngineeringMath;
using System;
using System.Collections.Generic;
using static EngineeringMath.Resources.LibraryResources;
using System.Text;

namespace DataSetBuilder.SeedData
{
    public class UnitCategorySeed : SeedBase
    {

        public UnitCategorySeed(EngineeringMathDS ds) : base(ds)
        {
        }

        public override void SeedData()
        {
            Ds.UnitCategory.AddUnitCategoryRow(nameof(Length), null);
            Ds.UnitCategory.AddUnitCategoryRow(nameof(Area), $"{nameof(Length)} * {nameof(Length)}");
        }
    }
}
