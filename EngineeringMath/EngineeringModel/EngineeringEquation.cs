using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringEquation
    {
        public int EquationId { get; set; }
        public IStringEquation Formula { get; set; }
        public string FunctionName { get; set; }
        public string OutputName { get; set; }
        public string OwnerName { get; set; }
        public override bool Equals(object obj)
        {
            if(obj is EngineeringEquation equation)
            {
                return EquationId == equation.EquationId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return -178868146 + EquationId.GetHashCode();
        }
    }
}
