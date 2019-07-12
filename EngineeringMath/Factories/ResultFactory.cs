using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Results;

namespace EngineeringMath.Factories
{
    class ResultFactory : IResultFactory
    {
        public IResult<RepositoryStatusCode, T> BuilderResult<T>(RepositoryStatusCode statusCode, RepositoryAction action, T resultObject)
        {
            return new RepositoryResult<T>(statusCode, action, resultObject);
        }

        public IResult<RepositoryStatusCode> BuilderResult(RepositoryStatusCode statusCode, RepositoryAction action)
        {
            return new RepositoryResult(statusCode, action);
        }
    }
}
