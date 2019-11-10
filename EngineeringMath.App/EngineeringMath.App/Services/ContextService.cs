using EngineeringMath.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EngineeringMath.App.Services
{
    public class ContextService : IContextService
    {
        public ContextService()
        {
            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"engineeringMath.db");
            var options = new DbContextOptionsBuilder<EngineeringMathContext>()
                .UseSqlite($"Data Source={dbFilePath}")
                .Options;

            //https://xamarinhelp.com/entity-framework-core-xamarin-forms/
            EngineeringMathContext = new EngineeringMathContext(options);

#if DEBUG
            EngineeringMathContext.Database.EnsureDeleted();
#endif
            EngineeringMathContext.Database.EnsureCreated();
        }
        public EngineeringMathContext EngineeringMathContext { get; }
    }
}
