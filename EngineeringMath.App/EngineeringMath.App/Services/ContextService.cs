using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.App.Services
{
    public class ContextService : IContextService
    {
        public ContextService()
        {
            var options = new DbContextOptionsBuilder<EngineeringMathContext>().Options;

            EngineeringMathContext = new EngineeringMathContext(options);
#if DEBUG
            EngineeringMathContext.Database.EnsureDeleted();
#endif
            EngineeringMathContext.Database.EnsureCreated();
        }
        public EngineeringMathContext EngineeringMathContext { get; }
    }
}
