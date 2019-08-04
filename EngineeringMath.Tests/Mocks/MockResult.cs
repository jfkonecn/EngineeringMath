using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Tests.Mocks
{
    public class MockResult<T> : MockResult, IResult<RepositoryStatusCode, T>
    {
        public T ResultObject { get; set; }
    }

    public class MockResult : IResult<RepositoryStatusCode>
    {
        public RepositoryStatusCode StatusCode { get; set; }
    }
}
