using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Factories
{
    interface IUnitCategoryFactory
    {
        EngineeringUnitCategory GetUnitCategory(UnitCategory unitCategory);
        /// <summary>
        /// Clears entire cashe
        /// </summary>
        void ClearCache();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        void RemoveFromCache(EngineeringUnitCategory unitCategory);

    }
}
