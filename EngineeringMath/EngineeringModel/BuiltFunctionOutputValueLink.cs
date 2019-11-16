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
            if (obj is BuiltFunctionOutputValueLink link)
            {
                return Id == link.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
