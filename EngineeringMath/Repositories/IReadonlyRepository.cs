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
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByIdAsync(IEnumerable<object> keys);
        Task<IEnumerable<T>> GetAllWhereAsync(Func<T, bool> whereCondition);
        Task<T> GetByIdAsync(object key);
    }
}
