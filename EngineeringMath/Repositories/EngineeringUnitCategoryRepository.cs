using EngineeringMath.EngineeringModel;
using EngineeringMath.Factories;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Repositories
{
    class EngineeringUnitCategoryRepository : IReadonlyCacheRepository<EngineeringUnitCategory>
    {
        public EngineeringUnitCategoryRepository(
            IReadonlyRepository<UnitCategory> unitCategoryRepository,
            IReadonlyRepository<Unit> unitRepository,
            IResultFactory resultFactory,
            ILogger logger)
        {
            UnitCategoryRepository = unitCategoryRepository;
            UnitRepository = unitRepository;
            ResultFactory = resultFactory;
            Logger = logger;
        }

        public void ClearCache()
        {
            UnitCategoryCache.Clear();
        }

  
        public void RemoveFromCache(EngineeringUnitCategory unitCategory)
        {
            UnitCategoryCache.Remove(unitCategory);
        }

        
        public IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetAll()
        {
            //var allCache = UnitCategoryCache.ToList();
            throw new NotImplementedException();
        }

        public IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetAllWhere(Func<EngineeringUnitCategory, bool> whereCondition)
        {
            //Logger.Debug("GetUnitCategory", $"Looking for {unitCategoryName}");
            //var unitCategory = UnitCategoryCache.Where(x => x.Name == unitCategoryName).SingleOrDefault();
            //if (unitCategory != null)
            //{
            //    return ResultFactory.BuilderResult(RepositoryStatusCode.success, RepositoryAction.Get, unitCategory);
            //}
            //var result = CreateUnitCategory(unitCategoryName);
            //UnitCategoryCache.Add(result.ResultObject);
            //return result;
            throw new NotImplementedException();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetById(IEnumerable<object> keys)
        {
            throw new NotImplementedException();
        }

        public IResult<RepositoryStatusCode, EngineeringUnitCategory> GetById(object key)
        {
            throw new NotImplementedException();
        }

        private IResult<RepositoryStatusCode, EngineeringUnitCategory> CreateUnitCategory(string unitCategoryName)
        {
            throw new NotImplementedException();
        }

        private IReadonlyRepository<UnitCategory> UnitCategoryRepository { get; }
        private IReadonlyRepository<Unit> UnitRepository { get; }
        public IResultFactory ResultFactory { get; }
        private ILogger Logger { get; }
        private HashSet<EngineeringUnitCategory> UnitCategoryCache { get; } = new HashSet<EngineeringUnitCategory>();
    }
}
