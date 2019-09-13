using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class UnitSystem
    {
        public UnitSystem()
        {

        }

        public UnitSystem(string name, string abbreviation, string ownerName)
        {
            Name = name;
            Abbreviation = abbreviation;
            OwnerName = ownerName;
        }
        public string Name { get; internal set; }
        public string Abbreviation { get; internal set; }
        public string OwnerName { get; internal set; }
        public override bool Equals(object obj)
        {
            if (obj is UnitSystem system)
            {
                return Name == system.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
