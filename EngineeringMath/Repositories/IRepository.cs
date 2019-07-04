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
    internal interface IRepository<T, S> : IReadonlyRepository<T>
    {
        RepositoryResult<IEnumerable<T>> Create(IEnumerable<S> blueprints);
        RepositoryResult<T> Create(S blueprint);

        RepositoryResult Update(IEnumerable<T> obj);
        RepositoryResult Update(T obj);

        RepositoryResult Delete(IEnumerable<T> obj);
        RepositoryResult Delete(T obj);
    }
}
