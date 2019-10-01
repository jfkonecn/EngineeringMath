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
        public DbSet<EquationDB> Equations { get; set; }
        public DbSet<FunctionDB> Functions { get; set; }
        public DbSet<FunctionCategoryDB> FunctionCategories { get; set; }
        public DbSet<OwnerDB> Owners { get; set; }
        public DbSet<ParameterDB> Parameters { get; set; }
        public DbSet<ParameterTypeDB> ParameterTypes { get; set; }
        public DbSet<UnitDB> Units { get; set; }
        public DbSet<UnitCategoryDB> UnitCategories { get; set; }
        public DbSet<UnitSystemDB> UnitSystems { get; set; }
        public DbSet<FunctionOutputValueLinkDB> ParameterFunctionLinks { get; set; }
        public DbSet<ParameterValueLinkDB> ParameterValueLinks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=engineeringMath.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var seedData = new EngineeringMathSeedData();
            modelBuilder.Entity<OwnerDB>().HasData(seedData.Owners.Values);
            modelBuilder.Entity<UnitSystemDB>().HasData(seedData.UnitSystems.Values);
            modelBuilder.Entity<UnitCategoryDB>().HasData(seedData.UnitCategories.Values);
            modelBuilder.Entity<ParameterTypeDB>().HasData(seedData.ParameterTypes.Values);
            modelBuilder.Entity<FunctionDB>().HasData(seedData.Functions.Values);
            modelBuilder.Entity<FunctionCategoryDB>().HasData(seedData.FunctionCategories.Values);
        }
    }
}
