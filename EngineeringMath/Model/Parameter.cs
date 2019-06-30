using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class Parameter
    {
        public int ParameterId { get; set; }
        [Required]
        public Function Function { get; set; }
        [Required]
        public ParameterType ParameterType { get; set; }
        public UnitCategory UnitCategory { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ParameterValueLink> ValueLinks { get; set; }
        public ICollection<FunctionOutputValueLink> FunctionLinks { get; set; }
        public string ValueConditions { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
