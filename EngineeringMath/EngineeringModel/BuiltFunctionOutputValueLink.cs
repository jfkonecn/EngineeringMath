using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltFunctionOutputValueLink : IBuiltModel
    {
        public int Id { get; internal set; }

        public string ParentFunctionName { get; internal set; }

        public string LinkFunctionName { get; internal set; }

        public string LinkOutputName { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is BuiltFunctionOutputValueLink builtLink)
            {
                return Id == builtLink.Id;
            }
            else if(obj is FunctionOutputValueLink link)
            {
                return Id == link.FunctionOutputValueLinkId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
