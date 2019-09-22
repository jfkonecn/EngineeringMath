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
    /// <typeparam name="S">blueprint to create new T</typeparam>
    public interface IRepository<T, S> : IReadonlyRepository<T>
    {
        Task<IResult<RepositoryStatusCode, IEnumerable<T>>> CreateAsync(IEnumerable<S> blueprints);
        Task<IResult<RepositoryStatusCode, T>> CreateAsync(S blueprint);

        Task<IResult<RepositoryStatusCode>> UpdateAsync(IEnumerable<T> obj);
        Task<IResult<RepositoryStatusCode>> UpdateAsync(T obj);

        Task<IResult<RepositoryStatusCode>> DeleteAsync(IEnumerable<T> obj);
        Task<IResult<RepositoryStatusCode>> DeleteAsync(T obj);
    }
}
