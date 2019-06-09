using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringMath.Model
{
    public class ImportedClass
    {
        public int ImportedClassId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ImportedMethod> Methods { get; set; }
        [Required]
        public Owner Owner { get; set; }
    }
}
