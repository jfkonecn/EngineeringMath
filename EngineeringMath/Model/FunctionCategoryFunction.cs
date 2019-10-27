using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionCategoryFunction
    {
        public int FunctionId { get; set; }
        public Function Function { get; set; }
        public int FunctionCategoryId { get; set; }
        public FunctionCategory FunctionCategory { get; set; }
    }
}
