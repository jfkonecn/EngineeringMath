using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">object this repository handles</typeparam>
    /// <typeparam name="S">blueprint to create new T</typeparam>
    public interface IRepository<T, S> : IReadonlyRepository<T>
    {
        IResult<RepositoryStatusCode, IEnumerable<T>> Create(IEnumerable<S> blueprints);
        IResult<RepositoryStatusCode, T> Create(S blueprint);

        IResult<RepositoryStatusCode> Update(IEnumerable<T> obj);
        IResult<RepositoryStatusCode> Update(T obj);

        IResult<RepositoryStatusCode> Delete(IEnumerable<T> obj);
        IResult<RepositoryStatusCode> Delete(T obj);
    }
}
