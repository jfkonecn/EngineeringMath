using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Controllers
{
    public class FunctionController
    {
        public FunctionController(IReadonlyRepository<Function> functionRepository)
        {
            FunctionRepository = functionRepository;
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
            Equation.Formula.Evaluate();
        }

        private IReadonlyRepository<Function> FunctionRepository { get; }
        private Function Function { get; set; }
        private Equation Equation { get; set; }
    }
}
