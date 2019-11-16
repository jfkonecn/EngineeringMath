using EngineeringMath.EngineeringModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public interface IReadonlyCacheRepository<T> : IReadonlyRepository<T>, ICache<T> where T : IBuiltModel
    {
    }
}
