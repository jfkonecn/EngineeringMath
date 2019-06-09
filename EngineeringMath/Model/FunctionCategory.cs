using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionCategory
    {
        public int FunctionCategoryId { get; set; }
        public FunctionCategory ParentFunction { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Function> Functions { get; set; }
    }
}
