using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Repository
{
    public class TableActionResult<T>
    {
        public TableActionResultCode Code { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
