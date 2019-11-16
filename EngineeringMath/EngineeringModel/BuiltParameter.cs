using EngineeringMath.Model;
using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltParameter : IBuiltModel
    {
        public int Id { get; internal set; }

        public string ParameterName { get; internal set; }

        public string FunctionName { get; internal set; }

        public Type Type { get; internal set; }

        public BuiltUnitCategory UnitCategory { get; internal set; }

        public IStringEquation ValueConditions { get; internal set; }

        public ICollection<string> ValueLinks { get; internal set; }
        
        public ICollection<BuiltFunctionOutputValueLink> FunctionLinks { get; internal set; }

        public double Value { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is BuiltParameter parameter)
            {
                return Id == parameter.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
