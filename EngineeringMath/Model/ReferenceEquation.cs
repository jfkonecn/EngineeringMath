using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class ReferenceEquation : IDBase
    {
        public Function Function { get; set; }
        public ReferenceTable ReferenceTable { get; set; }
        public IEnumerable<string> Inputs { get; set; }
        public IEnumerable<string> Outputs { get; set; }
    }
}
