using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ParameterType
    {
        public int ParameterTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
