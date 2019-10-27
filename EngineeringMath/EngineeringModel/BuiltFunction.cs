using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltFunction
    {
        public string Name { get; internal set; }
        public string OwnerName { get; internal set; }
        public IEnumerable<BuiltEquation> Equations { get; internal set; }
        public IEnumerable<BuiltParameter> Parameters { get; internal set; }
        public override bool Equals(object obj)
        {
            if (obj is BuiltFunction func)
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
