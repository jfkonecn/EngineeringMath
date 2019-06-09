using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ImportedEquation
    {
        public int ImportedEquationId { get; set; }
        [Required]
        public int ImportedMethod { get; set; }
        public string OutputName { get; set; }
    }
}
