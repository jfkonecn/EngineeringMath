using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using Moq;
using NUnit.Framework;
using StringMath;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.EngineeringModel;
using Microsoft.Extensions.Logging;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class UnitCategoryRepositoryTest
    {
        private Mock<ILogger> LoggerMock { get; set; }
        private Mock<IReadonlyRepository<UnitCategoryDB>> UnitCategoryRepositoryMock { get; set; }
        private UnitCategoryRepository SUT { get; set; }

        private EngineeringMathSeedData Data { get; set; }

        [SetUp]
        public void ClassSetup()
        {
            UnitCategoryRepositoryMock = new Mock<IReadonlyRepository<UnitCategoryDB>>();
            LoggerMock = new Mock<ILogger>();
            SUT = new UnitCategoryRepository(
                UnitCategoryRepositoryMock.Object, 
                new StringEquationFactory(),
                LoggerMock.Object);
        }

        private void SetupMockRepositories(bool addBadData = false)
        {
            Data = new EngineeringMathSeedData();
            OwnerDB system = Data.Owners["SYSTEM"];
            UnitSystemDB siUnits = Data.UnitSystems[nameof(LibraryResources.SIFullName)];
            UnitSystemDB uscsUnits = Data.UnitSystems[nameof(LibraryResources.USCSFullName)];

            UnitCategoryDB corruptedCompositeEquation = new UnitCategoryDB()
            {
                CompositeEquation = "Im bad",
                Name = "BadComposite",
                Owner = system,
            };
            UnitCategoryDB corruptedUnits = new UnitCategoryDB()
            {
                Name = "BadUnits",
                Owner = system,
                Units = new List<UnitDB>
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = "Bad unit",
                        Symbol = "yep",
                        ConvertFromSi = "bad equation",
                        ConvertToSi = "bad equation",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>
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

            List<UnitCategoryDB> resultList = new List<UnitCategoryDB>();
            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<UnitCategoryDB, bool>>()))
                .ReturnsAsync((Func<UnitCategoryDB, bool> whereCondition) =>
                        Data.UnitCategories.Values.Where(whereCondition));

        }



        [Test]
        public void ShouldHandleCorruptedCompositeUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            // assert
            Assert.ThrowsAsync<ArgumentException>(() => SUT.GetByIdAsync("BadComposite"));
        }

        [Test]
        public void ShouldHandleCorruptedUnitsInUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            // assert
            var result = Assert.ThrowsAsync<ArgumentException>(() => SUT.GetByIdAsync("BadUnits"));
        }

        [Test]
        public void ShouldHandleRepositoryError()
        {
            // arrange
            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<UnitCategoryDB, bool>>()))
                .ThrowsAsync(new Exception());

            // act
            // assert
            var result = Assert.ThrowsAsync<Exception>(() => SUT.GetAllAsync());
        }

        [Test]
        public void ShouldHandleGetAllUnitCategories()
        {
            // arrange
            SetupMockRepositories();

            // act
            var result = SUT.GetAllAsync().Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Data.UnitCategories.Count(), result.Count());
        }


        [Test]
        public void ShouldHandleCacheUnitCategories()
        {
            // arrange
            SetupMockRepositories();

            // act
            var startingResult = SUT.GetAllAsync().Result;
            var cacheResult = SUT.GetAllAsync().Result;

            // assert

            foreach (UnitCategory starting in startingResult)
            {
                foreach (UnitCategory cached in cacheResult)
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Area)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MeterFullName }{(2d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.MeterAbbrev }{(2d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(2d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(2d).ToSuperScript()}",
                CurrentUnitValue = 21.5278,
                SiValue = 2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Density)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.KilogramsFullName } * { LibraryResources.MeterFullName }{(-3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.KilogramsAbbrev } * { LibraryResources.MeterAbbrev }{(-3d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.PoundsMassFullName } * { LibraryResources.FeetFullName }{(-3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.PoundsMassAbbrev } * { LibraryResources.FeetAbbrev }{(-3d).ToSuperScript()}",
                CurrentUnitValue = 62.4,
                SiValue = 1000,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Energy)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.JoulesFullName }",
                Symbol = $"{ LibraryResources.JoulesAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.BTUFullName }",
                Symbol = $"{ LibraryResources.BTUAbbrev }",
                CurrentUnitValue = 20,
                SiValue = 21101.1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.KilocaloriesFullName }",
                Symbol = $"{ LibraryResources.KilocaloriesAbbrev }",
                CurrentUnitValue = 5,
                SiValue = 20920,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.KilojoulesFullName }",
                Symbol = $"{ LibraryResources.KilojoulesAbbrev }",
                CurrentUnitValue = 1000,
                SiValue = 1e6,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.ThermsFullName }",
                Symbol = $"{ LibraryResources.ThermsAbbrev }",
                CurrentUnitValue = 2,
                SiValue = 2.11e+8,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Enthalpy)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.JoulesFullName } * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.JoulesAbbrev } * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.BTUFullName } * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.BTUAbbrev } * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 50,
                SiValue = 116.3e3,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Entropy)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.JoulesFullName } * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()} * { LibraryResources.KelvinFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.JoulesAbbrev } * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()} * { LibraryResources.KelvinAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.BTUFullName } * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()} * { LibraryResources.RankineFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.BTUAbbrev } * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()} * { LibraryResources.RankineAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 5,
                SiValue = 20934,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.IsothermalCompressibility)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.PascalsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.PascalsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsForcePerSqInFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.PoundsForcePerSqInAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 5e4,
                SiValue = 7.2518869,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Length)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = LibraryResources.MeterFullName,
                Symbol = LibraryResources.MeterAbbrev,
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.FeetFullName,
                Symbol = LibraryResources.FeetAbbrev,
                CurrentUnitValue = 20.2,
                SiValue = 6.15696,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.InchesFullName,
                Symbol = LibraryResources.InchesAbbrev,
                CurrentUnitValue = 196.85,
                SiValue = 5,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.MilesFullName,
                Symbol = LibraryResources.MilesAbbrev,
                CurrentUnitValue = 1.242742,
                SiValue = 2000,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.MillimetersFullName,
                Symbol = LibraryResources.MillimetersAbbrev,
                CurrentUnitValue = 2000,
                SiValue = 2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.CentimetersFullName,
                Symbol = LibraryResources.CentimetersAbbrev,
                CurrentUnitValue = 200,
                SiValue = 2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = LibraryResources.KilometersFullName,
                Symbol = LibraryResources.KilometersAbbrev,
                CurrentUnitValue = 2,
                SiValue = 2000,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Mass)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.KilogramsFullName }",
                Symbol = $"{LibraryResources.KilogramsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsMassFullName }",
                Symbol = $"{ LibraryResources.PoundsMassAbbrev }",
                CurrentUnitValue = 11.0231,
                SiValue = 5,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.GramsFullName }",
                Symbol = $"{ LibraryResources.GramsAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MilligramsFullName }",
                Symbol = $"{ LibraryResources.MilligramsAbbrev }",
                CurrentUnitValue = 1e6,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MicrogramsFullName }",
                Symbol = $"{ LibraryResources.MicrogramsAbbrev }",
                CurrentUnitValue = 1e9,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MetricTonsFullName }",
                Symbol = $"{ LibraryResources.MetricTonsAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.OuncesFullName }",
                Symbol = $"{ LibraryResources.OuncesAbbrev }",
                CurrentUnitValue = 35274,
                SiValue = 1e3,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.USTonsFullName }",
                Symbol = $"{ LibraryResources.USTonsAbbrev }",
                CurrentUnitValue = 1.10231252347,
                SiValue = 1000,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Power)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.WattsFullName }",
                Symbol = $"{LibraryResources.WattsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.HorsepowerFullName }",
                Symbol = $"{ LibraryResources.HorsepowerAbbrev }",
                CurrentUnitValue = 10,
                SiValue = 7457,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.KilowattFullName }",
                Symbol = $"{ LibraryResources.KilowattAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Pressure)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.PascalsFullName }",
                Symbol = $"{LibraryResources.PascalsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.PoundsForcePerSqInFullName }",
                Symbol = $"{ LibraryResources.PoundsForcePerSqInAbbrev }",
                CurrentUnitValue = 20,
                SiValue = 137895,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.BarFullName }",
                Symbol = $"{ LibraryResources.BarAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 100000,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.KilopascalsFullName }",
                Symbol = $"{ LibraryResources.KilopascalsAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 1e3,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.TorrFullName }",
                Symbol = $"{ LibraryResources.TorrAbbrev }",
                CurrentUnitValue = 50,
                SiValue = 6666.12,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.SpecificVolume)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()} * { LibraryResources.KilogramsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()} * { LibraryResources.KilogramsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.FeetFullName }{(3d).ToSuperScript()} * { LibraryResources.PoundsMassFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.FeetAbbrev }{(3d).ToSuperScript()} * { LibraryResources.PoundsMassAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 1000,
                SiValue = 62.4,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Temperature)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.KelvinFullName }",
                Symbol = $"{LibraryResources.KelvinAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.RankineFullName }",
                Symbol = $"{ LibraryResources.RankineAbbrev }",
                CurrentUnitValue = 671.67,
                SiValue = 373.15,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.CelsiusFullName }",
                Symbol = $"{ LibraryResources.CelsiusAbbrev }",
                CurrentUnitValue = 100,
                SiValue = 373.15,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.FahrenheitFullName }",
                Symbol = $"{ LibraryResources.FahrenheitAbbrev }",
                CurrentUnitValue = 212,
                SiValue = 373.15,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Time)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.SecondsFullName }",
                Symbol = $"{LibraryResources.SecondsAbbrev }",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM"),
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MinutesFullName }",
                Symbol = $"{ LibraryResources.MinutesAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 60,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.HoursFullName }",
                Symbol = $"{ LibraryResources.HoursAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 3600,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MillisecondsFullName }",
                Symbol = $"{ LibraryResources.MillisecondsAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.DaysFullName }",
                Symbol = $"{ LibraryResources.DaysAbbrev }",
                CurrentUnitValue = 1,
                SiValue = 24 * 60 * 60,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM"),
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Velocity)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName } * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev } * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName } * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev } * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 6.56168,
                SiValue = 2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.Volume)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(3d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(3d).ToSuperScript()}",
                CurrentUnitValue = Math.Pow(3.28084, 3),
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.LitersFullName }",
                Symbol = $"{ LibraryResources.LitersAbbrev }",
                CurrentUnitValue = 1e3,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.MillilitersFullName }",
                Symbol = $"{ LibraryResources.MillilitersAbbrev }",
                CurrentUnitValue = 1e6,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.MetricFullName, LibraryResources.MetricAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.GallonsFullName }",
                Symbol = $"{ LibraryResources.GallonsAbbrev }",
                CurrentUnitValue = 264.172,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.ImperialFullName, LibraryResources.ImperialAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.VolumeExpansivity)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.KelvinFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.KelvinAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.RankineFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.RankineAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 373.15,
                SiValue = 671.67,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
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
            UnitCategory result = SUT.GetByIdAsync(nameof(LibraryResources.VolumetricFlowRate)).Result;
            IEnumerable<Unit> units = result.Units;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Units);

            new UnitValidator()
            {
                FullName = $"{LibraryResources.MeterFullName }{(3d).ToSuperScript()} * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{LibraryResources.MeterAbbrev }{(3d).ToSuperScript()} * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 20.2,
                SiValue = 20.2,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.SIFullName, LibraryResources.SIAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);

            new UnitValidator()
            {
                FullName = $"{ LibraryResources.FeetFullName }{(3d).ToSuperScript()} * { LibraryResources.SecondsFullName }{(-1d).ToSuperScript()}",
                Symbol = $"{ LibraryResources.FeetAbbrev }{(3d).ToSuperScript()} * { LibraryResources.SecondsAbbrev }{(-1d).ToSuperScript()}",
                CurrentUnitValue = 35.3147,
                SiValue = 1,
                UnitSystems =
                {
                   new UnitSystem(LibraryResources.USCSFullName, LibraryResources.USCSAbbrev, "SYSTEM")
                },
                Owner = "SYSTEM"
            }.AssertUnitValid(units);
        }
    }
}
