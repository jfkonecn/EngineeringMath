using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Factories
{
    class UnitCategoryFactory : IUnitCategoryFactory
    {
        public UnitCategoryFactory(IReadonlyRepository<UnitCategory> unitCategoryRepository, IReadonlyRepository<Unit> unitRepository)
        {
            UnitCategoryRepository = unitCategoryRepository;
            UnitRepository = unitRepository;
        }

        public void ClearCache()
        {
            UnitCategoryCache.Clear();
        }

        public EngineeringUnitCategory GetUnitCategory(UnitCategory unitCategory)
        {
            var result = UnitCategoryCache.Where(x => x.Name == unitCategory.Name).SingleOrDefault();
            if (result != null)
            {
                return result;
            }
            return CreateUnitCategory(unitCategory);
        }

        private EngineeringUnitCategory CreateUnitCategory(UnitCategory unitCategory)
        {

        }

        public void RemoveFromCache(EngineeringUnitCategory unitCategory)
        {
            UnitCategoryCache.Remove(unitCategory);
        }

        private IReadonlyRepository<UnitCategory> UnitCategoryRepository { get; }
        private IReadonlyRepository<Unit> UnitRepository { get; }
        private HashSet<EngineeringUnitCategory> UnitCategoryCache { get; } = new HashSet<EngineeringUnitCategory>();
    }
}
