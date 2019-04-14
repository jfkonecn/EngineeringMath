using DataSetBuilder.SeedData;
using DataSetBuilder.Table;
using EngineeringMath;
using System;
using System.Data;

namespace DataSetBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create DataSet instance.
            EngineeringMathDS set = new EngineeringMathDS();
            MainSeeder.SeedAll(set);
            // Write xml data.
            Console.WriteLine(set.GetXml());
        }
    }
}
