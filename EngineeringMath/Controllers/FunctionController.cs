using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Results;
using EngineeringMath.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Controllers
{
    public class FunctionController
    {
        public FunctionController(IReadonlyRepository<Function> functionRepository, IValidator<Parameter> parameterValidator)
        {
            FunctionRepository = functionRepository;
            ParameterValidator = parameterValidator;
        }

        /// <summary>
        /// Sets the function to be controlled
        /// </summary>
        /// <param name="functionName"></param>
        public RepositoryStatusCode SetFunction(string functionName)
        {
            IResult<RepositoryStatusCode, Function> result = FunctionRepository.GetById(functionName);
            Function = result.ResultObject;
            return result.StatusCode;
        }


        public RepositoryStatusCode SetEquation(string equationName)
        {
            Equation = Function.Equations.FirstOrDefault(x => x.Name == equationName);
            return Equation == null ? RepositoryStatusCode.objectNotFound : RepositoryStatusCode.success;
        }

        public void Evaluate()
        {
            Dictionary<string, Parameter> curParams = Function
                .Parameters
                .Where(x => Equation.Formula.EquationArguments
                .Contains(x.KeyName))
                .ToDictionary(x => x.KeyName, x => x);
            foreach (Parameter parameter in curParams.Values)
            {
                ParameterValidator.Validate(parameter);
            }
            var paraArr = new double[curParams.Values.Count()];
            for (int i = 0; i < paraArr.Length; i++)
            {
                string parameterKey = Equation.Formula.EquationArguments[i];
                paraArr[i] = curParams[parameterKey].Value;
            }
            Equation.Formula.Evaluate(paraArr);
        }

        private IReadonlyRepository<Function> FunctionRepository { get; }
        public IValidator<Parameter> ParameterValidator { get; }
        private Function Function { get; set; }
        private Equation Equation { get; set; }
    }
}
