using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringFunction
    {
        public string Name { get; internal set; }
        public string OwnerName { get; internal set; }
        public IEnumerable<EngineeringEquation> Equations { get; internal set; }
        public IEnumerable<EngineeringParameter> Parameters { get; internal set; }
        public override bool Equals(object obj)
        {
            if (obj is EngineeringFunction func)
            {
                return func.Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
