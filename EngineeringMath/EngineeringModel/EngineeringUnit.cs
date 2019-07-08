using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class EngineeringUnit
    {

        public string Name { get; set; }
        public string Symbol { get; set; }
        public IStringEquation ConvertToSi { get; set; }
        public IStringEquation ConvertFromSi { get; set; }
        public ICollection<string> UnitSystems { get; set; }
    }
}
