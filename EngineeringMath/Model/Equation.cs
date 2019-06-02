using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class Equation : IDBase
    {
        public Function Function { get; set; }
        public string Formula { get; set; }
        public string OutputName { get; set; }
    }
}
