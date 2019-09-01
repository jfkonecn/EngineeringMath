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
    public class EngineeringFunctionTests
    {
        private Mock<IReadonlyRepository<Function>> FunctionRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<EngineeringEquation>> EquationRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<EngineeringParameter>> ParameterRepositoryMock { get; set; }
        private EngineeringFunctionRepository SUT { get; set; }
        private EngineeringMathSeedData Data { get; set; }

        [SetUp]
        public void ClassSetup()
        {
            FunctionRepositoryMock = new Mock<IReadonlyRepository<Function>>();
            EquationRepositoryMock = new Mock<IReadonlyRepository<EngineeringEquation>>();
            ParameterRepositoryMock = new Mock<IReadonlyRepository<EngineeringParameter>>();
            SUT = new EngineeringFunctionRepository(
                    ParameterRepositoryMock.Object,
                    EquationRepositoryMock.Object,
                    FunctionRepositoryMock.Object,
                    new ConsoleLogger());
        }

        private Action<Func<Function, bool>> CreateResult(MockResult<IEnumerable<Function>> resultList)
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
            Function expectedFunction = Data.Functions[funName];

            MockResult<IEnumerable<Function>> func = new MockResult<IEnumerable<Function>>();
            FunctionRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<Function, bool>>()))
                .Callback(CreateResult(func))
                .Returns(func);

            MockResult<IEnumerable<EngineeringEquation>> equ = new MockResult<IEnumerable<EngineeringEquation>>()
            {
                ResultObject = new List<EngineeringEquation>(),
                StatusCode = RepositoryStatusCode.success
            };
            ICollection<Equation> expectedEquations = expectedFunction.Equations;
            List<Equation> equationsFound = new List<Equation>();
            ;
            EquationRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<EngineeringEquation, bool>>()))
                .Returns(equ);


            MockResult<IEnumerable<EngineeringParameter>> para = new MockResult<IEnumerable<EngineeringParameter>>()
            {
                ResultObject = new List<EngineeringParameter>(),
                StatusCode = RepositoryStatusCode.success
            };
            ParameterRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<EngineeringParameter, bool>>()))
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
