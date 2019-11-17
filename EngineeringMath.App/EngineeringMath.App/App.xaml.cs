using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EngineeringMath.App.Views;
using Microsoft.EntityFrameworkCore;
using EngineeringMath.Model;
using EngineeringMath.Controllers;
using EngineeringMath.Repositories;
using EngineeringMath.EngineeringModel;
using EngineeringMath.Validators;
using StringMath;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Logging;
using EngineeringMath.App.Services;

namespace EngineeringMath.App
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();
            //DependencyService.Register<IContextService, ContextService>();
            //DependencyService.Register<IFunctionController, FunctionController>();
            //DependencyService.Register<IReadonlyRepository<BuiltFunction>, FunctionRepository>(); 
            //DependencyService.Register<IReadonlyRepository<BuiltEquation>, EquationRepository>();
            //DependencyService.Register<IReadonlyRepository<BuiltParameter>, ParameterRepository>();
            //DependencyService.Register<IReadonlyRepository<BuiltUnitCategory>, UnitCategoryRepository>();
            //DependencyService.Register<IStringEquationFactory, StringEquationFactory>();
            //DependencyService.Register<IValidator<BuiltParameter>, ParameterValidator>();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            EngineeringMathContext engineeringMathContext = ServiceProvider.GetRequiredService<EngineeringMathContext>();
#if DEBUG
            engineeringMathContext.Database.EnsureDeleted();
#endif
            engineeringMathContext.Database.EnsureCreated();
            MainPage = new MainPage();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {


            //https://xamarinhelp.com/entity-framework-core-xamarin-forms/

            serviceCollection
                .AddDbContext<EngineeringMathContext>(SetupDbOptions, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddSingleton<IFunctionController, FunctionController>()
                .AddSingleton<IReadonlyRepository<BuiltFunction>, FunctionRepository>()
                .AddSingleton<IReadonlyRepository<BuiltEquation>, EquationRepository>()
                .AddSingleton<IReadonlyRepository<BuiltParameter>, ParameterRepository>()
                .AddSingleton<IReadonlyRepository<BuiltUnitCategory>, UnitCategoryRepository>()
                .AddSingleton<IStringEquationFactory, StringEquationFactory>()
                .AddSingleton<IValidator<BuiltParameter>, ParameterValidator>()
                .AddSingleton<ILogger, LocalLogger>()
                .AddLogging();


        }

        private void SetupDbOptions(DbContextOptionsBuilder obj)
        {
            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "engineeringMath.db");
            obj.UseSqlite($"Data Source={dbFilePath}");
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
