using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Validators
{
    public interface IValidator<T>
    {
        void Validate(T obj);
    }
}
