using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EngineeringMath.Repository
{
    public class DSContainer
    {
        private DSContainer()
        {
        }
        public static void Bootstrap(Func<StreamReader> defaultDSXMLStreamBuilder)
        {
            Instance = new EngineeringMathDS();
            Instance.ReadFromStream(defaultDSXMLStreamBuilder);
        }
        public static EngineeringMathDS Instance { get; private set; }

        public static IRepository<Unit> Units;
    }
}
