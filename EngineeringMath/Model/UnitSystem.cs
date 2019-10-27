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

        public int? ParentId { get; set; }
        public UnitSystem Parent { get; set; }
        public ICollection<UnitSystem> Children { get; set; }

        public ICollection<UnitSystemUnit> UnitSystemUnits { get; set; }

        public ICollection<Unit> Units 
        { 
            set
            {
                if (value == null) UnitSystemUnits = null;

                UnitSystemUnits = new List<UnitSystemUnit>();
                foreach (Unit unit in value)
                {
                    UnitSystemUnits.Add(new UnitSystemUnit()
                    {
                        Unit = unit,
                        UnitSystem = this
                    });
                }
            } 
        }
        [Required]
        public Owner Owner { get; set; }
    }
}
