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
    public static class Bootstrapper
    {
        /// <summary>
        /// Adds the services to the service collection
        /// </summary>
        /// <param name="services"></param>
        public static void Bootstrap(IServiceCollection services)
        {
            services.AddDbContext<EngineeringMathContext>();

            services.AddSingleton<IFunctionController, FunctionController>();


            services.AddSingleton<IReadonlyCacheRepository<BuiltEquation>, EquationRepository>();

            services.AddSingleton<IReadonlyCacheRepository<BuiltFunction>, FunctionRepository>();

            services.AddSingleton<IReadonlyCacheRepository<BuiltParameter>, ParameterRepository>();

            services.AddSingleton<IReadonlyCacheRepository<BuiltUnitCategory>, UnitCategoryRepository>();

            services.AddSingleton<IValidator<BuiltParameter>, ParameterValidator>();
        }

    }
}
