using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.App.Models
{
    public class HomeMenuFunction
    {
        public HomeMenuFunction()
        {

        }
        public HomeMenuFunction(Function function)
        {
            Name = function.Name.TryToFindStringInLibraryResources();
        }

        public string Name { get; set; }
    }
}
