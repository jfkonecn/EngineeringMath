using EngineeringMath.App.Models;
using EngineeringMath.App.Views;
using EngineeringMath.Controllers;
using EngineeringMath.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EngineeringMath.App.ViewModels
{
    public class FunctionViewModel : BaseViewModel
    {
        public FunctionViewModel(INavigation navigation) : this(App.ServiceProvider.GetRequiredService<EngineeringMathContext>(), App.ServiceProvider.GetRequiredService<IFunctionController>(), navigation)
        {
        }
        public FunctionViewModel(EngineeringMathContext context, IFunctionController functionController, INavigation navigation) : base()
        {
            EngineeringMathContext = context;
            FunctionController = functionController;
            FunctionLinkCommand = new Command(OnTapped);
            Navigation = navigation;
        }
        public ObservableCollection<HomeMenuFunction> Functions
        {
            get
            {
                return new ObservableCollection<HomeMenuFunction>(
                    EngineeringMathContext
                    .Functions
                    .Select(x => new HomeMenuFunction(x))
                    .ToList());
            }
        }
        public async void OnTapped(object contextObj)
        {
            if (contextObj is HomeMenuFunction functionModel)
            {
                await FunctionController.SetFunctionAsync(functionModel.Id);
                var detailPage = new FunctionDetailPage(FunctionController);
                await Navigation.PushAsync(detailPage);
            }
        }

        public ICommand FunctionLinkCommand { get; }

        private EngineeringMathContext EngineeringMathContext { get; }
        private IFunctionController FunctionController { get; }
        public INavigation Navigation { get; }
    }
}
