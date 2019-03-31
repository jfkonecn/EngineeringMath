using EngineeringMath.DBObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataSetBuilder.Table
{
    public class UnitTable : BaseTable<Unit>
    {
        public UnitTable()
        {
            
        }

        protected override IEnumerable<Unit> GetSeedData()
        {
            yield return new Unit()
            {
                
            };
        }
    }
}
