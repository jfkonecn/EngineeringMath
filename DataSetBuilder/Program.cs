using DataSetBuilder.Table;
using System;
using System.Data;

namespace DataSetBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create DataSet instance.
            DataSet set = new DataSet("EngineeringMath");
            // Add new table.
            set.Tables.Add(new UnitTable());
            // Write xml data.
            Console.WriteLine(set.GetXml());
        }
    }
}
