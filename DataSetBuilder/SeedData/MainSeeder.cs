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
            yield return new UnitSeed(ds);
        }
    }
}
