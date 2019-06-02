using EngineeringMath.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EngineeringMath
{
    public static class BootStrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultDSXMLStreamBuilder">A creates a stream which points to the location of the default 
        /// Engineering Math data set XML</param>
        public static void BootStrap(Func<StreamReader> defaultDSXMLStreamBuilder)
        {
            DSContainer.Bootstrap(defaultDSXMLStreamBuilder);
        }

    }
}
