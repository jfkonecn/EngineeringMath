using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringUnit
    {

        public string Name { get; internal set; }
        public string Symbol { get; internal set; }
        public IStringEquation ConvertToSi { get; internal set; }
        public IStringEquation ConvertFromSi { get; internal set; }
        public ICollection<string> UnitSystems { get; internal set; }
        public string OwnerName { get; internal set; }
    }
}
