using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionCategory : IDBase
    {
        public FunctionCategory ParentFunction { get; set; }
        public string Name { get; set; }
    }
}
