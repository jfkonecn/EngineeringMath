using EngineeringMath.Model;
using EngineeringMath.Repositories;
using System;
using System.Collections.Generic;
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
        public void SetFunction(string functionName)
        {
            FunctionRepository.GetAllWhere(x => x.Name == functionName);
        }

        private IReadonlyRepository<Function> FunctionRepository { get; }
    }
}
