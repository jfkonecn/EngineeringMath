using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltUnit
    {

        public string Name { get; internal set; }
        public string Symbol { get; internal set; }
        public IStringEquation ConvertToSi { get; internal set; }
        public IStringEquation ConvertFromSi { get; internal set; }
        public ICollection<BuiltUnitSystem> UnitSystems { get; internal set; }
        public string OwnerName { get; internal set; }

        public override bool Equals(object obj)
        {
            if(obj is BuiltUnitSystem unitSystem)
            {
                return unitSystem.Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
