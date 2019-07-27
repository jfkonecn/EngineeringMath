using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Results
{
    public class RepositoryResult<T> : RepositoryResult, IResult<RepositoryStatusCode, T>
    {
        public RepositoryResult(RepositoryStatusCode statusCode, T resultObject) : base(statusCode)
        {
            ResultObject = resultObject;
        }

        public T ResultObject { get; }

    }

    public class RepositoryResult : IResult<RepositoryStatusCode>
    {
        public RepositoryResult(RepositoryStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public RepositoryStatusCode StatusCode { get; }
    }
}
