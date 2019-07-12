using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Factories
{
    interface IResultFactory
    {
        IResult<RepositoryStatusCode, T> BuilderResult<T>(RepositoryStatusCode statusCode, RepositoryAction action, T resultObject);
        IResult<RepositoryStatusCode> BuilderResult(RepositoryStatusCode statusCode, RepositoryAction action);
    }
}
