using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Key">The key the cache and the repository objects share</typeparam>
    /// <typeparam name="T">Cache Object</typeparam>
    /// <typeparam name="S">Repository Object</typeparam>
    public abstract class ReadonlyCacheRepositoryBase<Key, T, S> : IReadonlyCacheRepository<T>
    {
        public ReadonlyCacheRepositoryBase(ILogger logger)
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
            IEnumerable<Key> keys = Cache.Where(whereCondition).Select(cat => GetKey(cat));

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
        public async Task<IEnumerable<T>> GetByIdAsync(IEnumerable<object> keys)
        {
            List<Key> realKeys = new List<Key>();
            foreach (var key in keys)
            {
                if (key is Key realKey)
                {
                    realKeys.Add(realKey);
                }
            }
            return await GetFromRepositoryWhereAsync(x => realKeys.Contains(GetKey(x)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(object key)
        {
            IEnumerable<T> result = await GetByIdAsync(new List<object>() { key });
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
            IEnumerable<Key> keys = buildResult.Select(GetKey);
            return Cache.Where(obj => keys.Contains(GetKey(obj)));
        }


        protected abstract Key GetKey(T obj);
        protected abstract Key GetKey(S obj);
        protected abstract Task<IEnumerable<T>> BuildTAsync(Func<S, bool> whereCondition);
        private HashSet<T> Cache { get; } = new HashSet<T>();
        private ILogger Logger { get; }

    }
}
