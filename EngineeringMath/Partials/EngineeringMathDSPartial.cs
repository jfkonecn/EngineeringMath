using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Security;
using System.Data;

namespace EngineeringMath
{
    public partial class EngineeringMathDS
    {

        /// <summary>
        /// Handles opening and closing the stream
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SecurityException"></exception>
        public void SaveToStream(Func<StreamWriter> streamBuilder)
        {
            using (StreamWriter stream = streamBuilder())
            {
                WriteXml(stream);
            }
        }

        /// <summary>
        /// Handles opening and closing the stream
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SecurityException"></exception>
        public EngineeringMathDS ReadFromStream(Func<StreamReader> streamBuilder)
        {
            EngineeringMathDS ds = new EngineeringMathDS();
            
            using (StreamReader stream = streamBuilder())
            {
                /**
                 * If you call ReadXml to load a very large file, you may encounter slow performance. 
                 * To ensure best performance for ReadXml, on a large file, call the BeginLoadData 
                 * method for each table in the DataSet, and then call ReadXml. Finally, call 
                 * EndLoadData for each table in the DataSet, as shown in the 
                 * following example.
                 * https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/dataset-datatable-dataview/loading-a-dataset-from-xml
                 */
                foreach (DataTable dataTable in ds.Tables)
                    dataTable.BeginLoadData();

                ds.ReadXml(stream);

                foreach (DataTable dataTable in ds.Tables)
                    dataTable.EndLoadData();
            }
            return ds;
        }
    }
}
