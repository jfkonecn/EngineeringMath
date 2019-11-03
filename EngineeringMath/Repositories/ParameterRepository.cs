using EngineeringMath.EngineeringModel;
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
    public class ParameterRepository : ReadonlyCacheRepositoryBase<string, BuiltParameter, Parameter>
    {
        public ParameterRepository(
            EngineeringMathContext dbContext,
            IReadonlyRepository<Parameter> repository,
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
            List<BuiltParameter> parameters = new List<BuiltParameter>();
            var blueprints = await DbContext.Parameters
                .Include(x => x.ParameterType)
                .Include(x => x.UnitCategory)
                .Include(x => x.ValueLinks)
                .Include(x => x.FunctionLinks)
                .Include(x => x.Owner)
                .ToListAsync();
            foreach (Parameter parameterDB in blueprints.Where(whereCondition))
            {
                var parameterUnitCategory = await UnitCategoryRepository.GetByIdAsync(parameterDB.UnitCategory.Name);


                parameters.Add(new BuiltParameter()
                {
                    ParameterName = parameterDB.ParameterName,
                    FunctionName = parameterDB.Function.Name,
                    Type = Type.GetType(parameterDB.ParameterType.Name),
                    UnitCategory = parameterUnitCategory,
                    ValueConditions = StringEquationFactory.CreateStringEquation(parameterDB.ValueConditions),
                    ValueLinks = parameterDB.ValueLinks.Select(x => x.Parameter.ParameterName).ToList(),
                    FunctionLinks = GetFunctionLinks(parameterDB),
                });
            }
            return parameters;
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


        protected override string GetKey(BuiltParameter obj)
        {
            return obj.ParameterName;
        }

        protected override string GetKey(Parameter obj)
        {
            return obj.ParameterName;
        }
    }
}
