using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Results;
using EngineeringMath.Tests.Mocks;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">System under test</typeparam>
    /// <typeparam name="S">Blueprint</typeparam>
    public abstract class BaseEngineeringRepositoryTest<T, S, U> where T : IReadonlyRepository<U> where S : class where U : class
    {
        private Mock<IReadonlyRepository<S>> SRepositoryMock { get; set; }
        protected T SUT { get; set; }
        private EngineeringMathSeedData Data { get; set; }
        private MockResult<IEnumerable<S>> ResultList { get; } = new MockResult<IEnumerable<S>>();

        [SetUp]
        public void ClassSetup()
        {
            Presetup();
            SRepositoryMock = BuildMockRepository();
            SUT = BuildSystemUnderTest();
        }

        protected abstract void Presetup();
        protected abstract T BuildSystemUnderTest();
        protected abstract Mock<IReadonlyRepository<S>> BuildMockRepository();
        protected abstract void AddBadData(EngineeringMathSeedData data);
        protected abstract Dictionary<string, S> GetAllBluePrints(EngineeringMathSeedData data);

        private void SetupMockRepositories(bool addBadData = false)
        {
            Data = new EngineeringMathSeedData();
            if (addBadData)
            {
                AddBadData(Data);

            }

            SRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<S, bool>>()))
                .Callback<Func<S, bool>>(CreateResult)
                .Returns(ResultList);

        }


        private void CreateResult(Func<S, bool> whereCondition)
        {
            var result = GetAllBluePrints(Data).Values.Where(whereCondition);
            if (result.Count() == 0)
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
            SRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<S, bool>>()))
                .Returns(new RepositoryResult<IEnumerable<S>>(RepositoryStatusCode.internalError, null));

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

            foreach (object starting in startingResult.ResultObject)
            {
                bool foundCachedVersion = false;
                foreach (object cached in cacheResult.ResultObject)
                {
                    if (ReferenceEquals(starting, cached))
                    {
                        foundCachedVersion = true;
                        break;
                    }
                }
                Assert.True(foundCachedVersion, $"{starting.ToString()} was not cached");
            }
        }
    }
}
