﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ImportedMethod
    {
        public int ImportedMethodId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
