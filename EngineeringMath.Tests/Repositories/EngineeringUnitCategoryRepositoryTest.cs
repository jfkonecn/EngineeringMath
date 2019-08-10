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
        public void ShouldGetSimpleEngineeringUnit()
        {
            // arrange 
            SetupMockRepositories();

            // act
            var result = SUT.GetById(nameof(LibraryResources.Length));
            var meters = result.ResultObject?.Units
                .Where(x => x.Name == nameof(LibraryResources.MeterFullName))
                .SingleOrDefault();

            var feet = result.ResultObject?.Units
                .Where(x => x.Name == nameof(LibraryResources.FeetFullName))
                .SingleOrDefault();

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.AreEqual(nameof(LibraryResources.Length), result.ResultObject.Name);
            Assert.AreEqual(7, result.ResultObject.Units.Count());
            Assert.NotNull(meters);
            Assert.AreEqual(20.2, meters.ConvertFromSi.Evaluate(20.2));
            Assert.AreEqual(20.2, meters.ConvertToSi.Evaluate(20.2));
            Assert.AreEqual(meters.OwnerName, "SYSTEM");
            Assert.AreEqual(meters.Symbol, nameof(LibraryResources.MeterAbbrev));
            Assert.IsNotNull(meters
                .UnitSystems.Where(x => x == nameof(LibraryResources.SIFullName))
                .SingleOrDefault());
            Assert.NotNull(feet);
            Assert.That(feet.ConvertFromSi.Evaluate(20.2), Is.EqualTo(66.27297).Within(0.1).Percent);
            Assert.That(feet.ConvertToSi.Evaluate(20.2), Is.EqualTo(6.15696).Within(0.1).Percent);
            Assert.AreEqual(feet.OwnerName, "SYSTEM");
            Assert.AreEqual(feet.Symbol, nameof(LibraryResources.FeetAbbrev));
            Assert.IsNotNull(feet
                .UnitSystems.Where(x => x == nameof(LibraryResources.USCSFullName))
                .SingleOrDefault());
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
