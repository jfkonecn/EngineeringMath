using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringEquation
    {
        public string Name { get; internal set; }
        public string FunctionName { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is EngineeringEquation equation)
            {
                return equation.Name == Name && equation.FunctionName == FunctionName;
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
