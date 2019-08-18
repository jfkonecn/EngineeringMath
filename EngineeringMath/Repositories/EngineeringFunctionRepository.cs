using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class EngineeringFunctionRepository : ReadonlyCacheRepositoryBase<string, EngineeringFunction, Function>
    {
        public EngineeringFunctionRepository(
            IReadonlyCacheRepository<EngineeringParameter> parameterRepository,
            IReadonlyCacheRepository<EngineeringEquation> equationRepository,
            IReadonlyRepository<Function> functionRepository, 
            ILogger logger) : base(functionRepository, logger)
        {
            ParameterRepository = parameterRepository;
            EquationRepository = equationRepository;
            FunctionRepository = functionRepository;
            Logger = logger;
        }

        public IReadonlyCacheRepository<EngineeringParameter> ParameterRepository { get; }
        public IReadonlyCacheRepository<EngineeringEquation> EquationRepository { get; }
        public IReadonlyRepository<Function> FunctionRepository { get; }
        public ILogger Logger { get; }

        protected override IResult<RepositoryStatusCode, IEnumerable<EngineeringFunction>> BuildT(IEnumerable<Function> blueprints)
        {
            RepositoryResult<IEnumerable<EngineeringFunction>> result = null;
            List<EngineeringFunction> createdFunctions = new List<EngineeringFunction>();
            foreach (var function in blueprints)
            {
                var equations = EquationRepository.GetAllWhere((x) => x.FunctionName == function.Name);
                if(equations.StatusCode != RepositoryStatusCode.success)
                {
                    result = new RepositoryResult<IEnumerable<EngineeringFunction>>(equations.StatusCode, null);
                    break;
                }
                var parameters = ParameterRepository.GetAllWhere((x) => x.FunctionName == function.Name);
                if (parameters.StatusCode != RepositoryStatusCode.success)
                {
                    result = new RepositoryResult<IEnumerable<EngineeringFunction>>(equations.StatusCode, null);
                    break;
                }
                createdFunctions.Add(new EngineeringFunction()
                {
                    Name = function.Name,
                    OwnerName = function.Owner.Name,
                    Equations = equations.ResultObject,
                    Parameters = parameters.ResultObject,
                });
            }
            return result ?? new RepositoryResult<IEnumerable<EngineeringFunction>>(RepositoryStatusCode.success, createdFunctions);
        }

        protected override string GetKey(Function obj)
        {
            return obj.Name;
        }

        protected override string GetKey(EngineeringFunction obj)
        {
            return obj.Name;
        }
    }
}
