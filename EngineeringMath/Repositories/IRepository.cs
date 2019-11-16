using EngineeringMath.EngineeringModel;
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
    public interface IRepository<T, S> : IReadonlyRepository<T> where T : IBuiltModel
    {
        Task<IEnumerable<T>> CreateAsync(IEnumerable<S> blueprints);
        Task<T> CreateAsync(S blueprint);

        Task UpdateAsync(IEnumerable<T> obj);
        Task UpdateAsync(T obj);

        Task DeleteAsync(IEnumerable<T> obj);
        Task DeleteAsync(T obj);
    }
}
