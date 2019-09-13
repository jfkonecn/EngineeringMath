using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using EngineeringMath.Tests.Mocks;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class FunctionTests
    {
        private Mock<IReadonlyRepository<FunctionDB>> FunctionRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<Equation>> EquationRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<Parameter>> ParameterRepositoryMock { get; set; }
        private FunctionRepository SUT { get; set; }
        private EngineeringMathSeedData Data { get; set; }

        [SetUp]
        public void ClassSetup()
        {
            FunctionRepositoryMock = new Mock<IReadonlyRepository<FunctionDB>>();
            EquationRepositoryMock = new Mock<IReadonlyRepository<Equation>>();
            ParameterRepositoryMock = new Mock<IReadonlyRepository<Parameter>>();
            SUT = new FunctionRepository(
                    ParameterRepositoryMock.Object,
                    EquationRepositoryMock.Object,
                    FunctionRepositoryMock.Object,
                    new ConsoleLogger());
        }

        private Action<Func<FunctionDB, bool>> CreateResult(MockResult<IEnumerable<FunctionDB>> resultList)
        {
            return (whereCondition) =>
            {
                var result = Data.Functions.Values.Where(whereCondition);
                if (result.Count() == 0)
                {
                    resultList.StatusCode = RepositoryStatusCode.objectNotFound;
                    resultList.ResultObject = null;
                }
                else
                {
                    resultList.StatusCode = RepositoryStatusCode.success;
                    resultList.ResultObject = result;
                }
            };
        }

        [TestCase(nameof(LibraryResources.OrificePlate))]
        public void ShouldCreateCorrectFunction(string funName)
        {
            // arrange
            Data = new EngineeringMathSeedData();
            FunctionDB expectedFunction = Data.Functions[funName];

            MockResult<IEnumerable<FunctionDB>> func = new MockResult<IEnumerable<FunctionDB>>();
            FunctionRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<FunctionDB, bool>>()))
                .Callback(CreateResult(func))
                .Returns(func);

            MockResult<IEnumerable<Equation>> equ = new MockResult<IEnumerable<Equation>>()
            {
                ResultObject = new List<Equation>(),
                StatusCode = RepositoryStatusCode.success
            };
            ICollection<EquationDB> expectedEquations = expectedFunction.Equations;
            List<EquationDB> equationsFound = new List<EquationDB>();
            ;
            EquationRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<Equation, bool>>()))
                .Returns(equ);


            MockResult<IEnumerable<Parameter>> para = new MockResult<IEnumerable<Parameter>>()
            {
                ResultObject = new List<Parameter>(),
                StatusCode = RepositoryStatusCode.success
            };
            ParameterRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<Parameter, bool>>()))
                .Returns(para);
            
            // act
            var result = SUT.GetById(funName);

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.IsNotNull(result.ResultObject);
            Assert.AreEqual(expectedFunction.Name, result.ResultObject.Name);
            Assert.AreEqual(expectedFunction.Owner.Name, result.ResultObject.OwnerName);
        }
    }
}
