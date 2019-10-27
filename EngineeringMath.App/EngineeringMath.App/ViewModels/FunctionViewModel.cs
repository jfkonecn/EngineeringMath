using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace EngineeringMath.App.ViewModels
{
    public class FunctionViewModel : BaseViewModel
    {
        public ObservableCollection<Function> Functions { get; } = new ObservableCollection<Function>()
        {
            //new Function()
            //{
            //    Name = "Unit Converter",
            //    FunctionCategories = new List<FunctionCategory>
            //    {
            //        new FunctionCategory()
            //        {
            //            Name = "General"
            //        }
            //    }
            //},
            //new Function()
            //{
            //    Name = "Orifice Plate",
            //    FunctionCategories = new List<FunctionCategory>
            //    {
            //        new FunctionCategory()
            //        {
            //            Name = "General"
            //        },
            //        new FunctionCategory()
            //        {
            //            Name = "Fluids"
            //        }
            //    }
            //},
            //            new Function()
            //{
            //    Name = "Unit Converter",
            //    FunctionCategories = new List<FunctionCategory>
            //    {
            //        new FunctionCategory()
            //        {
            //            Name = "General"
            //        }
            //    }
            //},
            //new Function()
            //{
            //    Name = "Orifice Plate",
            //    FunctionCategories = new List<FunctionCategory>
            //    {
            //        new FunctionCategory()
            //        {
            //            Name = "General"
            //        },
            //        new FunctionCategory()
            //        {
            //            Name = "Fluids"
            //        }
            //    }
            //},
        };
    }
}
