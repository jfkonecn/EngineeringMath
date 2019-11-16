using EngineeringMath.EngineeringModel;
using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public abstract class DBRepositoryBase<T> : IReadonlyRepository<T> where T : IBuiltModel
    {

        public abstract Task<IEnumerable<T>> GetAllAsync();

        public async Task<IEnumerable<T>> GetAllWhereAsync(Func<T, bool> whereCondition)
        {
            return (await GetAllAsync()).Where(whereCondition);
        }

        public async Task<IEnumerable<T>> GetByIdAsync(IEnumerable<int> keys)
        {
            return await GetAllWhereAsync(x => keys.Contains(x.Id));
        }

        public async Task<T> GetByIdAsync(int key)
        {
            return (await GetByIdAsync(new int[] { key })).FirstOrDefault();
        }
    }
}
