using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionOutputValueLinkDB
    {
        public int FunctionOutputValueLinkId { get; set; }
        [Required]
        public FunctionDB Function { get; set; }
        [Required]
        public string OutputParameterName { get; set; }
    }
}
