using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">object this repository handles</typeparam>
    public interface IReadonlyRepository<T>
    {
        RepositoryResult<IEnumerable<T>> GetAll();
        RepositoryResult<IEnumerable<T>> GetById(IEnumerable<object> keys);
        RepositoryResult<T> GetById(object key);
    }
}
