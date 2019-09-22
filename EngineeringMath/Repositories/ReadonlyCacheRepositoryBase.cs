using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Results;
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
        public ReadonlyCacheRepositoryBase(IReadonlyRepository<S> repository, ILogger logger)
        {
            Repository = repository;
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


        public async Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetAllAsync()
        {
            return await GetAllWhereAsync(x => true);
        }




        public async Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetAllWhereAsync(Func<T, bool> whereCondition)
        {
            IEnumerable<Key> keys = Cache.Where(whereCondition).Select(cat => GetKey(cat));

            bool whereNotInCache(S x) => keys
                .Where(key => Equals(key, GetKey(x)))
                .Count() == 0;
            var repositoryResult = await GetFromRepositoryWhereAsync(whereNotInCache);

            if(repositoryResult.StatusCode == RepositoryStatusCode.internalError)
            {
                return repositoryResult;
            }

            var cacheResult = Cache.Where(whereCondition);
            var status = cacheResult.Count() == 0 ? RepositoryStatusCode.objectNotFound : RepositoryStatusCode.success;
            return new RepositoryResult<IEnumerable<T>>(status, cacheResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public async Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetByIdAsync(IEnumerable<object> keys)
        {
            List<Key> realKeys = new List<Key>();
            foreach (var key in keys)
            {
                if (!(key is Key realKey))
                {
                    return new RepositoryResult<IEnumerable<T>>(RepositoryStatusCode.objectNotFound, null);
                }
                realKeys.Add(realKey);
            }

            IResult<RepositoryStatusCode, IEnumerable<T>> result =
                await GetFromRepositoryWhereAsync(x => realKeys.Contains(GetKey(x)));
            if (result.StatusCode != RepositoryStatusCode.success ||
                result.ResultObject.Count() != realKeys.Count())
            {
                var statusCode = result.StatusCode != RepositoryStatusCode.success ? result.StatusCode : RepositoryStatusCode.objectNotFound;
                return new RepositoryResult<IEnumerable<T>>(statusCode, null);
            }
            return new RepositoryResult<IEnumerable<T>>(RepositoryStatusCode.success, result.ResultObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public async Task<IResult<RepositoryStatusCode, T>> GetByIdAsync(object key)
        {
            IResult<RepositoryStatusCode, IEnumerable<T>> result = await GetByIdAsync(new List<object>() { key });
            T resultObject = result.ResultObject != default(IEnumerable<T>) ? result.ResultObject.FirstOrDefault() : default;
            return new RepositoryResult<T>(result.StatusCode, resultObject);
        }


        protected async Task<IResult<RepositoryStatusCode, IEnumerable<T>>> GetFromRepositoryWhereAsync(Func<S, bool> whereCondition)
        {
            IResult<RepositoryStatusCode, IEnumerable<S>> queryResult = await Repository.GetAllWhereAsync(whereCondition);

            if (queryResult.StatusCode != RepositoryStatusCode.success)
            {
                return new RepositoryResult<IEnumerable<T>>(queryResult.StatusCode, null);
            }

            IResult<RepositoryStatusCode, IEnumerable<T>> buildResult = await BuildTAsync(queryResult.ResultObject);

            if (buildResult.StatusCode != RepositoryStatusCode.success)
            {
                return new RepositoryResult<IEnumerable<T>>(buildResult.StatusCode, null);
            }

            foreach (T item in buildResult.ResultObject)
            {
                Cache.Add(item);
            }
            IEnumerable<Key> keys = buildResult.ResultObject.Select(GetKey);
            return new RepositoryResult<IEnumerable<T>>(RepositoryStatusCode.success, Cache.Where(obj => keys.Contains(GetKey(obj))));
        }


        protected abstract Key GetKey(T obj);
        protected abstract Key GetKey(S obj);
        protected abstract Task<IResult<RepositoryStatusCode, IEnumerable<T>>> BuildTAsync(IEnumerable<S> blueprints);

        private HashSet<T> Cache { get; } = new HashSet<T>();
        private IReadonlyRepository<S> Repository { get; }
        private ILogger Logger { get; }

    }
}
