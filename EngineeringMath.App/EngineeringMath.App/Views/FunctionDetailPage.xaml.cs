using EngineeringMath.App.Models;
using EngineeringMath.App.ViewModels;
using EngineeringMath.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EngineeringMath.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FunctionDetailPage : ContentPage
    {
        public FunctionDetailViewModel VM { get; }
        public FunctionDetailPage(IFunctionController functionController)
        {
            VM = new FunctionDetailViewModel(functionController);
            BindingContext = VM;
            InitializeComponent();
        }
    }
}