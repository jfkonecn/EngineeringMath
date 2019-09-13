using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EngineeringMath.Model
{
    public class UnitDB
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
        public ICollection<UnitSystemDB> UnitSystems { get; set; }
        [Required]
        public OwnerDB Owner { get; set; }
        [Required]
        public bool IsOnAbsoluteScale { get; set; }
    }
}
