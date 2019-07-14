using EngineeringMath.EngineeringModel;
using EngineeringMath.Factories;
using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Resources;
using EngineeringMath.Results;
using StringMath;
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
            IStringEquationFactory stringEquationFactory,
            ILogger logger)
        {
            UnitCategoryRepository = unitCategoryRepository;
            UnitRepository = unitRepository;
            ResultFactory = resultFactory;
            StringEquationFactory = stringEquationFactory;
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
            List<EngineeringUnitCategory> allCache = UnitCategoryCache.ToList();
            var existingNames = new HashSet<string>();
            allCache.ForEach(x => existingNames.Add(x.Name));
            bool whereCondition(UnitCategory x) => !existingNames.Contains(x.Name);
            return GetFromRepositoryWhere(whereCondition);

        }



        public IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetAllWhere(Func<EngineeringUnitCategory, bool> whereCondition)
        {
            IEnumerable<EngineeringUnitCategory> unitCategory = UnitCategoryCache.Where(whereCondition);
            if (unitCategory.Count() > 0)
            {
                return ResultFactory.BuilderResult(RepositoryStatusCode.success, RepositoryAction.Get, unitCategory);
            }
            return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                RepositoryStatusCode.objectNotFound, RepositoryAction.Get, null);
        }

        private IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetFromRepositoryWhere(Func<UnitCategory, bool> whereCondition)
        {
            IResult<RepositoryStatusCode, IEnumerable<UnitCategory>> queryResult =
                UnitCategoryRepository
                .GetAllWhere(whereCondition);

            if (queryResult.StatusCode != RepositoryStatusCode.success)
            {
                return ResultFactory
                    .BuilderResult<IEnumerable<EngineeringUnitCategory>>(queryResult.StatusCode, RepositoryAction.Get, null);
            }

            IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> buildResult =
                BuildEngineeringUnitCategory(queryResult.ResultObject);

            if (buildResult.StatusCode != RepositoryStatusCode.success)
            {
                return ResultFactory
                    .BuilderResult<IEnumerable<EngineeringUnitCategory>>(buildResult.StatusCode, RepositoryAction.Get, null);
            }

            foreach (EngineeringUnitCategory item in buildResult.ResultObject)
            {
                UnitCategoryCache.Add(item);
            }
            return ResultFactory
                    .BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                        RepositoryStatusCode.success,
                        RepositoryAction.Get,
                        UnitCategoryCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> GetById(IEnumerable<object> keys)
        {
            if (!(keys is IEnumerable<string> realKeys))
            {
                return ResultFactory
                    .BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                    RepositoryStatusCode.objectNotFound,
                    RepositoryAction.Get,
                    null);
            }
            IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> result = 
                GetFromRepositoryWhere(x => realKeys.Contains(x.Name));
            if(result.StatusCode != RepositoryStatusCode.success || 
                result.ResultObject.Count() != result.ResultObject.Count())
            {
                var statusCode = result.StatusCode != RepositoryStatusCode.success ? result.StatusCode : RepositoryStatusCode.objectNotFound;
                return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(statusCode, RepositoryAction.Get, null);
            }
            return ResultFactory.BuilderResult(RepositoryStatusCode.success, RepositoryAction.Get, result.ResultObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">unitCategoryName</param>
        /// <returns></returns>
        public IResult<RepositoryStatusCode, EngineeringUnitCategory> GetById(object key)
        {
            IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> result = GetById(new List<object>(){ key });
            EngineeringUnitCategory resultObject = result.ResultObject == null ? result.ResultObject.FirstOrDefault() : null;
            return ResultFactory.BuilderResult(result.StatusCode, RepositoryAction.Get, resultObject);
        }

        private IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> BuildEngineeringUnitCategory(IEnumerable<UnitCategory> blueprints)
        {
            Stack<EngineeringUnitCategory> unitCategories = new Stack<EngineeringUnitCategory>();
            foreach (UnitCategory blueprint in blueprints)
            {
                IResult<RepositoryStatusCode, IEnumerable<EngineeringUnit>> compositeResult = CreateCompositeUnits(blueprint);
                if (compositeResult.StatusCode != RepositoryStatusCode.success)
                {
                    return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                            compositeResult.StatusCode, RepositoryAction.Get, null);
                }


            }
            return ResultFactory.BuilderResult<IEnumerable<EngineeringUnitCategory>>(
                RepositoryStatusCode.success, RepositoryAction.Get, unitCategories);
        }

        private IResult<RepositoryStatusCode, IEnumerable<EngineeringUnit>> CreateCompositeUnits(UnitCategory blueprint)
        {
            Stack<EngineeringUnit> compositeUnits = new Stack<EngineeringUnit>();
            if (!string.IsNullOrEmpty(blueprint.CompositeEquation))
            {
                IStringEquation stringEquation = StringEquationFactory.CreateStringEquation(blueprint.CompositeEquation);
                IResult<RepositoryStatusCode, IEnumerable<EngineeringUnitCategory>> compositeResult = GetById(stringEquation.EquationArguments);
                if (compositeResult.StatusCode != RepositoryStatusCode.success)
                {
                    return ResultFactory
                        .BuilderResult<IEnumerable<EngineeringUnit>>(compositeResult.StatusCode, RepositoryAction.Get, null);
                }
                Dictionary<string, IEnumerable<EngineeringUnit>> dic = compositeResult
                    .ResultObject
                    .ToDictionary(cat => cat.Name, cat => cat.Units);

                var unitSystems = new Dictionary<string, Dictionary<string, EngineeringUnit>>();

                foreach (KeyValuePair<string, IEnumerable<EngineeringUnit>> item in dic)
                {
                    foreach (var unit in item.Value)
                    {
                        foreach (var system in unit.UnitSystems)
                        {
                            if( system == nameof(LibraryResources.ImperialFullName) ||
                                system == nameof(LibraryResources.MetricFullName))
                            {
                                // SI or USC good systems to use, but we can't inner join all the units
                                // since that would cost too much memory
                                continue;
                            }

                            if(!unitSystems.ContainsKey(system))
                            {
                                unitSystems.Add(system, new Dictionary<string, EngineeringUnit>());
                            }

                            if (unitSystems[system].ContainsKey(item.Key))
                            {
                                Logger.Error("CreateCompositeUnits", $"More than one unit using {system} as a system!");
                                return ResultFactory.BuilderResult<IEnumerable<EngineeringUnit>>(
                                    RepositoryStatusCode.internalError, RepositoryAction.Get, null);

                            }
                            unitSystems[system].Add(item.Key, unit);
                        }
                    }
                }

            }
            return ResultFactory.BuilderResult<IEnumerable<EngineeringUnit>>(
                RepositoryStatusCode.success, RepositoryAction.Get, compositeUnits);

        }



        private IReadonlyRepository<UnitCategory> UnitCategoryRepository { get; }
        private IReadonlyRepository<Unit> UnitRepository { get; }
        public IResultFactory ResultFactory { get; }
        public IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }
        private HashSet<EngineeringUnitCategory> UnitCategoryCache { get; } = new HashSet<EngineeringUnitCategory>();
    }
}
