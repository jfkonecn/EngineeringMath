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
        public ObservableCollection<FunctionDB> Functions { get; } = new ObservableCollection<FunctionDB>()
        {
            new FunctionDB()
            {
                Name = "Unit Converter",
                FunctionCategories = new List<FunctionCategoryDB>
                {
                    new FunctionCategoryDB()
                    {
                        Name = "General"
                    }
                }
            },
            new FunctionDB()
            {
                Name = "Orifice Plate",
                FunctionCategories = new List<FunctionCategoryDB>
                {
                    new FunctionCategoryDB()
                    {
                        Name = "General"
                    },
                    new FunctionCategoryDB()
                    {
                        Name = "Fluids"
                    }
                }
            },
                        new FunctionDB()
            {
                Name = "Unit Converter",
                FunctionCategories = new List<FunctionCategoryDB>
                {
                    new FunctionCategoryDB()
                    {
                        Name = "General"
                    }
                }
            },
            new FunctionDB()
            {
                Name = "Orifice Plate",
                FunctionCategories = new List<FunctionCategoryDB>
                {
                    new FunctionCategoryDB()
                    {
                        Name = "General"
                    },
                    new FunctionCategoryDB()
                    {
                        Name = "Fluids"
                    }
                }
            },
        };
    }
}
