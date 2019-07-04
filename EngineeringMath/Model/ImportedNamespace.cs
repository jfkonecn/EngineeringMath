﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ImportedNamespace
    {
        public int ImportedNamespaceId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ImportedClass> Classes { get; set; }
    }
}