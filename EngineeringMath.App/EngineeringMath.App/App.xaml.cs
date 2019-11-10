using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EngineeringMath.App.Services;
using EngineeringMath.App.Views;
using Microsoft.EntityFrameworkCore;
using EngineeringMath.Model;

namespace EngineeringMath.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IContextService, ContextService>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
