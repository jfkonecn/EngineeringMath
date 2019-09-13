using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EngineeringMath.Model
{
    public class EquationDB
    {
        public int EquationId { get; set; }
        public FunctionDB Function { get; set; }
        [Required]
        public string Formula { get; set; }
        [Required]
        public string OutputName { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
