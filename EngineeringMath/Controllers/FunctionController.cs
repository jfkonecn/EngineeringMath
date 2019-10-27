using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using EngineeringMath.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Controllers
{
    public class FunctionController : IFunctionController
    {
        public FunctionController(IReadonlyRepository<BuiltFunction> functionRepository, IValidator<BuiltParameter> parameterValidator)
        {
            FunctionRepository = functionRepository;
            ParameterValidator = parameterValidator;
        }

        /// <summary>
        /// Sets the function to be controlled
        /// </summary>
        /// <param name="functionName"></param>
        public async Task SetFunctionAsync(string functionName)
        {
            Function = await FunctionRepository.GetByIdAsync(functionName) ??
                throw new ArgumentException(string.Format(LibraryResources.FunctionCouldNotBeFound, functionName));
        }


        public Task SetEquationAsync(string equationName)
        {
            return new Task(() =>
            {
                Equation = Function.Equations.FirstOrDefault(x => x.Name == equationName) ??
                    throw new ArgumentException(string.Format(LibraryResources.EquationCouldNotBeFound, equationName));
            });
        }

        public Task EvaluateAsync()
        {
            return new Task(() =>
            {
                Dictionary<string, BuiltParameter> curParams = Function
                    .Parameters
                    .Where(x => Equation.Formula.EquationArguments
                    .Contains(x.ParameterName))
                    .ToDictionary(x => x.ParameterName, x => x);
                foreach (BuiltParameter parameter in curParams.Values)
                {
                    ParameterValidator.Validate(parameter);
                }
                double[] paraArr = new double[curParams.Values.Count()];
                for (int i = 0; i < paraArr.Length; i++)
                {
                    string parameterKey = Equation.Formula.EquationArguments[i];
                    paraArr[i] = curParams[parameterKey].Value;
                }
                double result = Equation.Formula.Evaluate(paraArr);
                BuiltParameter outputParameter = curParams[Equation.OutputName];
                outputParameter.Value = result;
                foreach (string parameterLink in outputParameter.ValueLinks)
                {
                    curParams[parameterLink].Value = result;
                }
            });
        }

        private IReadonlyRepository<BuiltFunction> FunctionRepository { get; }
        private IValidator<BuiltParameter> ParameterValidator { get; }
        private BuiltFunction Function { get; set; }
        private BuiltEquation Equation { get; set; }
    }
}
