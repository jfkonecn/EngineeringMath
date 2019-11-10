using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

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
            var temp = Assembly.GetAssembly(typeof(EngineeringMathContext)).GetManifestResourceNames();
            Source = ImageSource.FromResource("EngineeringMath.Assets.NoImage.png", Assembly.GetAssembly(typeof(EngineeringMathContext)));
        }

        public string Name { get; set; }

        public ImageSource Source { get; set; }
    }
}
