using EngineeringMath.Model;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class FunctionRepository : IReadonlyRepository<Function>
    {
        public IResult<RepositoryStatusCode, IEnumerable<Function>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IResult<RepositoryStatusCode, IEnumerable<Function>> GetAllWhere(Func<Function, bool> whereCondition)
        {
            throw new NotImplementedException();
        }

        public IResult<RepositoryStatusCode, IEnumerable<Function>> GetById(IEnumerable<object> keys)
        {
            throw new NotImplementedException();
        }

        public IResult<RepositoryStatusCode, Function> GetById(object key)
        {
            throw new NotImplementedException();
        }
    }
}
