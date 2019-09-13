using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Model
{
    public class EngineeringMathSeedData
    {
        public Dictionary<string, FunctionDB> Functions { get; }
        public Dictionary<string, FunctionCategoryDB> FunctionCategories { get; }
        public Dictionary<string, OwnerDB> Owners { get; }
        public Dictionary<string, ParameterTypeDB> ParameterTypes { get; }
        public Dictionary<string, UnitCategoryDB> UnitCategories { get; }
        public Dictionary<string, UnitSystemDB> UnitSystems { get; }
        public EngineeringMathSeedData()
        {
            #region Owner
            OwnerDB system = new OwnerDB()
            {
                Name = "SYSTEM"
            };
            Owners = new Dictionary<string, OwnerDB>
            {
                { system.Name, system }
            };
            #endregion
            #region UnitSystem
            UnitSystemDB siUnits = new UnitSystemDB()
            {
                Abbreviation = nameof(LibraryResources.SIAbbrev),
                Name = nameof(LibraryResources.SIFullName),
                Owner = system
            };
            UnitSystemDB uscsUnits = new UnitSystemDB()
            {
                Abbreviation = nameof(LibraryResources.USCSAbbrev),
                Name = nameof(LibraryResources.USCSFullName),
                Owner = system
            };
            UnitSystemDB imperialUnits = new UnitSystemDB()
            {
                Abbreviation = nameof(LibraryResources.ImperialAbbrev),
                Name = nameof(LibraryResources.ImperialFullName),
                Owner = system,
                Children = new List<UnitSystemDB>()
                {
                    uscsUnits
                }
            };
            UnitSystemDB metricUnits = new UnitSystemDB()
            {
                Abbreviation = nameof(LibraryResources.MetricAbbrev),
                Name = nameof(LibraryResources.MetricFullName),
                Owner = system,
                Children = new List<UnitSystemDB>()
                {
                    siUnits
                }
            };
            var unitSystems = new List<UnitSystemDB> { siUnits, uscsUnits, imperialUnits, metricUnits };
            UnitSystems = unitSystems.ToDictionary(x => x.Name);
            #endregion
            #region UnitCategory


            UnitCategoryDB area = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 2",
                Name = nameof(LibraryResources.Area),
                Owner = system
            };

            UnitCategoryDB density = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Mass)} / ${nameof(LibraryResources.Volume)}",
                Name = nameof(LibraryResources.Density),
                Owner = system
            };
            UnitCategoryDB energy = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Energy),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.JoulesFullName),
                        Symbol = nameof(LibraryResources.JoulesAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.BTUFullName),
                        Symbol = nameof(LibraryResources.BTUAbbrev),
                        ConvertFromSi = "$0 / 1055.06",
                        ConvertToSi = "$0 * 1055.06",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilocaloriesFullName),
                        Symbol = nameof(LibraryResources.KilocaloriesAbbrev),
                        ConvertFromSi = "$0 / 4184",
                        ConvertToSi = "$0 * 4184",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilojoulesFullName),
                        Symbol = nameof(LibraryResources.KilojoulesAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.ThermsFullName),
                        Symbol = nameof(LibraryResources.ThermsAbbrev),
                        ConvertFromSi = "$0 / 1.055e+8",
                        ConvertToSi = "$0 * 1.055e+8",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    }
                },
            };
            UnitCategoryDB enthalpy = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.Enthalpy),
                Owner = system
            };
            UnitCategoryDB entropy = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / (${nameof(LibraryResources.Mass)} * ${nameof(LibraryResources.Temperature)})",
                Name = nameof(LibraryResources.Entropy),
                Owner = system
            };
            UnitCategoryDB isothermalCompressibility = new UnitCategoryDB()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Pressure)}",
                Name = nameof(LibraryResources.IsothermalCompressibility),
                Owner = system
            };
            UnitCategoryDB length = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Length),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MeterFullName),
                        Symbol = nameof(LibraryResources.MeterAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.FeetFullName),
                        Symbol = nameof(LibraryResources.FeetAbbrev),
                        ConvertFromSi = "$0 * 3.28084",
                        ConvertToSi = "$0 / 3.28084",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.InchesFullName),
                        Symbol = nameof(LibraryResources.InchesAbbrev),
                        ConvertFromSi = "$0 * 39.3701",
                        ConvertToSi = "$0 / 39.3701",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MilesFullName),
                        Symbol = nameof(LibraryResources.MilesAbbrev),
                        ConvertFromSi = "$0 / 1609.34",
                        ConvertToSi = "$0 * 1609.34",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MillimetersFullName),
                        Symbol = nameof(LibraryResources.MillimetersAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.CentimetersFullName),
                        Symbol = nameof(LibraryResources.CentimetersAbbrev),
                        ConvertFromSi = "$0 * 1e2",
                        ConvertToSi = "$0 / 1e2",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilometersFullName),
                        Symbol = nameof(LibraryResources.KilometersAbbrev),
                        ConvertFromSi = "$0 / 1e3",
                        ConvertToSi = "$0 * 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    }
                },
            };
            UnitCategoryDB mass = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Mass),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilogramsFullName),
                        Symbol = nameof(LibraryResources.KilogramsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.PoundsMassFullName),
                        Symbol = nameof(LibraryResources.PoundsMassAbbrev),
                        ConvertFromSi = "$0 * 2.20462",
                        ConvertToSi = "$0 / 2.20462",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.GramsFullName),
                        Symbol = nameof(LibraryResources.GramsAbbrev),
                        ConvertFromSi = "$0 * 1000",
                        ConvertToSi = "$0 / 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MilligramsFullName),
                        Symbol = nameof(LibraryResources.MilligramsAbbrev),
                        ConvertFromSi = "$0 * 1e6",
                        ConvertToSi = "$0 / 1e6",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MicrogramsFullName),
                        Symbol = nameof(LibraryResources.MicrogramsAbbrev),
                        ConvertFromSi = "$0 * 1e9",
                        ConvertToSi = "$0 / 1e9",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MetricTonsFullName),
                        Symbol = nameof(LibraryResources.MetricTonsAbbrev),
                        ConvertFromSi = "$0 / 1e3",
                        ConvertToSi = "$0 * 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.OuncesFullName),
                        Symbol = nameof(LibraryResources.OuncesAbbrev),
                        ConvertFromSi = "$0 * 35.274",
                        ConvertToSi = "$0 / 35.274",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.USTonsFullName),
                        Symbol = nameof(LibraryResources.USTonsAbbrev),
                        ConvertFromSi = "$0 / 907.185",
                        ConvertToSi = "$0 * 907.185",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                },
            };

            UnitCategoryDB power = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Power),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.WattsFullName),
                        Symbol = nameof(LibraryResources.WattsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.HorsepowerFullName),
                        Symbol = nameof(LibraryResources.HorsepowerAbbrev),
                        ConvertFromSi = "$0 / 745.7",
                        ConvertToSi = "$0 * 745.7",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilowattFullName),
                        Symbol = nameof(LibraryResources.KilowattAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },

            };

            UnitCategoryDB pressure = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Pressure),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.PascalsFullName),
                        Symbol = nameof(LibraryResources.PascalsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.PoundsForcePerSqInFullName),
                        Symbol = nameof(LibraryResources.PoundsForcePerSqInAbbrev),
                        ConvertFromSi = "$0 / 6894.76",
                        ConvertToSi = "$0 * 6894.76",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.AtmospheresFullName),
                        Symbol = nameof(LibraryResources.AtmospheresAbbrev),
                        ConvertFromSi = "$0 / 101325",
                        ConvertToSi = "$0 * 101325",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.BarFullName),
                        Symbol = nameof(LibraryResources.BarAbbrev),
                        ConvertFromSi = "$0 / 1e5",
                        ConvertToSi = "$0 * 1e5",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KilopascalsFullName),
                        Symbol = nameof(LibraryResources.KilopascalsAbbrev),
                        ConvertFromSi = "$0 / 1000",
                        ConvertToSi = "$0 * 1000",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.TorrFullName),
                        Symbol = nameof(LibraryResources.TorrAbbrev),
                        ConvertFromSi = "$0 / 133.322",
                        ConvertToSi = "$0 * 133.322",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },
            };
            UnitCategoryDB specificVolume = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.SpecificVolume),
                Owner = system
            };

            UnitCategoryDB temperature = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Temperature),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.KelvinFullName),
                        Symbol = nameof(LibraryResources.KelvinAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.RankineFullName),
                        Symbol = nameof(LibraryResources.RankineAbbrev),
                        ConvertFromSi = "$0 * 1.8",
                        ConvertToSi = "$0 / 1.8",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.FahrenheitFullName),
                        Symbol = nameof(LibraryResources.FahrenheitAbbrev),
                        ConvertFromSi = "($0 - 273.15) * 9 / 5 + 32",
                        ConvertToSi = "($0 - 32) * 5 / 9 + 273.15",
                        IsOnAbsoluteScale = false,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.CelsiusFullName),
                        Symbol = nameof(LibraryResources.CelsiusAbbrev),
                        ConvertFromSi = "$0 - 273.15",
                        ConvertToSi = "$0 + 273.15",
                        IsOnAbsoluteScale = false,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },

            };

            UnitCategoryDB time = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Time),
                Owner = system,
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MinutesFullName),
                        Symbol = nameof(LibraryResources.MinutesAbbrev),
                        ConvertFromSi = "$0 / 60",
                        ConvertToSi = "$0 * 60",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.HoursFullName),
                        Symbol = nameof(LibraryResources.HoursAbbrev),
                        ConvertFromSi = "$0 / 3600",
                        ConvertToSi = "$0 * 3600",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MillisecondsFullName),
                        Symbol = nameof(LibraryResources.MillisecondsAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.DaysFullName),
                        Symbol = nameof(LibraryResources.DaysAbbrev),
                        ConvertFromSi = "$0 / (3600 * 24)",
                        ConvertToSi = "$0 * 3600 * 24",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits,
                            imperialUnits
                        },
                        Owner = system
                    },
                },

            };

            UnitCategoryDB velocity = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.Velocity),
                Owner = system
            };


            UnitCategoryDB volume = new UnitCategoryDB()
            {
                Name = nameof(LibraryResources.Volume),
                Owner = system,
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 3",
                Units = new List<UnitDB>()
                {
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.GallonsFullName),
                        Symbol = nameof(LibraryResources.GallonsAbbrev),
                        ConvertFromSi = "$0 * 264.172",
                        ConvertToSi = "$0 / 264.172",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            imperialUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.LitersFullName),
                        Symbol = nameof(LibraryResources.LitersAbbrev),
                        ConvertFromSi = "$0 * 1e3",
                        ConvertToSi = "$0 / 1e3",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                    new UnitDB()
                    {
                        Name = nameof(LibraryResources.MillilitersFullName),
                        Symbol = nameof(LibraryResources.MillilitersAbbrev),
                        ConvertFromSi = "$0 * 1e6",
                        ConvertToSi = "$0 / 1e6",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystemDB>()
                        {
                            metricUnits
                        },
                        Owner = system
                    },
                },

            };
            UnitCategoryDB volumeExpansivity = new UnitCategoryDB()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Temperature)}",
                Name = nameof(LibraryResources.VolumeExpansivity),
                Owner = system
            };
            UnitCategoryDB volumetricFlowRate = new UnitCategoryDB()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.VolumetricFlowRate),
                Owner = system
            };
            var unitCaegories = new List<UnitCategoryDB>
                {
                    area, density, energy,
                    enthalpy, entropy, isothermalCompressibility,
                    length, mass, power, pressure, specificVolume,
                    temperature, time, velocity, volume, volumeExpansivity,
                    volumetricFlowRate
                };
            UnitCategories = unitCaegories.ToDictionary(x => x.Name);
            #endregion
            #region ParameterType
            ParameterTypeDB integerType = new ParameterTypeDB()
            {
                Name = nameof(Int32),
                Owner = system
            };
            ParameterTypeDB doubleType = new ParameterTypeDB()
            {
                Name = nameof(Double),
                Owner = system,
            };
            ParameterTypeDB unitCategoryType = new ParameterTypeDB()
            {
                Name = nameof(UnitCategoryDB),
                Owner = system,
            };
            var parameterType = new List<ParameterTypeDB>
                {
                    integerType, doubleType, unitCategoryType
                };
            ParameterTypes = parameterType.ToDictionary(x => x.Name);
            #endregion
            #region Function
            FunctionDB areaFunction = new FunctionDB()
            {
                Name = nameof(LibraryResources.Area),
                Owner = system,
                Parameters = new List<ParameterDB>()
                {
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Diameter),
                        Owner = system,
                        ValueConditions = "$0 >= 0",
                        ParameterType = doubleType,
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Area),
                        Owner = system,
                        ValueConditions = "$0 >= 0",
                        ParameterType = doubleType,
                    }
                },
                Equations = new List<EquationDB>()
                {
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.Area),
                        Formula = $"{nameof(LibraryResources.Diameter)} ^ 2 * PI() / 4",
                        Owner = system
                    }
                },
            };
            FunctionOutputValueLinkDB areaFunctionLink = new FunctionOutputValueLinkDB()
            {
                Function = areaFunction,
                OutputParameterName = nameof(LibraryResources.Area)
            };
            FunctionDB unitConverter = new FunctionDB()
            {
                Name = nameof(LibraryResources.UnitConverter),
                Owner = system,
                Equations = new List<EquationDB>()
                {
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.Output),
                        Formula = $"${nameof(LibraryResources.Input)}",
                        Owner = system
                    }
                },
                Parameters = new List<ParameterDB>()
                {
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.UnitType),
                        Owner = system,
                        ParameterType = unitCategoryType,
                        ValueLinks = new List<ParameterValueLinkDB>()
                        {
                            new ParameterValueLinkDB()
                            {
                                ParameterName = nameof(LibraryResources.Input)
                            },
                            new ParameterValueLinkDB()
                            {
                                ParameterName = nameof(LibraryResources.Output)
                            }
                        }
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Input),
                        Owner = system,
                        ParameterType = doubleType,
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Output),
                        Owner = system,
                        ParameterType = doubleType,
                    }
                }
            };
            // https://github.com/jfkonecn/OpenChE/blob/Better_UI/Backend/EngineeringMath/Component/DefaultFunctions/BernoullisEquation.cs
            FunctionDB bernoullisEquation = new FunctionDB()
            {
                Name = nameof(LibraryResources.BernoullisEquation),
                Owner = system,
                Parameters = new List<ParameterDB>()
                {
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.InletVelocity),
                        ParameterType = doubleType,
                        UnitCategory = velocity,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.OutletVelocity),
                        ParameterType = doubleType,
                        UnitCategory = velocity,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.InletPressure),
                        ParameterType = doubleType,
                        UnitCategory = pressure,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.OutletPressure),
                        ParameterType = doubleType,
                        UnitCategory = pressure,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.InletHeight),
                        ParameterType = doubleType,
                        UnitCategory = length,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.OutletHeight),
                        ParameterType = doubleType,
                        UnitCategory = length,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Density),
                        ParameterType = doubleType,
                        UnitCategory = density,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    }
                },
                Equations = new List<EquationDB>()
                {
                    new EquationDB()
                    {
                        Formula = $@"Sqrt(2 * (${LibraryResources.OutletVelocity} ^ 2 / 2 
                            + 9.81 * (${LibraryResources.OutletHeight} - ${LibraryResources.InletHeight} ) 
                            + (${LibraryResources.OutletPressure} - ${LibraryResources.InletPressure} ) 
                                / ${LibraryResources.Density} ))",
                        OutputName = LibraryResources.InletVelocity,
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"Sqrt(2 * (${nameof(LibraryResources.InletVelocity)} ^ 2 / 2 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) 
                                / ${nameof(LibraryResources.Density)}))",
                        OutputName = nameof(LibraryResources.OutletVelocity),
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.OutletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.OutletHeight)} - ${nameof(LibraryResources.InletHeight)}) 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.InletPressure),
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.InletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.OutletPressure),
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"((${nameof(LibraryResources.OutletPressure)} - ${nameof(LibraryResources.InletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.OutletHeight)}",
                        OutputName = nameof(LibraryResources.InletHeight),
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"((${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.InletHeight)}",
                        OutputName = nameof(LibraryResources.OutletHeight),
                        Owner = system
                    },
                    new EquationDB()
                    {
                        Formula = $@"($pout - $pin) / (0.5 * ($uin ^ 2 - $uout ^ 2) 
                            + 9.81 * ($hin - ${nameof(LibraryResources.OutletHeight)}))",
                        OutputName = nameof(LibraryResources.Density),
                        Owner = system
                    }
                }
            };
            FunctionDB orificePlate = new FunctionDB()
            {
                Name = nameof(LibraryResources.OrificePlate),
                Owner = system,
                Parameters = new List<ParameterDB>()
                {
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.DischargeCoefficient),
                        ParameterType = integerType,
                        UnitCategory = null,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.Density),
                        ParameterType = doubleType,
                        UnitCategory = density,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.InletPipeArea),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        FunctionLinks = new List<FunctionOutputValueLinkDB>()
                        {
                            areaFunctionLink
                        },
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.OrificeArea),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        FunctionLinks = new List<FunctionOutputValueLinkDB>()
                        {
                            areaFunctionLink
                        },
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.PressureDrop),
                        ParameterType = doubleType,
                        UnitCategory = area,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                    new ParameterDB()
                    {
                        Name = nameof(LibraryResources.VolumetricFlowRate),
                        ParameterType = doubleType,
                        UnitCategory = volumetricFlowRate,
                        ValueConditions = "$0 >= 0",
                        Owner = system
                    },
                },
                Equations = new List<EquationDB>()
                {
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.DischargeCoefficient),
                        Formula = $@"${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.InletPipeArea)} * 
                            Sqrt((2 * ${nameof(LibraryResources.PressureDrop)}) / (${nameof(LibraryResources.Density)} * 
                                (${nameof(LibraryResources.InletPipeArea)} ^ 2 / ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))))",
                        Owner = system
                    },
                    new EquationDB()
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
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.InletPipeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.OrificeArea)} ^ 2) - 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        Owner = system
                    },
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.OrificeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.InletPipeArea)} ^ 2) + 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / 
                                (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        Owner = system
                    },
                    new EquationDB()
                    {
                        OutputName = nameof(LibraryResources.PressureDrop),
                        Formula = $@"((${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.DischargeCoefficient)} * 
                            ${nameof(LibraryResources.InletPipeArea)})) ^ 2 * 
                            (${nameof(LibraryResources.Density)} * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / 
                                ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))) / 2",
                        Owner = system
                    },
                    new EquationDB()
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
            var functions = new List<FunctionDB>
                {
                    orificePlate, bernoullisEquation, unitConverter, areaFunction
                };
            Functions = functions.ToDictionary(x => x.Name);
            #endregion
            #region FuncationCategories
            FunctionCategoryDB fluidDynamics = new FunctionCategoryDB()
            {
                Name = nameof(LibraryResources.FluidDynamics),
                Functions = new List<FunctionDB>()
                {
                    bernoullisEquation,
                    orificePlate
                }
            };
            FunctionCategoryDB utility = new FunctionCategoryDB()
            {
                Name = nameof(LibraryResources.Utility),
                Functions = new List<FunctionDB>()
                {
                    unitConverter, areaFunction
                }
            };
            var functionCategories = new List<FunctionCategoryDB>
                {
                    fluidDynamics, utility
                };
            FunctionCategories = functionCategories.ToDictionary(x => x.Name);
            #endregion
        }
    }
}
