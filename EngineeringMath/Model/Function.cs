using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class Function : IDBase
    {
        public FunctionCategory FunctionCategory { get; set; }
        public string Name { get; set; }
        public bool IsUserDefined { get; set; }
    }
}
