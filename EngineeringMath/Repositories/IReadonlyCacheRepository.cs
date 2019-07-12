using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    interface IReadonlyCacheRepository<T> : IReadonlyRepository<T>, ICache
    {
    }
}
