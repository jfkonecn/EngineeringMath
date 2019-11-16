using System.Threading.Tasks;

namespace EngineeringMath.Controllers
{
    public interface IFunctionController
    {
        Task EvaluateAsync();
        Task SetEquationAsync(int equationId);
        Task SetFunctionAsync(int functionId);
    }
}