using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class ParameterDBRepository : DBRepositoryBase<ParameterDB>
    {
        public ParameterDBRepository(EngineeringMathContext context) : base(context)
        {
            Context = context;
        }

        public EngineeringMathContext Context { get; }

        public override async Task<IEnumerable<ParameterDB>> GetAllAsync()
        {
            return await Context.Parameters
                .Include(x => x.ParameterType)
                .Include(x => x.UnitCategory)
                .Include(x => x.ValueLinks)
                .Include(x => x.FunctionLinks)
                .Include(x => x.Owner)
                .ToListAsync();
        }
    }
}
