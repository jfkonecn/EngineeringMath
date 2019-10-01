using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class FunctionDBRepository : DBRepositoryBase<FunctionDB>
    {
        public FunctionDBRepository(EngineeringMathContext context) : base(context)
        {
            Context = context;
        }
        private EngineeringMathContext Context { get; }

        public override async Task<IEnumerable<FunctionDB>> GetAllAsync()
        {
            return await Context.Functions
                .Include(x => x.Owner)
                .Include(x => x.Equations)
                .Include(x => x.Parameters)
                .ToListAsync();
        }
    }
}
