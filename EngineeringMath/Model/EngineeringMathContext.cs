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
        public DbSet<ParameterOption> ParameterOptions { get; set; }
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
                Children =
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
                CompositeEquation = $"${nameof(LibraryResources.Mass)} / ${nameof(LibraryResources.Length)} ^ 3",
                Name = nameof(LibraryResources.Density),
                Owner = system
            };
            UnitCategory energy = new UnitCategory()
            {
                Name = nameof(LibraryResources.Energy),
                Owner = system,
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.JoulesFullName),
                        Symbol = nameof(LibraryResources.JoulesAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MeterFullName),
                        Symbol = nameof(LibraryResources.MetricAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KilogramsFullName),
                        Symbol = nameof(LibraryResources.KilogramsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.WattsFullName),
                        Symbol = nameof(LibraryResources.WattsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.PascalsFullName),
                        Symbol = nameof(LibraryResources.PascalsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KelvinFullName),
                        Symbol = nameof(LibraryResources.KelvinAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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
                Units =
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.GallonsFullName),
                        Symbol = nameof(LibraryResources.GallonsAbbrev),
                        ConvertFromSi = "$0 * 264.172",
                        ConvertToSi = "$0 / 264.172",
                        IsOnAbsoluteScale = true,
                        UnitSystems =
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
                        UnitSystems =
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
                        UnitSystems =
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

            Function unitConverter = new Function()
            {
                Name = nameof(LibraryResources.UnitConverter),
                Owner = system,
                Equations =
                {
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.Output),
                        Formula = $"{nameof(LibraryResources.Input)}",
                        Owner = system
                    }
                },
                Parameters =
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
                Parameters =
                {
                    new Parameter()
                    {
                        Name = nameof(LibraryResources.InletVelocity),
                        ParameterType = doubleType,
                        UnitCategory = velocity,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    }
                },
                Equations =
                {
                    new Equation()
                    {

                    }
                }
            };

            modelBuilder.Entity<Function>().HasData(
                bernoullisEquation, unitConverter
                );
            #endregion
            #region FuncationCategories
            FunctionCategory fluidDynamics = new FunctionCategory()
            {
                Name = nameof(LibraryResources.FluidDynamics),
                Functions =
                {
                    bernoullisEquation
                }
            };
            modelBuilder.Entity<FunctionCategory>().HasData(
                fluidDynamics
            );
            #endregion
        }
    }
}
