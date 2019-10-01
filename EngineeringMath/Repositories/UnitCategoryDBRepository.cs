using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public class UnitCategoryDBRepository : DBRepositoryBase<UnitCategoryDB>
    {
        public UnitCategoryDBRepository(EngineeringMathContext context) : base(context)
        {
            Context = context;
        }

        public EngineeringMathContext Context { get; }

        public override async Task<IEnumerable<UnitCategoryDB>> GetAllAsync()
        {
            return await Context
                .UnitCategories
                .Include(x => x.Units)
                .Include(x => x.Owner)
                .ToListAsync();
        }
    }
}
