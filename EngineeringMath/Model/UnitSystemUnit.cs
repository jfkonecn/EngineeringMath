using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class UnitSystemUnit
    {
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int UnitSystemId { get; set; }
        public UnitSystem UnitSystem { get; set; }

    }
}
