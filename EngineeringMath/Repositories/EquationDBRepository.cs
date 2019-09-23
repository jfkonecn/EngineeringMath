using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class EquationDBRepository : DBRepositoryBase<EquationDB>
    {
        public EquationDBRepository(EngineeringMathContext context) : base(context)
        {
            Context = context;
        }

        private EngineeringMathContext Context { get; }

        public override async Task<IEnumerable<EquationDB>> GetAllAsync()
        {
            return await Context.Equations
                .Include(x => x.Owner)
                .Include(x => x.Function)
                .ToListAsync();
        }

    }
}
