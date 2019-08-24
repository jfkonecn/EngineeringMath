using EngineeringMath.Model;
using EngineeringMath.Repositories;
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
    public abstract class BaseEngineeringRepositoryTest<T, S> where T : class where S : class
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


    }
}
