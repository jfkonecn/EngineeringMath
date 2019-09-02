using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringEquation
    {
        public IStringEquation Formula { get; set; }
        public string FunctionName { get; set; }
        public string OutputName { get; set; }
        public string OwnerName { get; set; }
        public override bool Equals(object obj)
        {
            if(obj is EngineeringEquation equation)
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
