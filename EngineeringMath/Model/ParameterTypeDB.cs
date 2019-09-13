using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ParameterTypeDB
    {
        public int ParameterTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
