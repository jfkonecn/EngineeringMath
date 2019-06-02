using DataSetBuilder.SeedData;
using EngineeringMath;
using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Xml;
using EngineeringMath.Repository;

namespace DataSetBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

            BootStrapper.BootStrap(() => new StreamReader(ConfigurationManager.AppSettings["DBSavePath"]));
            // Create DataSet instance.
            EngineeringMathDS set = DSContainer.Instance;
            MainSeeder.SeedAll(set);
            // Write xml data.
            Console.WriteLine(set.GetXml());
            Console.ReadLine();
        }
    }
}
