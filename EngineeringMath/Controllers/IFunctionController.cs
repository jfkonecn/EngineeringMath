using System.Threading.Tasks;

namespace EngineeringMath.Controllers
{
    public interface IFunctionController
    {
        Task EvaluateAsync();
        Task SetEquationAsync(string equationName);
        Task SetFunctionAsync(string functionName);
    }
}