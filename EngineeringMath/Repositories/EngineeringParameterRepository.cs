using EngineeringMath.EngineeringModel;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class EngineeringParameterRepository : ReadonlyCacheRepositoryBase<string, EngineeringParameter, Parameter>
    {
        public EngineeringParameterRepository(
            IReadonlyRepository<Parameter> repository, 
            ILogger logger) : base(repository, logger)
        {
        }

        protected override IResult<RepositoryStatusCode, IEnumerable<EngineeringParameter>> BuildT(IEnumerable<Parameter> blueprints)
        {
            throw new NotImplementedException();
        }

        protected override string GetKey(EngineeringParameter obj)
        {
            return $"{obj.FunctionName}:{obj.Name}";
        }

        protected override string GetKey(Parameter obj)
        {
            return $"{obj.Function.Name}:{obj.Name}";
        }
    }
}
