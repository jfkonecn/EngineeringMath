﻿using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class EquationRepository : ReadonlyCacheRepositoryBase<string, Equation, EquationDB>
    {
        public EquationRepository(
            IReadonlyRepository<EquationDB> equationRepository,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(equationRepository, logger)
        {
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

        public IStringEquationFactory StringEquationFactory { get; }
        public ILogger Logger { get; }


        protected override Task<IEnumerable<Equation>> BuildTAsync(IEnumerable<EquationDB> blueprints)
        {
            return new Task<IEnumerable<Equation>>(() => BuildT(blueprints));
        }


        private IEnumerable<Equation> BuildT(IEnumerable<EquationDB> blueprints)
        {
            List<Equation> equations = new List<Equation>();
            foreach (EquationDB equation in blueprints)
            {
                try
                {
                    equations.Add(
                    new Equation()
                    {
                        Formula = StringEquationFactory.CreateStringEquation(equation.Formula),
                        FunctionName = equation.Function.Name,
                        OutputName = equation.OutputName,
                        OwnerName = equation.Owner.Name,
                    });
                }
                catch (Exception e)
                {
                    Logger.LogError($"{nameof(EquationRepository)}", $"ID-\"{equation.EquationId}\" Formula-\"{equation.Formula}\" has an error with the message ${e.Message}");
                    throw;
                }
            }
            return equations;
        }

        protected override string GetKey(Equation obj)
        {
            return obj.Name;
        }

        protected override string GetKey(EquationDB obj)
        {
            return obj.Name;
        }
    }
}
