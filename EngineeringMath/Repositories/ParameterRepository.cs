using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class ParameterRepository : ReadonlyCacheRepositoryBase<string, Parameter, ParameterDB>
    {
        public ParameterRepository(
            IReadonlyRepository<ParameterDB> repository, 
            IReadonlyRepository<UnitCategory> unitCategoryRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(repository, logger)
        {
            UnitCategoryRepository = unitCategoryRepository;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

        private IReadonlyRepository<UnitCategory> UnitCategoryRepository { get; }
        private IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }

        protected async override Task<IEnumerable<Parameter>> BuildTAsync(IEnumerable<ParameterDB> blueprints)
        {
            List<Parameter> parameters = new List<Parameter>();
            foreach (ParameterDB parameterDB in blueprints)
            {
                var parameterUnitCategory = await UnitCategoryRepository.GetByIdAsync(parameterDB.UnitCategory.Name);


                parameters.Add(new Parameter()
                {
                    ParameterName = parameterDB.ParameterName,
                    FunctionName = parameterDB.Function.Name,
                    Type = Type.GetType(parameterDB.ParameterType.Name),
                    UnitCategory = parameterUnitCategory,
                    ValueConditions = StringEquationFactory.CreateStringEquation(parameterDB.ValueConditions),
                    ValueLinks = parameterDB.ValueLinks.Select(x => x.ParameterName).ToList(),
                    FunctionLinks = GetFunctionLinks(parameterDB),
                });
            }
            return parameters;
        }

        private ICollection<FunctionOutputValueLink> GetFunctionLinks(ParameterDB parameterDB)
        {
            List<FunctionOutputValueLink> links = new List<FunctionOutputValueLink>();
            foreach (FunctionOutputValueLinkDB link in parameterDB.FunctionLinks)
            {
                links.Add(new FunctionOutputValueLink()
                {
                    LinkFunctionName = link.Function.Name,
                    LinkOutputName = link.OutputParameterName,
                    ParentFunctionName = parameterDB.Function.Name,
                });
            }
            return links;
        }


        protected override string GetKey(Parameter obj)
        {
            return obj.ParameterName;
        }

        protected override string GetKey(ParameterDB obj)
        {
            return obj.ParameterName;
        }
    }
}
