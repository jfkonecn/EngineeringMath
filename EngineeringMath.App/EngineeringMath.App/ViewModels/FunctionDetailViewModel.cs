using EngineeringMath.App.Models;
using EngineeringMath.Controllers;
using EngineeringMath.EngineeringModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EngineeringMath.App.ViewModels
{
    public class FunctionDetailViewModel
    {
        private IFunctionController FunctionController { get; }
        public FunctionDetail FunctionDetail { get; set; }

        public FunctionDetailViewModel(IFunctionController functionController)
        {
            FunctionController = functionController;
            FunctionDetail = new FunctionDetail(functionController.Function);
            foreach (BuiltParameter parameter in functionController.Function.Parameters)
            {
                Parameters.Add(new ParameterDetail(parameter));
            }
        }

        public FunctionDetailViewModel() : this(App.ServiceProvider.GetRequiredService<IFunctionController>())
        {
        }

        public ObservableCollection<ParameterDetail> Parameters { get; } = new ObservableCollection<ParameterDetail>();

    }
}
