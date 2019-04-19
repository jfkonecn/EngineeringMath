using EngineeringMath;
using System;
using System.Collections.Generic;
using static EngineeringMath.Resources.LibraryResources;
using System.Text;
using EngineeringMath.Resources;

namespace DataSetBuilder.SeedData
{
    public class ParameterTypeSeed : SeedBase
    {
        public ParameterTypeSeed(EngineeringMathDS ds) : base(ds)
        {
        }

        public override void SeedData()
        {
            Ds.ParameterType.AddParameterTypeRow(nameof(Number));
        }
    }
}
