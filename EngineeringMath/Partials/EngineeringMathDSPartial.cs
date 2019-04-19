using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Security;

namespace EngineeringMath
{
    public partial class EngineeringMathDS
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SecurityException"></exception>
        public void SaveToFile(string path)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                WriteXml(file);
            }
        }
    }
}
