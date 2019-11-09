using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringMath.Model
{
    public class Unit
    {
        public int UnitId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string ConvertToSi { get; set; }
        [Required]
        public string ConvertFromSi { get; set; }
        public ICollection<UnitSystemUnit> UnitSystemUnits { get; set; }
        public int UnitCategoryId { get; set; }
        [Required]
        public UnitCategory UnitCategory { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public Owner Owner { get; set; }
        [Required]
        public bool IsOnAbsoluteScale { get; set; }
    }
}
