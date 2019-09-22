using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
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

        protected override async Task<IEnumerable<Function>> BuildTAsync(IEnumerable<FunctionDB> blueprints)
        {
            List<Function> createdFunctions = new List<Function>();
            foreach (var function in blueprints)
            {
                var equationsTask = EquationRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                var parametersTask = ParameterRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                await Task.WhenAll(equationsTask, parametersTask);

                createdFunctions.Add(new Function()
                {
                    Name = function.Name,
                    OwnerName = function.Owner.Name,
                    Equations = equationsTask.Result,
                    Parameters = parametersTask.Result,
                });
            }
            return createdFunctions;
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
