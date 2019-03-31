using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.DBObject
{
    public class Unit
    {
        public Guid UnitId { get; set; }
        public Guid UnitSystemId { get; set; }
        public Guid UnitCategoryId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string ConvertToSi { get; set; }
        public string ConvertFromSi { get; set; }
        public bool IsUserDefined { get; set; }
        public bool IsOnAbsoluteScale { get; set; }
    }
}
