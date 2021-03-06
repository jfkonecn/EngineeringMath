﻿using EngineeringMath.EngineeringModel;
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
    public class FunctionRepository : ReadonlyCacheRepositoryBase<BuiltFunction, Function>
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
            var blueprints = DbContext.Functions
                .Include(x => x.Owner)
                .Include(x => x.Equations)
                .Include(x => x.Parameters)
                .Where(whereCondition)
                .ToList();
            foreach (var function in blueprints)
            {
                var equationsTask = EquationRepository.GetAllWhereAsync((x) => x.FunctionId == function.FunctionId);
                var parametersTask = ParameterRepository.GetAllWhereAsync((x) => x.FunctionId == function.FunctionId);
                await Task.WhenAll(equationsTask, parametersTask);

                createdFunctions.Add(new BuiltFunction()
                {
                    Id = function.FunctionId,
                    Name = function.Name,
                    OwnerName = function.Owner.Name,
                    Equations = equationsTask.Result,
                    Parameters = parametersTask.Result,
                });
            }
            return createdFunctions;
        }

        protected override int GetKey(Function obj)
        {
            return obj.FunctionId;
        }

    }
}
