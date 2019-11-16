using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltUnitCategory : IBuiltModel
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public IEnumerable<BuiltUnit> Units { get; internal set; }

        public string OwnerName { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is BuiltUnitCategory unitCat)
            {
                return Id == unitCat.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
