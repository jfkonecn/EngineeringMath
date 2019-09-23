using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Repositories
{
    public abstract class DBRepositoryBase<T> : IReadonlyRepository<T> where T : class
    {
        public DBRepositoryBase(EngineeringMathContext context)
        {
            Context = context;
        }

        private EngineeringMathContext Context { get; }

        public abstract async Task<IEnumerable<T>> GetAllAsync();

        public async Task<IEnumerable<T>> GetAllWhereAsync(Func<T, bool> whereCondition)
        {
            return (await GetAllAsync()).Where(whereCondition);
        }

        public async Task<IEnumerable<T>> GetByIdAsync(IEnumerable<object> keys)
        {
            return await GetAllWhereAsync(x => GetKeys(x).Where(key => keys.Contains(key)).Count() > 0);
        }

        public async Task<T> GetByIdAsync(object key)
        {
            return (await GetByIdAsync(new object[] { key })).FirstOrDefault();
        }

        private IDictionary<string, object> GetKeys(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = Context.Entry(entity);
            var primaryKey = entry.Metadata.FindPrimaryKey();
            var keys = primaryKey.Properties.ToDictionary(x => x.Name, x => x.PropertyInfo.GetValue(entity));

            return keys;
        }

    }
}
