using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class Function
    {
        public int FunctionId { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Equation> Equations { get; set; }
        public ICollection<Parameter> Parameters { get; set; }
        public ICollection<FunctionCategoryFunction> FunctionCategoryFunctions { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
