using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionDB
    {
        public int FunctionId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<EquationDB> Equations { get; set; }
        public ICollection<ParameterDB> Parameters { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
