using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Owner Owner { get; set; }
        [Required]
        public bool IsOnAbsoluteScale { get; set; }
    }
}
