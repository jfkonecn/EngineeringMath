using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class EngineeringFunctionRepository : IReadonlyRepository<>
    {
        RepositoryResult<IEnumerable<T>> GetByName(IEnumerable<string> names)
        {

        }
        RepositoryResult<T> GetByName(string name)
        {

        }
    }
}
