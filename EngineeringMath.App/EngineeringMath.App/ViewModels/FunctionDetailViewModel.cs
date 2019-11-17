using EngineeringMath.App.Models;
using EngineeringMath.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EngineeringMath.App.ViewModels
{
    public class FunctionDetailViewModel
    {
        private IFunctionController FunctionController { get; }

        public FunctionDetailViewModel(IFunctionController functionController)
        {
            FunctionController = functionController;
        }

        public FunctionDetailViewModel() : this(App.ServiceProvider.GetRequiredService<IFunctionController>())
        {
        }
    }
}
