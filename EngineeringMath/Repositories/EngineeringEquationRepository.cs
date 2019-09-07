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
    public class EngineeringEquationRepository : ReadonlyCacheRepositoryBase<string, EngineeringEquation, Equation>
    {
        public EngineeringEquationRepository(
            IReadonlyRepository<Equation> equationRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(equationRepository, logger)
        {
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

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
                        Formula = StringEquationFactory.CreateStringEquation(equation.Formula),
                        FunctionName = equation.Function.Name,
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

        protected override string GetKey(EngineeringEquation obj)
        {
            return $"{obj.FunctionName}.{obj.OutputName}";
        }

        protected override string GetKey(Equation obj)
        {
            return $"{obj.Function.Name}.{obj.OutputName}";
        }
    }
}
