using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class EquationRepository : ReadonlyCacheRepositoryBase<string, BuiltEquation, Equation>
    {
        public EquationRepository(
            EngineeringMathContext dbContext,
            IStringEquationFactory stringEquationFactory,
            ILogger logger) : base(logger)
        {
            DbContext = dbContext;
            StringEquationFactory = stringEquationFactory;
            Logger = logger;
        }

        private EngineeringMathContext DbContext { get; }
        private IStringEquationFactory StringEquationFactory { get; }
        private ILogger Logger { get; }



        protected override async Task<IEnumerable<BuiltEquation>> BuildTAsync(Func<Equation, bool> whereCondition)
        {
            var blueprints = await DbContext.Equations
                .Include(x => x.Owner)
                .Include(x => x.Function)
                .ToListAsync();
                

            return BuildT(blueprints.Where(whereCondition));
        }


        private IEnumerable<BuiltEquation> BuildT(IEnumerable<Equation> blueprints)
        {
            List<BuiltEquation> equations = new List<BuiltEquation>();
            foreach (Equation equation in blueprints)
            {
                try
                {
                    equations.Add(
                    new BuiltEquation()
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

        protected override string GetKey(BuiltEquation obj)
        {
            return obj.Name;
        }

        protected override string GetKey(Equation obj)
        {
            return obj.Name;
        }


    }
}
