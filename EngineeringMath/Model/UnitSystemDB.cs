using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class UnitSystemDB
    {
        public int UnitSystemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abbreviation { get; set; }

        public ICollection<UnitSystemDB> Children { get; set; }
        public ICollection<UnitDB> Units { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
