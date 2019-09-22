using EngineeringMath.Model;
using EngineeringMath.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class EquationDBRepository : IReadonlyRepository<EquationDB>
    {
        public EquationDBRepository(EngineeringMathContext context)
        {
            Context = context;
        }

        public EngineeringMathContext Context { get; }

        public async Task<IResult<RepositoryStatusCode, IEnumerable<EquationDB>>> GetAllAsync()
        {
            var result = await Context.Equations.Include(x => x.Owner).Include(x => x.Function).ToListAsync();
            var status = result.Count() == 0 ? RepositoryStatusCode.objectNotFound : RepositoryStatusCode.success;

            return new RepositoryResult<IEnumerable<EquationDB>>(status, result);
        }

        public Task<IResult<RepositoryStatusCode, IEnumerable<EquationDB>>> GetAllWhereAsync(Func<EquationDB, bool> whereCondition)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<RepositoryStatusCode, IEnumerable<EquationDB>>> GetByIdAsync(IEnumerable<object> keys)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<RepositoryStatusCode, EquationDB>> GetByIdAsync(object key)
        {
            throw new NotImplementedException();
        }
    }
}
