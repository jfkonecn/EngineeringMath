using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    [SetUpFixture]
    public class RepositoryTestSetup
    {
        public static EngineeringMathContext Context { get; private set; }

        [OneTimeSetUp]
        public void SetupTests()
        {
            var options = new DbContextOptionsBuilder<EngineeringMathContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0")
                .EnableSensitiveDataLogging()
                .Options;

            Context = new EngineeringMathContext(options);
            Context.Database.EnsureCreated();
        }


        [OneTimeTearDown]
        public void TearDownTests()
        {
            Context = null;
        }
    }
}
