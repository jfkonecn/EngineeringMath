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
            Id = function.FunctionId;
            Name = function.Name.TryToFindStringInLibraryResources();
            Source = ImageSource.FromResource("EngineeringMath.Assets.NoImage.png", Assembly.GetAssembly(typeof(EngineeringMathContext)));
        }

        public int Id { get; }
        public string Name { get; }

        public ImageSource Source { get; }
    }
}
