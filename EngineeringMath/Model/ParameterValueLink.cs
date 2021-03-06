﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    /// <summary>
    /// Lock the value of this parameter to another parameter (one way)
    /// </summary>
    public class ParameterValueLink
    {
        public int ParameterValueLinkId { get; set; }

        public int ParameterId { get; set; }
        [Required]
        public Parameter Parameter { get; set; }
    }
}
