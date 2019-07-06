using EngineeringMath.EngineeringModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Builders
{
    interface IEngineeringFunctionBuilder
    {
        void AddEquation(EngineeringEquation equation);
        void AddFunction(EngineeringParameter parameter);
        EngineeringFunction Create();
    }
}
