using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ParameterDB
    {
        public int ParameterId { get; set; }
        [Required]
        public FunctionDB Function { get; set; }
        [Required]
        public ParameterTypeDB ParameterType { get; set; }
        public UnitCategoryDB UnitCategory { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ParameterValueLinkDB> ValueLinks { get; set; }
        public ICollection<FunctionOutputValueLinkDB> FunctionLinks { get; set; }
        public string ValueConditions { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
