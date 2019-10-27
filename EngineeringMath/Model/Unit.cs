using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public ICollection<UnitSystem> UnitSystems
        {
            set
            {
                if (value == null) UnitSystemUnits = null;
                UnitSystemUnits = new List<UnitSystemUnit>();
                UnitSystemUnits = value.Select(x => new UnitSystemUnit()
                {
                    Unit = this,
                    UnitSystem = x,
                }).ToList();
            }
        }
        [Required]
        public Owner Owner { get; set; }
        [Required]
        public bool IsOnAbsoluteScale { get; set; }
    }
}
