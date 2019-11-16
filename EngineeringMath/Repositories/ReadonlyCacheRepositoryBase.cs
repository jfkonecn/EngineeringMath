using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.EngineeringModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Cache Object</typeparam>
    /// <typeparam name="S">Repository Object</typeparam>
    public abstract class ReadonlyCacheRepositoryBase<T, S> : IReadonlyCacheRepository<T> where T : IBuiltModel
    {
        protected ReadonlyCacheRepositoryBase(ILogger logger)
        {
            Logger = logger;
        }


        public void ClearCache()
        {
            Cache.Clear();
        }


        public void RemoveFromCache(T obj)
        {
            Cache.Remove(obj);
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetAllWhereAsync(x => true);
        }

        public async Task<IEnumerable<T>> GetAllWhereAsync(Func<T, bool> whereCondition)
        {
            IEnumerable<int> keys = Cache.Where(whereCondition).Select(cat => cat.Id);

            bool whereNotInCache(S x) => keys
                .Where(key => Equals(key, GetKey(x)))
                .Count() == 0;
            var repositoryResult = await GetFromRepositoryWhereAsync(whereNotInCache);
            var cacheResult = Cache.Where(whereCondition);
            return cacheResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetByIdAsync(IEnumerable<int> keys)
        {
            HashSet<int> keySet = keys.ToHashSet();
            return await GetFromRepositoryWhereAsync(x => keySet.Contains(GetKey(x)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int key)
        {
            IEnumerable<T> result = await GetByIdAsync(new List<int>() { key });
            T resultObject = result != default(IEnumerable<T>) ? result.FirstOrDefault() : default;
            return resultObject;
        }


        protected async Task<IEnumerable<T>> GetFromRepositoryWhereAsync(Func<S, bool> whereCondition)
        {
            IEnumerable<T> buildResult = await BuildTAsync(whereCondition);
            foreach (T item in buildResult)
            {
                Cache.Add(item);
            }
            IEnumerable<int> keys = buildResult.Select(x => x.Id);
            return Cache.Where(obj => keys.Contains(obj.Id));
        }


        protected abstract int GetKey(S obj);
        protected abstract Task<IEnumerable<T>> BuildTAsync(Func<S, bool> whereCondition);
        private HashSet<T> Cache { get; } = new HashSet<T>();
        private ILogger Logger { get; }

    }
}
