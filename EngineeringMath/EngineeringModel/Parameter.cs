using EngineeringMath.Model;
using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class Parameter
    {
        public string Name { get; internal set; }

        public string FunctionName { get; internal set; }

        public Type Type { get; internal set; }

        public UnitCategory UnitCategory { get; internal set; }

        public IStringEquation ValueConditions { get; internal set; }

        public ICollection<string> ValueLinks { get; internal set; }
        
        public ICollection<FunctionOutputValueLink> FunctionLinks { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is Parameter parameter)
            {
                return parameter.Name == Name && parameter.FunctionName == FunctionName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1676595688;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FunctionName);
            return hashCode;
        }
    }
}
