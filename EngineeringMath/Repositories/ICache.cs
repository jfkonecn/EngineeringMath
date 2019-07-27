using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public interface ICache<T>
    {
        void ClearCache();
        void RemoveFromCache(T obj);
    }
}
