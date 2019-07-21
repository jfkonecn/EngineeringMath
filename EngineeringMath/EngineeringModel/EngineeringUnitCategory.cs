using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringUnitCategory
    {
        public string Name { get; internal set; }
        public IEnumerable<EngineeringUnit> Units { get; internal set; }
        public string OwnerName { get; internal set; }
        public override bool Equals(object obj)
        {
            if(obj is EngineeringUnitCategory cat)
            {
                return cat.Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
