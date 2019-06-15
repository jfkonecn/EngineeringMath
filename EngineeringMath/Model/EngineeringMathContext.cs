using EngineeringMath.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Model
{
    /// <summary>
    /// EF Help
    /// https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started
    /// https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
    /// Install-Package Microsoft.EntityFrameworkCore.Tools -ProjectName EngineeringMath.Migrations.Startup
    /// Add-Migration InitialCreate -StartupProject EngineeringMath.Migrations.Startup
    /// Remove-Migration -StartupProject EngineeringMath.Migrations.Startup
    /// Make sure PM is pointing to EngineeringMath!!!
    /// </summary>
    public class EngineeringMathContext : DbContext
    {
        public DbSet<Equation> Equations { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<FunctionCategory> FunctionCategories { get; set; }
        public DbSet<ImportedClass> ImportedClasses { get; set; }
        public DbSet<ImportedEquation> ImportedEquations { get; set; }
        public DbSet<ImportedMethod> ImportedMethods { get; set; }
        public DbSet<ImportedNamespace> ImportedNamespaces { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<ParameterType> ParameterTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitCategory> UnitCategories { get; set; }
        public DbSet<UnitSystem> UnitSystems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=engineeringMath.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Owner system = new Owner()
            {
                Name = "SYSTEM"
            };
            modelBuilder.Entity<Owner>().HasData(system);

            modelBuilder.Entity<UnitSystem>().HasData(
                new UnitSystem()
            {
                Owner = system,
            });

            UnitCategory length = new UnitCategory()
            {
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.Meters),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        Symbol = "m",
                        UnitSystems =
                        {

                        },
                        Owner = system
                    }
                },
                Name = nameof(LibraryResources.Area),
                Owner = system
            };

            UnitCategory area = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 2",
                Name = nameof(LibraryResources.Length),
                Owner = system
            };
        }
    }
}
