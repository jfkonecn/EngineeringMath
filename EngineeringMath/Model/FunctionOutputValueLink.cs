using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class FunctionOutputValueLink
    {
        public int FunctionOutputValueLinkId { get; set; }
        public Function Function { get; set; }
        [Required]
        public string OutputParameterName { get; set; }
    }
}
