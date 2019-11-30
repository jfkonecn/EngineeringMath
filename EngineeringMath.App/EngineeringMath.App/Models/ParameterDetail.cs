using EngineeringMath.EngineeringModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.App.Models
{
    public class ParameterDetail
    {
        public ParameterDetail(BuiltParameter builtParameter)
        {
            BuiltParameter = builtParameter;
        }

        private BuiltParameter BuiltParameter { get; }


        public string Name
        {
            get
            {
                return BuiltParameter.ParameterName;
            }
        }

        public double Value 
        {
            get
            {
                return BuiltParameter.Value;
            }
            set
            {
                BuiltParameter.Value = value;
            } 
        }
    }
}
