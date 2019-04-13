using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineeringMath.EngineeringMathDS;

namespace EngineeringMath.Tests
{
    [TestFixture]
    public class EngineeringMathDbSetTests
    {
        [Test]
        public void ShouldAddItem()
        {
            EngineeringMathDS set = new EngineeringMathDS();
            UnitRow s = set.Unit[0];

        }
    }
}
