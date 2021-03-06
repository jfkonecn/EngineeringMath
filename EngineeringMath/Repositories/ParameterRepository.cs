﻿using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class ParameterRepository : ReadonlyCacheRepositoryBase<BuiltParameter, Parameter>
    {
        public ParameterRepository(
            EngineeringMathContext dbContext,
            IReadonlyRepository<BuiltUnitCategory> unitCategoryRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(logger)
        {
            DbContext = dbContext;
            UnitCategoryRepository = unitCategoryRepository;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

        private EngineeringMathContext DbContext { get; }
        private IReadonlyRepository<BuiltUnitCategory> UnitCategoryRepository { get; }
        private IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }

        protected async override Task<IEnumerable<BuiltParameter>> BuildTAsync(Func<Parameter, bool> whereCondition)
        {
            List<BuiltParameter> builtParameters = new List<BuiltParameter>();
            var blueprints = DbContext.Parameters
                .Include(x => x.ParameterType)
                .Include(x => x.UnitCategory)
                .Include(x => x.ValueLinks)
                .Include(x => x.FunctionLinks)
                .Include(x => x.Owner)
                .Where(whereCondition)
                .ToList();
            foreach (Parameter parameter in blueprints.Where(whereCondition))
            {
                var parameterUnitCategory = parameter.UnitCategory == null ? null : await UnitCategoryRepository.GetByIdAsync(parameter.UnitCategory.UnitCategoryId);
                var valueConditions = parameter.ValueConditions == null ? null : StringEquationFactory.CreateStringEquation(parameter.ValueConditions);

                builtParameters.Add(new BuiltParameter()
                {
                    Id = parameter.ParameterId,
                    ParameterName = parameter.ParameterName,
                    FunctionId = parameter.FunctionId,
                    FunctionName = parameter.Function.Name,
                    Type = Type.GetType(parameter.ParameterType.Name),
                    UnitCategory = parameterUnitCategory,
                    ValueConditions = valueConditions,
                    ValueLinks = parameter.ValueLinks.Select(x => x.Parameter.ParameterName).ToList(),
                    FunctionLinks = GetFunctionLinks(parameter),
                });
            }
            return builtParameters;
        }

        private ICollection<BuiltFunctionOutputValueLink> GetFunctionLinks(Parameter parameterDB)
        {
            List<BuiltFunctionOutputValueLink> links = new List<BuiltFunctionOutputValueLink>();
            foreach (FunctionOutputValueLink link in parameterDB.FunctionLinks)
            {
                links.Add(new BuiltFunctionOutputValueLink()
                {
                    LinkFunctionName = link.Function.Name,
                    LinkOutputName = link.OutputParameterName,
                    ParentFunctionName = parameterDB.Function.Name,
                });
            }
            return links;
        }

        protected override int GetKey(Parameter obj)
        {
            return obj.ParameterId;
        }
    }
}
