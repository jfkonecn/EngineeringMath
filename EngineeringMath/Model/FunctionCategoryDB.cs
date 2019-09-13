using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionCategoryDB
    {
        public int FunctionCategoryId { get; set; }
        public ICollection<FunctionCategoryDB> ChildFunctionCategories { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<FunctionDB> Functions { get; set; }
    }
}
