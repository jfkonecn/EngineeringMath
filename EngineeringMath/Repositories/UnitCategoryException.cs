using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repositories
{
    public class UnitCategoryException : Exception
    {
        public UnitCategoryException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}
