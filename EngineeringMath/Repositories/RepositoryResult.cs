using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class RepositoryResult<T> : RepositoryResult
    {
        public RepositoryResult(T resultObject, RepositoryStatusCode statusCode, string statusMessage) 
            : base(statusCode, statusMessage)
        {
            ResultObject = resultObject;
        }

        public T ResultObject { get; }

    }

    public class RepositoryResult
    {
        public RepositoryResult(RepositoryStatusCode statusCode, string statusMessage)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        public RepositoryStatusCode StatusCode { get; }
        public string StatusMessage { get; set; }
    }
}
