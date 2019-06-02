using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repository
{
    public interface ITableRepository<T> where T : IDBase
    {
        TableActionResult<T> Create(T obj);
        TableActionResult<T> Get(int id);
        TableActionResult<IEnumerable<T>> GetAll();
        TableActionResult<T> Update(T obj);
        TableActionResult<T> Delete(T obj);
    }
}
