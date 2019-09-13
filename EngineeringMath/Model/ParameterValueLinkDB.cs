using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    /// <summary>
    /// Lock the value of this parameter to another parameter (one way)
    /// </summary>
    public class ParameterValueLinkDB
    {
        public int ParameterValueLinkId { get; set; }
        [Required]
        public string ParameterName { get; set; }
    }
}
