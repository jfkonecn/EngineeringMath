﻿using EngineeringMath.App.ViewModels;
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
    public partial class FunctionsPage : ContentPage
    {
        FunctionViewModel VM { get; }
        public FunctionsPage()
        {
            InitializeComponent();
            VM = (FunctionViewModel)BindingContext;
        }
    }
}