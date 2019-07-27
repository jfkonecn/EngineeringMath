using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Status enum</typeparam>
    public interface IResult<T> where T : Enum
    {
        T StatusCode { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Status enum</typeparam>
    /// <typeparam name="S">Return object</typeparam>
    public interface IResult<T, S> : IResult<T> where T : Enum
    {
        S ResultObject { get; }
    }
}
