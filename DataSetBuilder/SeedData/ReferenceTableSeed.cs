using System;
using System.Collections.Generic;
using static EngineeringMath.Resources.LibraryResources;
using System.Text;
using EngineeringMath;

namespace DataSetBuilder.SeedData
{
    public class ReferenceTableSeed :SeedBase
    {
        public ReferenceTableSeed(EngineeringMathDS ds) : base(ds)
        {
        }

        public override void SeedData()
        {
            Ds.ReferenceTable.AddReferenceTableRow("EngineeringLookupTables");
        }
    }
}
