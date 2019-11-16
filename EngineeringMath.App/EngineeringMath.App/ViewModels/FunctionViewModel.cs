using EngineeringMath.App.Models;
using EngineeringMath.App.Services;
using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EngineeringMath.App.ViewModels
{
    public class FunctionViewModel : BaseViewModel
    {
        public FunctionViewModel() : base()
        {
            Context = DependencyService.Resolve<IContextService>();
            FunctionLinkCommand = new Command(OnTapped);
        }
        public FunctionViewModel(IContextService context) : base()
        {
            Context = context;
        }
        public ObservableCollection<HomeMenuFunction> Functions
        {
            get
            {
                return new ObservableCollection<HomeMenuFunction>(
                    Context
                    .EngineeringMathContext
                    .Functions
                    .Select(x => new HomeMenuFunction(x))
                    .ToList());
            }
        }
        public static void OnTapped(object contextObj)
        {
            if(contextObj is HomeMenuFunction functionModel)
            {

            }
        }

        public ICommand FunctionLinkCommand { get; }

        private IContextService Context { get; }
    }
}
