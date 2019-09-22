using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">object this repository handles</typeparam>
    public interface IReadonlyRepository<T>
    {
        Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetAllAsync();
        Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetByIdAsync(IEnumerable<object> keys);
        Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetAllWhereAsync(Func<T, bool> whereCondition);
        Task<IResult<RepositoryStatusCode, T>> GetByIdAsync(object key);
    }
}
