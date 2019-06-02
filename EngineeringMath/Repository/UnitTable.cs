using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repository
{
    public class UnitTable : ITableRepository<Unit>
    {
        public UnitTable(EngineeringMathDS dataSet)
        {
            DataSet = dataSet;
        }


        public TableActionResult<Unit> Create(Unit obj)
        {
            var unitCat = DataSet.UnitCategory.FindByUnitCategoryId(obj.UnitCategory.Id);
            EngineeringMathDS.UnitRow result = DataSet.Unit.AddUnitRow(
                unitCat, obj.Name, obj.Symbol, 
                obj.ConvertToSi, obj.ConvertFromSi, 
                obj.IsUserDefined, obj.IsOnAbsoluteScale);

            return new TableActionResult<Unit>()
            {
                Code = TableActionResultCode.Success,
                Message = "Unit Row Added",
                Result = System.Convert.FromUnitRow(result),
            };
        }

        public TableActionResult<Unit> Delete(Unit obj)
        {
            throw new NotImplementedException();
        }

        public TableActionResult<Unit> Get(int id)
        {
            throw new NotImplementedException();
        }

        public TableActionResult<IEnumerable<Unit>> GetAll()
        {
            throw new NotImplementedException();
        }

        public TableActionResult<Unit> Update(Unit obj)
        {
            throw new NotImplementedException();
        }



        public EngineeringMathDS DataSet { get; }
    }
}
