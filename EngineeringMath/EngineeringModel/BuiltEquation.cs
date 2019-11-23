using EngineeringMath.Model;
using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltEquation : IBuiltModel
    {
        public int Id { get; internal set; }

        public string Name

        {
            get
            {
                return $"{FunctionName}.{OutputName}";
            }
        }

        public IStringEquation Formula { get; internal set; }

        public int FunctionId { get; set; }

        public string FunctionName { get; internal set; }

        public string OutputName { get; internal set; }

        public string OwnerName { get; internal set; }

        public override bool Equals(object obj)
        {
            if(obj is BuiltEquation builtEquation)
            {
                return Id == builtEquation.Id;
            }
            else if(obj is Equation equation)
            {
                return Id == equation.EquationId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
