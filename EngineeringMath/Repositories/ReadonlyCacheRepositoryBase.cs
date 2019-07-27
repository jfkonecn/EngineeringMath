using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringMath.Factories;
using EngineeringMath.Loggers;
using EngineeringMath.Results;

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
        public ReadonlyCacheRepositoryBase(IReadonlyRepository<S> repository, IResultFactory resultFactory, ILogger logger)
        {
            Repository = repository;
            ResultFactory = resultFactory;
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


        public IResult<RepositoryStatusCode, IEnumerable<T>> GetAll()
        {

            var existingNames = new HashSet<Key>();
            foreach (T unitCat in Cache)
            {
                existingNames.Add(GetKey(unitCat));
            }
            bool whereCondition(S x) => !existingNames.Contains(GetKey(x));
            return GetFromRepositoryWhere(whereCondition);

        }




        public IResult<RepositoryStatusCode, IEnumerable<T>> GetAllWhere(Func<T, bool> whereCondition)
        {
            IEnumerable<T> unitCategory = Cache.Where(whereCondition);
            if (unitCategory.Count() > 0)
            {
                return ResultFactory.BuilderResult(RepositoryStatusCode.success, RepositoryAction.Get, unitCategory);
            }
            return ResultFactory.BuilderResult<IEnumerable<T>>(RepositoryStatusCode.objectNotFound, RepositoryAction.Get, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public IResult<RepositoryStatusCode, IEnumerable<T>> GetById(IEnumerable<object> keys)
        {
            if (!(keys is IEnumerable<Key> realKeys))
            {
                return ResultFactory
                    .BuilderResult<IEnumerable<T>>(
                    RepositoryStatusCode.objectNotFound,
                    RepositoryAction.Get,
                    null);
            }
            IResult<RepositoryStatusCode, IEnumerable<T>> result =
                GetFromRepositoryWhere(x => realKeys.Contains(GetKey(x)));
            if (result.StatusCode != RepositoryStatusCode.success ||
                result.ResultObject.Count() != result.ResultObject.Count())
            {
                var statusCode = result.StatusCode != RepositoryStatusCode.success ? result.StatusCode : RepositoryStatusCode.objectNotFound;
                return ResultFactory.BuilderResult<IEnumerable<T>>(statusCode, RepositoryAction.Get, null);
            }
            return ResultFactory.BuilderResult(RepositoryStatusCode.success, RepositoryAction.Get, result.ResultObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public IResult<RepositoryStatusCode, T> GetById(object key)
        {
            IResult<RepositoryStatusCode, IEnumerable<T>> result = GetById(new List<object>() { key });
            T resultObject = result.ResultObject == default(IEnumerable<T>) ? result.ResultObject.FirstOrDefault() : default;
            return ResultFactory.BuilderResult(result.StatusCode, RepositoryAction.Get, resultObject);
        }


        protected IResult<RepositoryStatusCode, IEnumerable<T>> GetFromRepositoryWhere(Func<S, bool> whereCondition)
        {
            IResult<RepositoryStatusCode, IEnumerable<S>> queryResult = Repository.GetAllWhere(whereCondition);

            if (queryResult.StatusCode != RepositoryStatusCode.success)
            {
                return ResultFactory
                    .BuilderResult<IEnumerable<T>>(queryResult.StatusCode, RepositoryAction.Get, null);
            }

            IResult<RepositoryStatusCode, IEnumerable<T>> buildResult = BuildT(queryResult.ResultObject);

            if (buildResult.StatusCode != RepositoryStatusCode.success)
            {
                return ResultFactory.BuilderResult<IEnumerable<T>>(buildResult.StatusCode, RepositoryAction.Get, null);
            }

            foreach (T item in buildResult.ResultObject)
            {
                Cache.Add(item);
            }
            return ResultFactory.BuilderResult<IEnumerable<T>>(RepositoryStatusCode.success, RepositoryAction.Get, Cache);
        }


        protected abstract Key GetKey(T obj);
        protected abstract Key GetKey(S obj);
        protected abstract IResult<RepositoryStatusCode, IEnumerable<T>> BuildT(IEnumerable<S> blueprints);

        private HashSet<T> Cache { get; } = new HashSet<T>();
        public IReadonlyRepository<S> Repository { get; }
        private IResultFactory ResultFactory { get; }
        private ILogger Logger { get; }

    }
}
