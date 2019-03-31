using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace DataSetBuilder.Table
{
    public abstract class BaseTable<T> : DataTable where T : class
    {
        protected BaseTable()
        {
            DataTable table = new DataTable("Prescription");
            TableName = $"{typeof(T).Name}Table";
            AddColumns();
            AddRows();
        }

        private void AddColumns()
        {
            foreach(var prop in typeof(T).GetProperties())
            {
                Columns.Add(prop.Name, prop.PropertyType);
            }
        }

        private void AddRows()
        {
            foreach(var record in GetSeedData())
            {
                Rows.Add(typeof(T).GetProperties().Select(x => x.GetValue(record)).ToArray());
            }
        }




        protected abstract IEnumerable<T> GetSeedData();


    }
}
