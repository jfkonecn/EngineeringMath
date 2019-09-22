using EngineeringMath.Model;
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

        public async Task<IEnumerable<EquationDB>> GetAllAsync()
        {
            return await Context.Equations
                .Include(x => x.Owner)
                .Include(x => x.Function)
                .ToListAsync();
        }

        public Task<IEnumerable<EquationDB>> GetAllWhereAsync(Func<EquationDB, bool> whereCondition)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EquationDB>> GetByIdAsync(IEnumerable<object> keys)
        {
            throw new NotImplementedException();
        }

        public Task<EquationDB> GetByIdAsync(object key)
        {
            throw new NotImplementedException();
        }
    }
}
