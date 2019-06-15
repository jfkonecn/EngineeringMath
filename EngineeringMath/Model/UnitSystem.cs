using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class UnitSystem
    {
        public int UnitSystemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abbreviation { get; set; }

        public ICollection<UnitSystem> Children { get; set; }
        public ICollection<Unit> Units { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
