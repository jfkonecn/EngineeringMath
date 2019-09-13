using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace EngineeringMath.Model
{
    public class UnitCategoryDB
    {
        public int UnitCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UnitDB> Units { get; set; }
        [Required]
        public string CompositeEquation { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
    }
}
