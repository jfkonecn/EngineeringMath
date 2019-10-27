using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltFunctionOutputValueLink
    {
        public string ParentFunctionName { get; internal set; }
        public string LinkFunctionName { get; internal set; }
        public string LinkOutputName { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is BuiltFunctionOutputValueLink equation)
            {
                return ParentFunctionName == equation.ParentFunctionName &&
                    LinkOutputName == equation.LinkOutputName &&
                    LinkFunctionName == equation.LinkFunctionName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1052276913;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ParentFunctionName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LinkFunctionName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LinkOutputName);
            return hashCode;
        }
    }
}
