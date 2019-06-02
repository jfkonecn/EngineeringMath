using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EngineeringMath.Model
{
    public class Unit : IDBase
    {
        public int UnitSystemId { get; set; }
        public UnitCategory UnitCategory { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string ConvertToSi { get; set; }
        public string ConvertFromSi { get; set; }
        public bool IsUserDefined { get; set; }
        public bool IsOnAbsoluteScale { get; set; }
    }
}
