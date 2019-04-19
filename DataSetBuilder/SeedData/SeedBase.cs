using EngineeringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetBuilder.SeedData
{
    public abstract class SeedBase : ISeed
    {
        protected EngineeringMathDS Ds { get; }

        public SeedBase(EngineeringMathDS ds)
        {
            Ds = ds;
        }
        public abstract void SeedData();
    }
}
