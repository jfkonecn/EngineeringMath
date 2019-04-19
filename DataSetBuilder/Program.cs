using DataSetBuilder.SeedData;
using EngineeringMath;
using System;
using System.Data;
using System.Configuration;

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
            set.SaveToFile(ConfigurationManager.AppSettings["DBSavePath"]);
            Console.ReadLine();
        }
    }
}
