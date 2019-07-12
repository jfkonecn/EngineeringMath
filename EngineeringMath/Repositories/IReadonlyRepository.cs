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
    public interface IReadonlyRepository<T>
    {
        IResult<RepositoryStatusCode, IEnumerable<T>> GetAll();
        IResult<RepositoryStatusCode, IEnumerable<T>> GetById(IEnumerable<object> keys);
        IResult<RepositoryStatusCode, IEnumerable<T>> GetAllWhere(Func<T, bool> whereCondition);
        IResult<RepositoryStatusCode, T> GetById(object key);
    }
}
