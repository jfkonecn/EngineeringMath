using EngineeringMath.Model;
using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class Parameter
    {
        public string EquationName { get; internal set; }

        public string FunctionName { get; internal set; }
        /// <summary>
        /// Unique Name
        /// </summary>
        public string KeyName
        {
            get
            {
                return $"{FunctionName}.{EquationName}";
            }
        }

        public Type Type { get; internal set; }

        public UnitCategory UnitCategory { get; internal set; }

        public IStringEquation ValueConditions { get; internal set; }

        public ICollection<string> ValueLinks { get; internal set; }
        
        public ICollection<FunctionOutputValueLink> FunctionLinks { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is Parameter parameter)
            {
                return parameter.EquationName == EquationName && parameter.FunctionName == FunctionName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1676595688;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EquationName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FunctionName);
            return hashCode;
        }
    }
}
