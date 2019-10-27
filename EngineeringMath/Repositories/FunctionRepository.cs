using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class FunctionRepository : ReadonlyCacheRepositoryBase<string, BuiltFunction, Function>
    {
        public FunctionRepository(
            EngineeringMathContext dbContext,
            IReadonlyRepository<BuiltParameter> parameterRepository,
            IReadonlyRepository<BuiltEquation> equationRepository,
            ILogger logger) : base(logger)
        {
            DbContext = dbContext;
            ParameterRepository = parameterRepository;
            EquationRepository = equationRepository;
        }

        private EngineeringMathContext DbContext { get; }
        private IReadonlyRepository<BuiltParameter> ParameterRepository { get; }
        private IReadonlyRepository<BuiltEquation> EquationRepository { get; }

        protected override async Task<IEnumerable<BuiltFunction>> BuildTAsync(Func<Function, bool> whereCondition)
        {
            List<BuiltFunction> createdFunctions = new List<BuiltFunction>();
            var blueprints = await DbContext.Functions
                .Include(x => x.Owner)
                .Include(x => x.Equations)
                .Include(x => x.Parameters)
                .ToListAsync();
            foreach (var function in blueprints.Where(whereCondition))
            {
                var equationsTask = EquationRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                var parametersTask = ParameterRepository.GetAllWhereAsync((x) => x.FunctionName == function.Name);
                await Task.WhenAll(equationsTask, parametersTask);

                createdFunctions.Add(new BuiltFunction()
                {
                    Name = function.Name,
                    OwnerName = function.Owner.Name,
                    Equations = equationsTask.Result,
                    Parameters = parametersTask.Result,
                });
            }
            return createdFunctions;
        }

        protected override string GetKey(Function obj)
        {
            return obj.Name;
        }

        protected override string GetKey(BuiltFunction obj)
        {
            return obj.Name;
        }
    }
}
