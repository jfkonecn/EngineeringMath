using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class FunctionTests
    {
        private Mock<IReadonlyRepository<FunctionDB>> FunctionRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<Equation>> EquationRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<Parameter>> ParameterRepositoryMock { get; set; }
        private Mock<ILogger> LoggerMock { get; set; }
        private FunctionRepository SUT { get; set; }
        private EngineeringMathSeedData Data { get; set; }

        [SetUp]
        public void ClassSetup()
        {
            FunctionRepositoryMock = new Mock<IReadonlyRepository<FunctionDB>>();
            EquationRepositoryMock = new Mock<IReadonlyRepository<Equation>>();
            ParameterRepositoryMock = new Mock<IReadonlyRepository<Parameter>>();
            LoggerMock = new Mock<ILogger>();
            SUT = new FunctionRepository(
                    ParameterRepositoryMock.Object,
                    EquationRepositoryMock.Object,
                    FunctionRepositoryMock.Object,
                    LoggerMock.Object);
        }

        private Action<Func<FunctionDB, bool>> CreateResult(List<FunctionDB> resultList)
        {
            return (whereCondition) =>
            {
                resultList.Clear();
                resultList.AddRange(Data.Functions.Values.Where(whereCondition));
            };
        }

        [TestCase(nameof(LibraryResources.OrificePlate))]
        public void ShouldCreateCorrectFunction(string funName)
        {
            // arrange
            Data = new EngineeringMathSeedData();
            FunctionDB expectedFunction = Data.Functions[funName];

            List<FunctionDB> func = new List<FunctionDB>();
            FunctionRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<FunctionDB, bool>>()))
                .ReturnsAsync((Func<FunctionDB, bool> whereCondition) =>
                        Data.Functions.Values.Where(whereCondition));

            IEnumerable<Equation> equ = new List<Equation>();

            ICollection<EquationDB> expectedEquations = expectedFunction.Equations;
            List<EquationDB> equationsFound = new List<EquationDB>();
            ;
            EquationRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<Equation, bool>>()))
                .ReturnsAsync(equ);


            IEnumerable<Parameter> para = new List<Parameter>();
            ParameterRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<Parameter, bool>>()))
                .ReturnsAsync(para);
            
            // act
            var result = SUT.GetByIdAsync(funName).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedFunction.Name, result.Name);
            Assert.AreEqual(expectedFunction.Owner.Name, result.OwnerName);
        }
    }
}
