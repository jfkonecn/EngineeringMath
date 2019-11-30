using EngineeringMath.EngineeringModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EngineeringMath.App.Models
{
    public class FunctionDetail
    {
        public FunctionDetail(BuiltFunction builtFunction)
        {
            BuiltFunction = builtFunction;
        }

        private BuiltFunction BuiltFunction { get; }


        public string Name
        {
            get { return BuiltFunction.Name; }
        }

    }
}
