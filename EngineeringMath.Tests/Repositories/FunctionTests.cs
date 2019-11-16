using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using Microsoft.EntityFrameworkCore.Storage;
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
        private Mock<IReadonlyRepository<BuiltEquation>> EquationRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<BuiltParameter>> ParameterRepositoryMock { get; set; }
        private Mock<ILogger> LoggerMock { get; set; }
        private FunctionRepository FunctionRepository { get; set; }
        public IDbContextTransaction Transaction { get; private set; }

        [SetUp]
        public void ClassSetup()
        {
            EquationRepositoryMock = new Mock<IReadonlyRepository<BuiltEquation>>();
            ParameterRepositoryMock = new Mock<IReadonlyRepository<BuiltParameter>>();
            LoggerMock = new Mock<ILogger>();
            FunctionRepository = new FunctionRepository(
                    RepositoryTestSetup.Context,
                    ParameterRepositoryMock.Object,
                    EquationRepositoryMock.Object,
                    LoggerMock.Object);

        }



        [TestCase(1)]
        public void ShouldCreateCorrectFunction(int funId)
        {
            // arrange

            List<Function> func = new List<Function>();

            IEnumerable<BuiltEquation> equ = new List<BuiltEquation>();

            EquationRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<BuiltEquation, bool>>()))
                .ReturnsAsync(equ);


            IEnumerable<BuiltParameter> para = new List<BuiltParameter>();
            ParameterRepositoryMock
                .Setup(x => x.GetAllWhereAsync(It.IsAny<Func<BuiltParameter, bool>>()))
                .ReturnsAsync(para);
            
            // act
            var result = FunctionRepository.GetByIdAsync(funId).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("SYSTEM", result.OwnerName);
        }
    }
}
