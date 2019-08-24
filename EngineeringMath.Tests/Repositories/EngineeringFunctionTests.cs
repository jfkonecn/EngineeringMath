using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class EngineeringFunctionTests : BaseEngineeringRepositoryTest<EngineeringFunctionRepository, Function>
    {
        private Mock<IReadonlyRepository<Function>> FunctionRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<EngineeringEquation>> EquationRepositoryMock { get; set; }
        private Mock<IReadonlyRepository<EngineeringParameter>> ParameterRepositoryMock { get; set; }


        protected override void AddBadData(EngineeringMathSeedData data)
        {
            throw new NotImplementedException();
        }

        protected override void Presetup()
        {
            EquationRepositoryMock = new Mock<IReadonlyRepository<EngineeringEquation>>();
            ParameterRepositoryMock = new Mock<IReadonlyRepository<EngineeringParameter>>();
        }

        protected override Mock<IReadonlyRepository<Function>> BuildMockRepository()
        {
            return FunctionRepositoryMock = new Mock<IReadonlyRepository<Function>>();
        }

        protected override EngineeringFunctionRepository BuildSystemUnderTest()
        {
            return new EngineeringFunctionRepository(
                ParameterRepositoryMock.Object,
                EquationRepositoryMock.Object,
                FunctionRepositoryMock.Object,
                new ConsoleLogger());
        }

        protected override Dictionary<string, Function> GetAllBluePrints(EngineeringMathSeedData data)
        {
            throw new NotImplementedException();
        }
    }
}
