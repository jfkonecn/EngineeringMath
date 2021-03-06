﻿using EngineeringMath.EngineeringModel;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Validators
{
    public class ParameterValidator : ValidatorBase<BuiltParameter>
    {
        protected override (bool isValid, string message) PerformValidation(BuiltParameter obj)
        {
            bool isValid = false;
            string message = string.Empty;
            try
            {
                isValid = Convert.ToBoolean(obj.ValueConditions.Evaluate(obj.Value));
                if (!isValid) message = string.Format(LibraryResources.OutOfRange, obj.ParameterName);
            }
            catch(Exception e)
            {
                message = e.Message;
            }
            return (isValid, message);
        }
    }
}
