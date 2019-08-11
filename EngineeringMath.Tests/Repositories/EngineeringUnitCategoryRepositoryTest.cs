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
