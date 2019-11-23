using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltUnitSystem : IBuiltModel
    {
        public int Id { get; internal set; }

        public BuiltUnitSystem()
        {

        }

        public BuiltUnitSystem(string name, string abbreviation, string ownerName)
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
            if (obj is BuiltUnitSystem builtUnitSystem)
            {
                return Id == builtUnitSystem.Id;
            }
            else if (obj is UnitSystem unitSystem)
            {
                return Id == unitSystem.UnitSystemId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
