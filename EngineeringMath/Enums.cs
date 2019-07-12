using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath
{
    public enum RepositoryStatusCode
    {
        success = 200,
        objectNotFound = 400,
        internalError = 500,
    }

    public enum RepositoryAction
    {
        Get = 100,
        Create = 200,
        Update = 300,
        Delete = 400
    }
}
