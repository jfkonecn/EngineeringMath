using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Results;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class EngineeringEquationRepository : ReadonlyCacheRepositoryBase<int, EngineeringEquation, Equation>
    {
        public EngineeringEquationRepository(
            IReadonlyRepository<Equation> equationRepository,
            IReadonlyCacheRepository<Function> functionRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(equationRepository, logger)
        {
            FunctionRepository = functionRepository;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

        public IReadonlyCacheRepository<Function> FunctionRepository { get; }
        public IStringEquationFactory StringEquationFactory { get; }
        public ILogger Logger { get; }

        protected override IResult<RepositoryStatusCode, IEnumerable<EngineeringEquation>> BuildT(IEnumerable<Equation> blueprints)
        {
            List<EngineeringEquation> equations = new List<EngineeringEquation>();
            RepositoryStatusCode statusCode = RepositoryStatusCode.success;
            foreach (Equation equation in blueprints)
            {
                try
                {
                    equations.Add(
                    new EngineeringEquation()
                    {
                        EquationId = equation.EquationId,
                        Formula = StringEquationFactory.CreateStringEquation(equation.Formula),
                        FunctionName = FunctionRepository
                            .GetAllWhere(x => x.Equations.Contains(equation))
                            .ResultObject
                            .Single()
                            .Name,
                        OutputName = equation.OutputName,
                        OwnerName = equation.Owner.Name,
                    });
                }
                catch (Exception e)
                {
                    Logger.Error($"{nameof(EngineeringEquationRepository)}", $"ID-\"{equation.EquationId}\" Formula-\"{equation.Formula}\" has an error with the message ${e.Message}");
                    statusCode = RepositoryStatusCode.internalError;
                    break;
                }
            }
            return new RepositoryResult<IEnumerable<EngineeringEquation>>(statusCode, equations);
        }

        protected override int GetKey(EngineeringEquation obj)
        {
            return obj.EquationId;
        }

        protected override int GetKey(Equation obj)
        {
            return obj.EquationId;
        }
    }
}
