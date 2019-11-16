using EngineeringMath.EngineeringModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">object this repository handles</typeparam>
    /// <typeparam name="S">blueprint to create new T</typeparam>
    public interface ICacheRepository<T, S> : IRepository<T, S>, ICache<T> where T : IBuiltModel
    {

    }
}
