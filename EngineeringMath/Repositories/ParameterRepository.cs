using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Results;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected override IResult<RepositoryStatusCode, IEnumerable<Parameter>> BuildT(IEnumerable<ParameterDB> blueprints)
        {
            List<Parameter> parameters = new List<Parameter>();
            foreach (ParameterDB parameterDB in blueprints)
            {
                var parameterUnitCategory = UnitCategoryRepository.GetById(parameterDB.UnitCategory.Name);
                if (parameterUnitCategory.StatusCode != RepositoryStatusCode.success)
                {
                    return new RepositoryResult<IEnumerable<Parameter>>(parameterUnitCategory.StatusCode, null);
                }
                try
                {
                    parameters.Add(new Parameter()
                    {
                        EquationName = parameterDB.EquationName,
                        FunctionName = parameterDB.Function.Name,
                        Type = Type.GetType(parameterDB.ParameterType.Name),
                        UnitCategory = parameterUnitCategory.ResultObject,
                        ValueConditions = StringEquationFactory.CreateStringEquation(parameterDB.ValueConditions),
                        ValueLinks = parameterDB.ValueLinks.Select(x => x.ParameterName).ToList(),
                        FunctionLinks = GetFunctionLinks(parameterDB),
                    });
                }
                catch(Exception e)
                {
                    Logger.LogError(e.Message);
                    return new RepositoryResult<IEnumerable<Parameter>>(RepositoryStatusCode.internalError, null);
                }
            }
            return new RepositoryResult<IEnumerable<Parameter>>(RepositoryStatusCode.success, parameters);
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
            return obj.KeyName;
        }

        protected override string GetKey(ParameterDB obj)
        {
            return obj.KeyName;
        }
    }
}
