using EngineeringMath.Controllers;
using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath
{
    public class Bootstrapper
    {
        private ILogger Logger { get; set; }

        public Bootstrapper SetLogger(ILogger logger)
        {
            Logger = logger;
            return this;
        }

        /// <summary>
        /// Adds the services to the service collection
        /// </summary>
        /// <param name="services"></param>
        public void Bootstrap(IServiceCollection services)
        {
            NullCheck();
            services.AddDbContext<EngineeringMathContext>();

            services.AddSingleton(Logger);
            services.AddSingleton<IFunctionController, FunctionController>();


            services.AddSingleton<IReadonlyRepository<EquationDB>, EquationDBRepository>();
            services.AddSingleton<IReadonlyCacheRepository<Equation>, EquationRepository>();

            services.AddSingleton<IReadonlyRepository<FunctionDB>, FunctionDBRepository>();
            services.AddSingleton<IReadonlyCacheRepository<Function>, FunctionRepository>();

            services.AddSingleton<IReadonlyRepository<ParameterDB>, ParameterDBRepository>();
            services.AddSingleton<IReadonlyCacheRepository<Parameter>, ParameterRepository>();

            services.AddSingleton<IReadonlyRepository<UnitCategoryDB>, UnitCategoryDBRepository>();
            services.AddSingleton<IReadonlyCacheRepository<UnitCategory>, UnitCategoryRepository>();

            services.AddSingleton<IValidator<Parameter>, ParameterValidator>();
        }

        private void NullCheck()
        {
            if (Logger == null)
            {
                throw new ArgumentNullException(nameof(Logger));
            }
        }
    }
}
