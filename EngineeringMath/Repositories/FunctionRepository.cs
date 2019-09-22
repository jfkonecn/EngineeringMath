using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class FunctionRepository : ReadonlyCacheRepositoryBase<string, Function, FunctionDB>
    {
        public FunctionRepository(
            IReadonlyRepository<Parameter> parameterRepository,
            IReadonlyRepository<Equation> equationRepository,
            IReadonlyRepository<FunctionDB> functionRepository, 
            ILogger logger) : base(functionRepository, logger)
        {
            ParameterRepository = parameterRepository;
            EquationRepository = equationRepository;
            FunctionDBRepository = functionRepository;
            Logger = logger;
        }

        public IReadonlyRepository<Parameter> ParameterRepository { get; }
        public IReadonlyRepository<Equation> EquationRepository { get; }
        public IReadonlyRepository<FunctionDB> FunctionDBRepository { get; }
        public ILogger Logger { get; }

        protected override async Task<IResult<RepositoryStatusCode, IEnumerable<Function>>> BuildTAsync(IEnumerable<FunctionDB> blueprints)
        {
            RepositoryResult<IEnumerable<Function>> result = null;
            List<Function> createdFunctions = new List<Function>();
            foreach (var function in blueprints)
            {
                var equations = await EquationRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                if(equations.StatusCode != RepositoryStatusCode.success)
                {
                    result = new RepositoryResult<IEnumerable<Function>>(equations.StatusCode, null);
                    break;
                }
                var parameters = await ParameterRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                if (parameters.StatusCode != RepositoryStatusCode.success)
                {
                    result = new RepositoryResult<IEnumerable<Function>>(equations.StatusCode, null);
                    break;
                }
                createdFunctions.Add(new Function()
                {
                    Name = function.Name,
                    OwnerName = function.Owner.Name,
                    Equations = equations.ResultObject,
                    Parameters = parameters.ResultObject,
                });
            }
            return result ?? new RepositoryResult<IEnumerable<Function>>(RepositoryStatusCode.success, createdFunctions);
        }

        protected override string GetKey(FunctionDB obj)
        {
            return obj.Name;
        }

        protected override string GetKey(Function obj)
        {
            return obj.Name;
        }
    }
}
