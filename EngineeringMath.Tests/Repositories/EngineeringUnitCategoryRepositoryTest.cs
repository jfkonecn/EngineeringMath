using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Results;
using Moq;
using NUnit.Framework;
using StringMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class EngineeringUnitCategoryRepositoryTest
    {
        private Mock<IReadonlyRepository<Unit>> UnitRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<UnitCategory>> UnitCategoryRepositoryMock { get; set; }
        private EngineeringUnitCategoryRepository SUT { get; set; }

        [SetUp]
        public void ClassSetup()
        {
            UnitRepositoryMock = new Mock<IReadonlyRepository<Unit>>();
            UnitCategoryRepositoryMock = new Mock<IReadonlyRepository<UnitCategory>>();
            SUT = new EngineeringUnitCategoryRepository(
                UnitCategoryRepositoryMock.Object, 
                UnitRepositoryMock.Object, 
                new StringEquationFactory(), 
                new ConsoleLogger());
        }

        [Test]
        public void ShouldHandleRepositoryError()
        {
            // arrange
            UnitRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<Unit, bool>>()))
                .Returns(new RepositoryResult<IEnumerable<Unit>>(RepositoryStatusCode.internalError, null));
            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<UnitCategory, bool>>()))
                .Returns(new RepositoryResult<IEnumerable<UnitCategory>>(RepositoryStatusCode.internalError, null));

            // act
            var result = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.AreEqual(null, result.ResultObject);
        }


    }
}
