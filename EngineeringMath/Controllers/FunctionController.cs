using EngineeringMath.Model;
using EngineeringMath.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Controllers
{
    public class FunctionController
    {
        public FunctionController(IReadonlyRepository<FunctionDB> functionRepository)
        {
            FunctionRepository = functionRepository;
        }

        /// <summary>
        /// Sets the function to be controlled
        /// </summary>
        /// <param name="functionName"></param>
        public void SetFunction(string functionName)
        {
            Function = FunctionRepository.GetAllWhere(x => x.Name == functionName).ResultObject.Single();
        }

        private IReadonlyRepository<FunctionDB> FunctionRepository { get; }
        private FunctionDB Function { get; set; }
    }
}
