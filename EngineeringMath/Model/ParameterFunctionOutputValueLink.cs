using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ParameterFunctionOutputValueLink
    {
        public int ParameterFunctionOutputValueLinkId { get; set; }
        public int FunctionOutputValueLinkId { get; set; }
        [Required]
        public FunctionOutputValueLink FunctionOutputValueLink { get; set; }

        public int ParameterId { get; set; }
        [Required]
        public Parameter Parameter { get; set; }
    }
}
