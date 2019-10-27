using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EngineeringMath.Model
{
    public class Equation
    {
        public string Name
        {
            get
            {
                return $"{Function.Name}.{OutputName}";
            }
        }
        public int EquationId { get; set; }
        public Function Function { get; set; }
        [Required]
        public string Formula { get; set; }
        [Required]
        public string OutputName { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
