using EngineeringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetBuilder.SeedData
{
    public static class MainSeeder
    {
        public static void SeedAll(EngineeringMathDS ds)
        {
            foreach (var seed in GetSeeds(ds))
            {
                seed.SeedData();
            }
            ds.AcceptChanges();
        }

        private static IEnumerable<ISeed> GetSeeds(EngineeringMathDS ds)
        {
            yield return new UnitCategorySeed(ds);
            yield return new UnitSystemSeed(ds);
            yield return new UnitSeed(ds);
            yield return new ParameterTypeSeed(ds);
            yield return new ReferenceTableSeed(ds);
            yield return new FunctionCategorySeed(ds);
            yield return new FunctionSeed(ds);
        }
    }
}
