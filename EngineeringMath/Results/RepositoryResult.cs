using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Results
{
    public class RepositoryResult<T> : RepositoryResult, IResult<RepositoryStatusCode, T>
    {
        public RepositoryResult(RepositoryStatusCode statusCode, RepositoryAction action, T resultObject)
            : base(statusCode, action)
        {
            ResultObject = resultObject;
        }

        public T ResultObject { get; }

    }

    public class RepositoryResult : IResult<RepositoryStatusCode>
    {
        public RepositoryResult(RepositoryStatusCode statusCode, RepositoryAction action)
        {


            DebugMessage = debugMessage ?? throw new ArgumentNullException(debugMessage);
            UIMessage = uIMessage ?? throw new ArgumentNullException(uIMessage);
            StatusCode = statusCode;
        }

        public RepositoryStatusCode StatusCode { get; }
        public string DebugMessage { get; }
        public string UIMessage { get; }
    }
}
