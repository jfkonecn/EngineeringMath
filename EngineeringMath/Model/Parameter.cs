using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class Parameter : IDBase
    {
        public Function Function { get; set; }
        public ParameterType ParameterType { get; set; }
        public UnitCategory UnitCategory { get; set; }
        public string Name { get; set; }
        public string ValueConditions { get; set; }
    }
}
