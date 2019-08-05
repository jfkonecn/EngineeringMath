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
        public DbSet<FunctionOutputValueLink> ParameterFunctionLinks { get; set; }
        public DbSet<ParameterValueLink> ParameterValueLinks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=engineeringMath.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Owner
            Owner system = new Owner()
            {
                Name = "SYSTEM"
            };
            modelBuilder.Entity<Owner>().HasData(system);
            #endregion
            #region UnitSystem
            UnitSystem siUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.SIAbbrev),
                Name = nameof(LibraryResources.SIFullName),
                Owner = system
            };
            UnitSystem uscsUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.USCSAbbrev),
                Name = nameof(LibraryResources.USCSFullName),
                Owner = system
            };
            UnitSystem imperialUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.ImperialAbbrev),
                Name = nameof(LibraryResources.ImperialFullName),
                Owner = system,
                Children =
                {
                    uscsUnits
                }
            };
            UnitSystem metricUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.MetricAbbrev),
                Name = nameof(LibraryResources.MetricFullName),
                Owner = system,
                Children = new List<UnitSystem>()
                {
                    siUnits
                }
            };
            modelBuilder.Entity<UnitSystem>().HasData(siUnits, uscsUnits, imperialUnits, metricUnits);
            #endregion
            #region UnitCategory


            UnitCategory area = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 2",
                Name = nameof(LibraryResources.Area),
                Owner = system
            };

            UnitCategory density = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Mass)} / ${nameof(LibraryResources.Volume)}",
                Name = nameof(LibraryResources.Density),
                Owner = system
            };
            UnitCategory energy = new UnitCategory()
            {
                Name = nameof(LibraryResources.Energy),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.JoulesFullName),
                        Symbol = nameof(LibraryResources.JoulesAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.BTUFullName),
                        Symbol = nameof(LibraryResources.BTUAbbrev),
                        ConvertFromSi = "$0 / 1055.06",
                        ConvertToSi = "$0 * 1055.06",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilocaloriesFullName),
                        Symbol = nameof(LibraryResources.KilocaloriesAbbrev),
                        ConvertFromSi = "$0 / 4184",
                        ConvertToSi = "$0 * 4184",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilojoulesFullName),
                        Symbol = nameof(LibraryResources.KilojoulesAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.ThermsFullName),
                        Symbol = nameof(LibraryResources.ThermsAbbrev),
                        ConvertFromSi = "$0 / 1.055e+8",
                        ConvertToSi = "$0 * 1.055e+8",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    }
                },
            };
            UnitCategory enthalpy = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.Enthalpy),
                Owner = system
            };
            UnitCategory entropy = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / (${nameof(LibraryResources.Mass)} * ${nameof(LibraryResources.Temperature)})",
                Name = nameof(LibraryResources.Entropy),
                Owner = system
            };
            UnitCategory isothermalCompressibility = new UnitCategory()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Pressure)}",
                Name = nameof(LibraryResources.IsothermalCompressibility),
                Owner = system
            };
            UnitCategory length = new UnitCategory()
            {
                Name = nameof(LibraryResources.Length),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MeterFullName),
                        Symbol = nameof(LibraryResources.MeterAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.FeetFullName),
                        Symbol = nameof(LibraryResources.FeetAbbrev),
                        ConvertFromSi = "$0 * 3.28084",
                        ConvertToSi = "$0 / 3.28084",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.InchesFullName),
                        Symbol = nameof(LibraryResources.InchesAbbrev),
                        ConvertFromSi = "$0 * 39.3701",
                        ConvertToSi = "$0 / 39.3701",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MilesFullName),
                        Symbol = nameof(LibraryResources.MilesAbbrev),
                        ConvertFromSi = "$0 / 1609.34",
                        ConvertToSi = "$0 * 1609.34",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MillimetersFullName),
                        Symbol = nameof(LibraryResources.MillimetersAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.CentimetersFullName),
                        Symbol = nameof(LibraryResources.CentimetersAbbrev),
                        ConvertFromSi = "$0 * 1e2",
                        ConvertToSi = "$0 / 1e2",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilometersFullName),
                        Symbol = nameof(LibraryResources.KilometersAbbrev),
                        ConvertFromSi = "$0 / 1e3",
                        ConvertToSi = "$0 * 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    }
                },
            };
            UnitCategory mass = new UnitCategory()
            {
                Name = nameof(LibraryResources.Mass),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilogramsFullName),
                        Symbol = nameof(LibraryResources.KilogramsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.PoundsMassFullName),
                        Symbol = nameof(LibraryResources.PoundsMassAbbrev),
                        ConvertFromSi = "$0 * 2.20462",
                        ConvertToSi = "$0 / 2.20462",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.GramsFullName),
                        Symbol = nameof(LibraryResources.GramsAbbrev),
                        ConvertFromSi = "$0 * 1000",
                        ConvertToSi = "$0 / 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MilligramsFullName),
                        Symbol = nameof(LibraryResources.MilligramsAbbrev),
                        ConvertFromSi = "$0 * 1e6",
                        ConvertToSi = "$0 / 1e6",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MicrogramsFullName),
                        Symbol = nameof(LibraryResources.MicrogramsAbbrev),
                        ConvertFromSi = "$0 * 1e9",
                        ConvertToSi = "$0 / 1e9",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MetricTonsFullName),
                        Symbol = nameof(LibraryResources.MetricTonsAbbrev),
                        ConvertFromSi = "$0 / 1e3",
                        ConvertToSi = "$0 * 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.OuncesFullName),
                        Symbol = nameof(LibraryResources.OuncesAbbrev),
                        ConvertFromSi = "$0 * 35.274",
                        ConvertToSi = "$0 / 35.274",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.USTonsFullName),
                        Symbol = nameof(LibraryResources.USTonsAbbrev),
                        ConvertFromSi = "$0 / 907.185",
                        ConvertToSi = "$0 * 907.185",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                },
            };

            UnitCategory power = new UnitCategory()
            {
                Name = nameof(LibraryResources.Power),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.WattsFullName),
                        Symbol = nameof(LibraryResources.WattsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.HorsepowerFullName),
                        Symbol = nameof(LibraryResources.HorsepowerAbbrev),
                        ConvertFromSi = "$0 / 745.7",
                        ConvertToSi = "$0 * 745.7",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilowattFullName),
                        Symbol = nameof(LibraryResources.KilowattAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },
                
            };

            UnitCategory pressure = new UnitCategory()
            {
                Name = nameof(LibraryResources.Pressure),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.PascalsFullName),
                        Symbol = nameof(LibraryResources.PascalsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.PoundsForcePerSqInFullName),
                        Symbol = nameof(LibraryResources.PoundsForcePerSqInAbbrev),
                        ConvertFromSi = "$0 / 6894.76",
                        ConvertToSi = "$0 * 6894.76",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.AtmospheresFullName),
                        Symbol = nameof(LibraryResources.AtmospheresAbbrev),
                        ConvertFromSi = "$0 / 101325",
                        ConvertToSi = "$0 * 101325",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.BarFullName),
                        Symbol = nameof(LibraryResources.BarAbbrev),
                        ConvertFromSi = "$0 / 1e5",
                        ConvertToSi = "$0 * 1e5",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilopascalsFullName),
                        Symbol = nameof(LibraryResources.KilopascalsAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.TorrFullName),
                        Symbol = nameof(LibraryResources.TorrAbbrev),
                        ConvertFromSi = "$0 / 133.322",
                        ConvertToSi = "$0 * 133.322",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },
            };
            UnitCategory specificVolume = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.SpecificVolume),
                Owner = system
            };

            UnitCategory temperature = new UnitCategory()
            {
                Name = nameof(LibraryResources.Temperature),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KelvinFullName),
                        Symbol = nameof(LibraryResources.KelvinAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.RankineFullName),
                        Symbol = nameof(LibraryResources.RankineAbbrev),
                        ConvertFromSi = "$0 / 1.8",
                        ConvertToSi = "$0 * 1.8",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.FahrenheitFullName),
                        Symbol = nameof(LibraryResources.FahrenheitAbbrev),
                        ConvertFromSi = "($0 - 273.15) * 9 / 5 + 32",
                        ConvertToSi = "($0 - 32) * 5 / 9 + 273.15",
                        IsOnAbsoluteScale = false,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.CelsiusFullName),
                        Symbol = nameof(LibraryResources.CelsiusAbbrev),
                        ConvertFromSi = "$0 - 273.15",
                        ConvertToSi = "$0 + 273.15",
                        IsOnAbsoluteScale = false,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },

            };

            UnitCategory time = new UnitCategory()
            {
                Name = nameof(LibraryResources.Time),
                Owner = system,
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MinutesFullName),
                        Symbol = nameof(LibraryResources.MinutesAbbrev),
                        ConvertFromSi = "$0 / 60",
                        ConvertToSi = "$0 * 60",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.HoursFullName),
                        Symbol = nameof(LibraryResources.HoursAbbrev),
                        ConvertFromSi = "$0 / 3600",
                        ConvertToSi = "$0 * 3600",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MillisecondsFullName),
                        Symbol = nameof(LibraryResources.MillisecondsAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.DaysFullName),
                        Symbol = nameof(LibraryResources.DaysAbbrev),
                        ConvertFromSi = "$0 / (3600 * 24)",
                        ConvertToSi = "$0 * 3600 * 24",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                },

            };

            UnitCategory velocity = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.Velocity),
                Owner = system
            };


            UnitCategory volume = new UnitCategory()
            {
                Name = nameof(LibraryResources.Volume),
                Owner = system,
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 3",
                Units = new List<Unit>()
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.GallonsFullName),
                        Symbol = nameof(LibraryResources.GallonsAbbrev),
                        ConvertFromSi = "$0 * 264.172",
                        ConvertToSi = "$0 / 264.172",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.LitersFullName),
                        Symbol = nameof(LibraryResources.LitersAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MillilitersFullName),
                        Symbol = nameof(LibraryResources.MillilitersAbbrev),
                        ConvertFromSi = "$0 * 1e6",
                        ConvertToSi = "$0 / 1e6",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },

            };
            UnitCategory volumeExpansivity = new UnitCategory()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Temperature)}",
                Name = nameof(LibraryResources.VolumeExpansivity),
                Owner = system
            };
            UnitCategory volumetricFlowRate = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.VolumetricFlowRate),
                Owner = system
            };
            modelBuilder.Entity<UnitCategory>().HasData(
                area, density, energy,
                enthalpy, entropy, isothermalCompressibility,
                length, mass, pressure, specificVolume,
                temperature, time, velocity, volume, volumeExpansivity,
                volumetricFlowRate);
            #endregion
            #region ParameterType
            ParameterType integerType = new ParameterType()
            {
                Name = nameof(Int32),
                Owner = system
            };
            ParameterType doubleType = new ParameterType()
            {
                Name = nameof(Double),
                Owner = system,
            };
            ParameterType unitCategoryType = new ParameterType()
            {
                Name = nameof(UnitCategory),
                Owner = system,
            };
            modelBuilder.Entity<ParameterType>().HasData(
                integerType, doubleType, unitCategoryType
                );
            #endregion
            #region Function
            Function areaFunction = new Function()
            {
                Name = nameof(LibraryResources.Area),
                Owner = system,
                Parameters =
                {
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Diameter),
                        Owner = system,
                        ValueConditions = "$0 >= 0",
                        ParameterType = doubleType,
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Area),
                        Owner = system,
                        ValueConditions = "$0 >= 0",
                        ParameterType = doubleType,
                    }
                },
                Equations =
                {
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.Area),
                        Formula = $"{nameof(LibraryResources.Diameter)} ^ 2 * PI() / 4",
                        Owner = system
                    }
                },
            };
            FunctionOutputValueLink areaFunctionLink = new FunctionOutputValueLink()
            {
                Function = areaFunction,
                OutputParameterName = nameof(LibraryResources.Area)
            };
            Function unitConverter = new Function()
            {
                Name = nameof(LibraryResources.UnitConverter),
                Owner = system,
                Equations = new List<Equation>()
                {
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.Output),
                        Formula = $"${nameof(LibraryResources.Input)}",
                        Owner = system
                    }
                },
                Parameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.UnitType),
                        Owner = system,
                        ParameterType = unitCategoryType,
                        ValueLinks =
                        {
                            new ParameterValueLink()
                            {
                                ParameterName = nameof(LibraryResources.Input)
                            },
                            new ParameterValueLink()
                            {
                                ParameterName = nameof(LibraryResources.Output)
                            }
                        }
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Input),
                        Owner = system,
                        ParameterType = doubleType,
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Output),
                        Owner = system,
                        ParameterType = doubleType,
                    }
                }
            };
            // https://github.com/jfkonecn/OpenChE/blob/Better_UI/Backend/EngineeringMath/Component/DefaultFunctions/BernoullisEquation.cs
            Function bernoullisEquation = new Function()
            {
                Name = nameof(LibraryResources.BernoullisEquation),
                Owner = system,
                Parameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.InletVelocity),
                        ParameterType = doubleType,
                        UnitCategory = velocity,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.OutletVelocity),
                        ParameterType = doubleType,
                        UnitCategory = velocity,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.InletPressure),
                        ParameterType = doubleType,
                        UnitCategory = pressure,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.OutletPressure),
                        ParameterType = doubleType,
                        UnitCategory = pressure,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.InletHeight),
                        ParameterType = doubleType,
                        UnitCategory = length,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.OutletHeight),
                        ParameterType = doubleType,
                        UnitCategory = length,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Density),
                        ParameterType = doubleType,
                        UnitCategory = density,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    }
                },
                Equations = new List<Equation>()
                {
                    new Equation()
                    {
                        Formula = $@"Sqrt(2 * (${LibraryResources.OutletVelocity} ^ 2 / 2 
                            + 9.81 * (${LibraryResources.OutletHeight} - ${LibraryResources.InletHeight} ) 
                            + (${LibraryResources.OutletPressure} - ${LibraryResources.InletPressure} ) 
                                / ${LibraryResources.Density} ))",
                        OutputName = LibraryResources.InletVelocity,
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"Sqrt(2 * (${nameof(LibraryResources.InletVelocity)} ^ 2 / 2 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) 
                                / ${nameof(LibraryResources.Density)}))",
                        OutputName = nameof(LibraryResources.OutletVelocity),
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.OutletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.OutletHeight)} - ${nameof(LibraryResources.InletHeight)}) 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.InletPressure),
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.InletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.OutletPressure),
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"((${nameof(LibraryResources.OutletPressure)} - ${nameof(LibraryResources.InletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.OutletHeight)}",
                        OutputName = nameof(LibraryResources.InletHeight),
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"((${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.InletHeight)}",
                        OutputName = nameof(LibraryResources.OutletHeight),
                        Owner = system
                    },
                    new Equation()
                    {
                        Formula = $@"($pout - $pin) / (0.5 * ($uin ^ 2 - $uout ^ 2) 
                            + 9.81 * ($hin - ${nameof(LibraryResources.OutletHeight)}))",
                        OutputName = nameof(LibraryResources.Density),
                        Owner = system
                    }
                }
            };
            Function orificePlate = new Function()
            {
                Name = $"{LibraryResources.OrificePlate}",
                Owner = system,
                Parameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.DischargeCoefficient),
                        ParameterType = integerType,
                        UnitCategory = null,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.Density),
                        ParameterType = doubleType,
                        UnitCategory = density,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.InletPipeArea),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        FunctionLinks =
                        {
                            areaFunctionLink
                        },
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.OrificeArea),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        FunctionLinks =
                        {
                            areaFunctionLink
                        },
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.PressureDrop),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.VolumetricFlowRate),
                        ParameterType = doubleType,
                        UnitCategory = volumetricFlowRate,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                },
                Equations = new List<Equation>()
                {
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.DischargeCoefficient),
                        Formula = $@"${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.InletPipeArea)} * 
                            Sqrt((2 * ${nameof(LibraryResources.PressureDrop)}) / (${nameof(LibraryResources.Density)} * 
                                (${nameof(LibraryResources.InletPipeArea)} ^ 2 / ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))))",
                        Owner = system
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.Density),
                        Formula = $@"(2 * ${nameof(LibraryResources.PressureDrop)}) / 
                            (((${nameof(LibraryResources.VolumetricFlowRate)}  / 
                            (${nameof(LibraryResources.DischargeCoefficient)} * 
                            ${nameof(LibraryResources.InletPipeArea)})) ^ 2) * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / 
                        ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))",
                        Owner = system
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.InletPipeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.OrificeArea)} ^ 2) - 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        Owner = system
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.OrificeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.InletPipeArea)} ^ 2) + 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / 
                                (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        Owner = system
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.PressureDrop),
                        Formula = $@"((${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.DischargeCoefficient)} * 
                            ${nameof(LibraryResources.InletPipeArea)})) ^ 2 * 
                            (${nameof(LibraryResources.Density)} * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / 
                                ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))) / 2",
                        Owner = system
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.VolumetricFlowRate),
                        Formula = $@"${nameof(LibraryResources.DischargeCoefficient)} * 
                        ${nameof(LibraryResources.InletPipeArea)} * Sqrt((2 * ${nameof(LibraryResources.PressureDrop)}) 
                            / (${nameof(LibraryResources.Density)} * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1)))",
                        Owner = system
                    }
                },
            };
            modelBuilder.Entity<Function>().HasData(
                orificePlate, bernoullisEquation, unitConverter, areaFunction
                );
            #endregion
            #region FuncationCategories
            FunctionCategory fluidDynamics = new FunctionCategory()
            {
                Name = nameof(LibraryResources.FluidDynamics),
                Functions = new List<Function>()
                {
                    bernoullisEquation,
                    orificePlate
                }
            };
            FunctionCategory utility = new FunctionCategory()
            {
                Name = nameof(LibraryResources.Utility),
                Functions = new List<Function>()
                {
                    unitConverter, areaFunction
                }
            };
            modelBuilder.Entity<FunctionCategory>().HasData(
                fluidDynamics, utility
            );
            #endregion
        }
    }
}
