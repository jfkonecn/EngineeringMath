using EngineeringMath;
using System;
using System.Collections.Generic;
using static EngineeringMath.Resources.LibraryResources;
using System.Text;

namespace DataSetBuilder.SeedData
{
    public class FunctionCategorySeed : SeedBase
    {
        public FunctionCategorySeed(EngineeringMathDS ds) : base(ds)
        {
        }

        public override void SeedData()
        {
            Ds.FunctionCategory.AddFunctionCategoryRow(null, nameof(Utility));
        }
    }
}
