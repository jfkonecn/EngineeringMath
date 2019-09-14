using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class Equation
    {
        public string Name
        {
            get
            {
                return $"{FunctionName}.{OutputName}";
            }
        }
        public IStringEquation Formula { get; internal set; }
        public string FunctionName { get; internal set; }
        public string OutputName { get; internal set; }
        public string OwnerName { get; internal set; }
        public override bool Equals(object obj)
        {
            if(obj is Equation equation)
            {
                return OutputName == equation.OutputName &&
                    FunctionName == equation.FunctionName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1052276913;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FunctionName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OutputName);
            return hashCode;
        }
    }
}
