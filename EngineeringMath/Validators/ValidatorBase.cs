using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Validators
{
    public abstract class ValidatorBase<T> : IValidator<T> where T : class
    {
        public void Validate(T obj)
        {
            (bool isValid, string message) = PerformValidation(obj);
            if (!isValid)
            {
                throw new ValidationException(message);
            }
        }

        protected abstract (bool isValid, string message) PerformValidation(T obj);
    }
}
