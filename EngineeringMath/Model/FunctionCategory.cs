using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionCategory
    {
        [Key]
        public int FunctionCategoryId { get; set; }
        public ICollection<FunctionCategory> ChildFunctionCategories { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<FunctionCategoryFunction> FunctionCategoryFunctions { get; set; }
    }
}
