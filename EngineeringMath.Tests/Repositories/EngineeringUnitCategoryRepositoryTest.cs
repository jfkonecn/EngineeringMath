using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using EngineeringMath.Results;
using Moq;
using NUnit.Framework;
using StringMath;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Tests.Mocks;
using EngineeringMath.EngineeringModel;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class EngineeringUnitCategoryRepositoryTest
    {
        private Mock<IReadonlyRepository<UnitCategory>> UnitCategoryRepositoryMock { get; set; }
        private EngineeringUnitCategoryRepository SUT { get; set; }

        private EngineeringMathSeedData Data { get; set; }
        private MockResult<IEnumerable<UnitCategory>> ResultList { get; } = new MockResult<IEnumerable<UnitCategory>>();

        [SetUp]
        public void ClassSetup()
        {
            UnitCategoryRepositoryMock = new Mock<IReadonlyRepository<UnitCategory>>();
            SUT = new EngineeringUnitCategoryRepository(
                UnitCategoryRepositoryMock.Object, 
                new StringEquationFactory(), 
                new ConsoleLogger());
        }


        [Test]
        public void ShouldHandleCorruptedCompositeUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            var result = SUT.GetById("BadComposite");

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleCorruptedUnitsInUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            var result = SUT.GetById("BadUnits");

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleRepositoryError()
        {
            // arrange
            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<UnitCategory, bool>>()))
                .Returns(new RepositoryResult<IEnumerable<UnitCategory>>(RepositoryStatusCode.internalError, null));

            // act
            var result = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleGetAllUnitCategories()
        {
            // arrange
            SetupMockRepositories();

            // act
            var result = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.IsNotNull(result.ResultObject);
            Assert.AreEqual(Data.UnitCategories.Count(), result.ResultObject.Count());
        }


        [Test]
        public void ShouldHandleCacheUnitCategories()
        {
            // arrange
            SetupMockRepositories();

            // act
            var startingResult = SUT.GetAll();
            var cacheResult = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, startingResult.StatusCode);
            Assert.AreEqual(RepositoryStatusCode.success, cacheResult.StatusCode);

            foreach (EngineeringUnitCategory starting in startingResult.ResultObject)
            {
                foreach (EngineeringUnitCategory cached in cacheResult.ResultObject)
                {
                    if(starting.Name == cached.Name)
                    {
                        Assert.AreSame(starting, cached);
                    }
                }
            }
        }

        [Test]
        public void ValidateAreaUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Area));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MeterFullName }{(2d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.MeterAbbrev }{(2d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(2d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(2d).ToSuperScript()}",
                CurrentUnitValue = 21.5278,
                SiValue = 2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateDensityUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Density));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.KilogramsFullName } * { LibraryResources.MeterFullName }{(-3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.KilogramsAbbrev } * { LibraryResources.MeterAbbrev }{(-3d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.PoundsMassFullName } * { LibraryResources.FeetFullName }{(-3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.PoundsMassAbbrev } * { LibraryResources.FeetAbbrev }{(-3d).ToSuperScript()}",
                CurrentUnitValue = 62.4,
                SiValue = 1000,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateEnergyUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Energy));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.JoulesFullName }",
                Symbol = $"{ LibraryResources.JoulesAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.BTUFullName }",
                Symbol = $"{ LibraryResources.BTUAbbrev }",
                CurrentUnitValue = 20,
                SiValue = 21101.1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.KilocaloriesFullName }",
                Symbol = $"{ LibraryResources.KilocaloriesAbbrev }",
                CurrentUnitValue = 5,
                SiValue = 20920,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.KilojoulesFullName }",
                Symbol = $"{ LibraryResources.KilojoulesAbbrev }",
                CurrentUnitValue = 1000,
                SiValue = 1e6,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.ThermsFullName }",
                Symbol = $"{ LibraryResources.ThermsAbbrev }",
                CurrentUnitValue = 2,
                SiValue = 2.11e+8,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateEnthalpyUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Enthalpy));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.JoulesFullName } * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.JoulesAbbrev } * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.BTUFullName } * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.BTUAbbrev } * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 50,
                SiValue = 116.3e3,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateEntropyUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Entropy));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.JoulesFullName } * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()} * { LibraryResources.KelvinFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.JoulesAbbrev } * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()} * { LibraryResources.KelvinAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.BTUFullName } * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()} * { LibraryResources.RankineFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.BTUAbbrev } * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()} * { LibraryResources.RankineAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 5,
                SiValue = 20934,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateIsothermalCompressibilityUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.IsothermalCompressibility));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.PascalsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.PascalsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsForcePerSqInFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.PoundsForcePerSqInAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 5e4,
                SiValue = 7.2518869,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateLengthUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Length));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.MeterFullName,
                Symbol = LibraryResources.MeterAbbrev,
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.FeetFullName,
                Symbol = LibraryResources.FeetAbbrev,
                CurrentUnitValue = 20.2,
                SiValue = 6.15696,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.InchesFullName,
                Symbol = LibraryResources.InchesAbbrev,
                CurrentUnitValue = 196.85,
                SiValue = 5,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.MilesFullName,
                Symbol = LibraryResources.MilesAbbrev,
                CurrentUnitValue = 1.242742,
                SiValue = 2000,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.MillimetersFullName,
                Symbol = LibraryResources.MillimetersAbbrev,
                CurrentUnitValue = 2000,
                SiValue = 2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.CentimetersFullName,
                Symbol = LibraryResources.CentimetersAbbrev,
                CurrentUnitValue = 200,
                SiValue = 2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = LibraryResources.KilometersFullName,
                Symbol = LibraryResources.KilometersAbbrev,
                CurrentUnitValue = 2,
                SiValue = 2000,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateMassUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Mass));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.KilogramsFullName }",
                Symbol = $"{LibraryResources.KilogramsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsMassFullName }",
                Symbol = $"{ LibraryResources.PoundsMassAbbrev }",
                CurrentUnitValue = 11.0231,
                SiValue = 5,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.GramsFullName }",
                Symbol = $"{ LibraryResources.GramsAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MilligramsFullName }",
                Symbol = $"{ LibraryResources.MilligramsAbbrev }",
                CurrentUnitValue = 1e6,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MicrogramsFullName }",
                Symbol = $"{ LibraryResources.MicrogramsAbbrev }",
                CurrentUnitValue = 1e9,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MetricTonsFullName }",
                Symbol = $"{ LibraryResources.MetricTonsAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.OuncesFullName }",
                Symbol = $"{ LibraryResources.OuncesAbbrev }",
                CurrentUnitValue = 35274,
                SiValue = 1e3,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.USTonsFullName }",
                Symbol = $"{ LibraryResources.USTonsAbbrev }",
                CurrentUnitValue = 1.10231252347,
                SiValue = 1000,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidatePowerUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Power));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.WattsFullName }",
                Symbol = $"{LibraryResources.WattsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.HorsepowerFullName }",
                Symbol = $"{ LibraryResources.HorsepowerAbbrev }",
                CurrentUnitValue = 10,
                SiValue = 7457,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.KilowattFullName }",
                Symbol = $"{ LibraryResources.KilowattAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

        }

        [Test]
        public void ValidatePressureUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Pressure));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.PascalsFullName }",
                Symbol = $"{LibraryResources.PascalsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsForcePerSqInFullName }",
                Symbol = $"{ LibraryResources.PoundsForcePerSqInAbbrev }",
                CurrentUnitValue = 20,
                SiValue = 137895,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.BarFullName }",
                Symbol = $"{ LibraryResources.BarAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 100000,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.KilopascalsFullName }",
                Symbol = $"{ LibraryResources.KilopascalsAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.TorrFullName }",
                Symbol = $"{ LibraryResources.TorrAbbrev }",
                CurrentUnitValue = 50,
                SiValue = 6666.12,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateSpecificVolumeUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.SpecificVolume));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()} * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()} * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.FeetFullName }{(3d).ToSuperScript()} * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.FeetAbbrev }{(3d).ToSuperScript()} * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 1000,
                SiValue = 62.4,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateTemperatureUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Temperature));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.KelvinFullName }",
                Symbol = $"{LibraryResources.KelvinAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.RankineFullName }",
                Symbol = $"{ LibraryResources.RankineAbbrev }",
                CurrentUnitValue = 671.67,
                SiValue = 373.15,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.CelsiusFullName }",
                Symbol = $"{ LibraryResources.CelsiusAbbrev }",
                CurrentUnitValue = 100,
                SiValue = 373.15,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.FahrenheitFullName }",
                Symbol = $"{ LibraryResources.FahrenheitAbbrev }",
                CurrentUnitValue = 212,
                SiValue = 373.15,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateTimeUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Time));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.SecondsFullName }",
                Symbol = $"{LibraryResources.SecondsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM"),
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MinutesFullName }",
                Symbol = $"{ LibraryResources.MinutesAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 60,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.HoursFullName }",
                Symbol = $"{ LibraryResources.HoursAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 3600,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MillisecondsFullName }",
                Symbol = $"{ LibraryResources.MillisecondsAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.DaysFullName }",
                Symbol = $"{ LibraryResources.DaysAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 24 * 60 * 60,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateVelocityUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Velocity));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName } * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev } * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName } * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev } * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 6.56168,
                SiValue = 2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateVolumeUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.Volume));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(3d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(3d).ToSuperScript()}",
                CurrentUnitValue = Math.Pow(3.28084, 3),
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.LitersFullName }",
                Symbol = $"{ LibraryResources.LitersAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.MillilitersFullName }",
                Symbol = $"{ LibraryResources.MillilitersAbbrev }",
                CurrentUnitValue = 1e6,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.GallonsFullName }",
                Symbol = $"{ LibraryResources.GallonsAbbrev }",
                CurrentUnitValue = 264.172,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateVolumeExpansivityUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.VolumeExpansivity));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.KelvinFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.KelvinAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.RankineFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.RankineAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 373.15,
                SiValue = 671.67,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        [Test]
        public void ValidateVolumetricFlowRateUnits()
        {
            // arrange 
            SetupMockRepositories();

            // act
            IResult<RepositoryStatusCode, EngineeringUnitCategory> result = SUT.GetById(nameof(LibraryResources.VolumetricFlowRate));
            IEnumerable<EngineeringUnit> units = result.ResultObject.Units;

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.NotNull(result.ResultObject.Units);

            new EngineeringUnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()} * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()} * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new EngineeringUnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(3d).ToSuperScript()} * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(3d).ToSuperScript()} * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 35.3147,
                SiValue = 1,
                UnitSystems =
                {
                   new EngineeringUnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }

        private void SetupMockRepositories(bool addBadData = false)
        {
            Data = new EngineeringMathSeedData();
            Owner system = Data.Owners["SYSTEM"];
            UnitSystem siUnits = Data.UnitSystems[nameof(LibraryResources.SIFullName)];
            UnitSystem uscsUnits = Data.UnitSystems[nameof(LibraryResources.USCSFullName)];

            UnitCategory corruptedCompositeEquation = new UnitCategory()
            {
                CompositeEquation = "Im bad",
                Name = "BadComposite",
                Owner = system,
            };
            UnitCategory corruptedUnits = new UnitCategory()
            {
                Name = "BadUnits",
                Owner = system,
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = "Bad unit",
                        Symbol = "yep",
                        ConvertFromSi = "bad equation",
                        ConvertToSi = "bad equation",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                }
            };

            if (addBadData)
            {
                Data.UnitCategories.Add(corruptedCompositeEquation.Name, corruptedCompositeEquation);
                Data.UnitCategories.Add(corruptedUnits.Name, corruptedUnits);
            }

            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<UnitCategory, bool>>()))
                .Callback<Func<UnitCategory, bool>>(CreateResult)
                .Returns(ResultList);

        }

        private void CreateResult(Func<UnitCategory, bool> whereCondition)
        {
            var result = Data.UnitCategories.Values.Where(whereCondition);
            if(result.Count() == 0)
            {
                ResultList.StatusCode = RepositoryStatusCode.objectNotFound;
                ResultList.ResultObject = null;
            }
            else
            {
                ResultList.StatusCode = RepositoryStatusCode.success;
                ResultList.ResultObject = result;
            }

        }
    }
}
