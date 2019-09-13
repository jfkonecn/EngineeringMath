using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class ParameterRepository : ReadonlyCacheRepositoryBase<string, Parameter, ParameterDB>
    {
        public ParameterRepository(
            IReadonlyRepository<ParameterDB> repository, 
            ILogger logger) : base(repository, logger)
        {
        }

        protected override IResult<RepositoryStatusCode, IEnumerable<Parameter>> BuildT(IEnumerable<ParameterDB> blueprints)
        {
            throw new NotImplementedException();
        }

        protected override string GetKey(Parameter obj)
        {
            return $"{obj.FunctionName}.{obj.Name}";
        }

        protected override string GetKey(ParameterDB obj)
        {
            return $"{obj.Function.Name}.{obj.Name}";
        }
    }
}
