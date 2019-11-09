using EngineeringMath.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public EngineeringMathContext(DbContextOptions options) : base(options)
        {
        }

        protected EngineeringMathContext() : base()
        {
        }

        public DbSet<Equation> Equations { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<FunctionCategory> FunctionCategories { get; set; }
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
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=engineeringMath.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UnitSystem>()
                .HasMany(x => x.Children)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<UnitSystemUnit>()
            .HasKey(x => new { x.UnitId, x.UnitSystemId });

            modelBuilder.Entity<UnitSystemUnit>()
                .HasOne(x => x.Unit)
                .WithMany(x => x.UnitSystemUnits)
                .HasForeignKey(x => x.UnitId);

            modelBuilder.Entity<UnitSystemUnit>()
                .HasOne(x => x.UnitSystem)
                .WithMany(x => x.UnitSystemUnits)
                .HasForeignKey(x => x.UnitSystemId);

            modelBuilder.Entity<FunctionCategoryFunction>()
                .HasKey(x => new { x.FunctionCategoryId, x.FunctionId });

            modelBuilder.Entity<FunctionCategoryFunction>()
                .HasOne(x => x.Function)
                .WithMany(x => x.FunctionCategoryFunctions)
                .HasForeignKey(x => x.FunctionId);

            modelBuilder.Entity<FunctionCategoryFunction>()
                .HasOne(x => x.FunctionCategory)
                .WithMany(x => x.FunctionCategoryFunctions)
                .HasForeignKey(x => x.FunctionCategoryId);

            AddSeedData(modelBuilder);
        }



        private void AddToBuilder<T>(ModelBuilder modelBuilder,IEnumerable<T> objs, Action<T, int> setPrimaryKey, int startingIndex = 1) where T : class
        {
            int index = startingIndex;
            foreach (T obj in objs)
            {
                setPrimaryKey(obj, index++);
            }
            modelBuilder.Entity<T>().HasData(objs);
        }

        private void AddSeedData(ModelBuilder modelBuilder)
        {
            #region Owner
            Owner system = new Owner()
            {
                OwnerId = 1,
                Name = "SYSTEM"
            };
            modelBuilder.Entity<Owner>().HasData(new List<Owner>() { system });

            #endregion
            #region UnitSystem
            UnitSystem siUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.SIAbbrev),
                Name = nameof(LibraryResources.SIFullName),
                ParentId = 3,
                OwnerId = system.OwnerId,
            };
            UnitSystem uscsUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.USCSAbbrev),
                Name = nameof(LibraryResources.USCSFullName),
                ParentId = 4,
                OwnerId = system.OwnerId,
            };
            UnitSystem metricUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.MetricAbbrev),
                Name = nameof(LibraryResources.MetricFullName),
                OwnerId = system.OwnerId,
            };
            UnitSystem imperialUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.ImperialAbbrev),
                Name = nameof(LibraryResources.ImperialFullName),
                OwnerId = system.OwnerId,
            };
            var unitSystems = new List<UnitSystem> { siUnits, uscsUnits,  metricUnits, imperialUnits, };
            AddToBuilder(modelBuilder, unitSystems, (obj, idx) => obj.UnitSystemId = idx);
            #endregion
            #region UnitCategory


            UnitCategory area = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 2",
                Name = nameof(LibraryResources.Area),
                OwnerId = system.OwnerId,
            };

            UnitCategory density = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Mass)} / ${nameof(LibraryResources.Volume)}",
                Name = nameof(LibraryResources.Density),
                OwnerId = system.OwnerId,
            };
            UnitCategory energy = new UnitCategory()
            {
                Name = nameof(LibraryResources.Energy),
                OwnerId = system.OwnerId,
            };
            UnitCategory enthalpy = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.Enthalpy),
                OwnerId = system.OwnerId,
            };
            UnitCategory entropy = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Energy)} / (${nameof(LibraryResources.Mass)} * ${nameof(LibraryResources.Temperature)})",
                Name = nameof(LibraryResources.Entropy),
                OwnerId = system.OwnerId,
            };
            UnitCategory isothermalCompressibility = new UnitCategory()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Pressure)}",
                Name = nameof(LibraryResources.IsothermalCompressibility),
                OwnerId = system.OwnerId,
            };
            UnitCategory length = new UnitCategory()
            {
                Name = nameof(LibraryResources.Length),
                OwnerId = system.OwnerId,
            };
            UnitCategory mass = new UnitCategory()
            {
                Name = nameof(LibraryResources.Mass),
                OwnerId = system.OwnerId,
            };

            UnitCategory power = new UnitCategory()
            {
                Name = nameof(LibraryResources.Power),
                OwnerId = system.OwnerId,
            };

            UnitCategory pressure = new UnitCategory()
            {
                Name = nameof(LibraryResources.Pressure),
                OwnerId = system.OwnerId,
            };
            UnitCategory specificVolume = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Mass)}",
                Name = nameof(LibraryResources.SpecificVolume),
                OwnerId = system.OwnerId,
            };

            UnitCategory temperature = new UnitCategory()
            {
                Name = nameof(LibraryResources.Temperature),
                OwnerId = system.OwnerId,
            };

            UnitCategory time = new UnitCategory()
            {
                Name = nameof(LibraryResources.Time),
                OwnerId = system.OwnerId,
            };

            UnitCategory velocity = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Length)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.Velocity),
                OwnerId = system.OwnerId,
            };


            UnitCategory volume = new UnitCategory()
            {
                Name = nameof(LibraryResources.Volume),
                OwnerId = system.OwnerId,
                CompositeEquation = $"${nameof(LibraryResources.Length)} ^ 3",
            };
            UnitCategory volumeExpansivity = new UnitCategory()
            {
                CompositeEquation = $"1 / ${nameof(LibraryResources.Temperature)}",
                Name = nameof(LibraryResources.VolumeExpansivity),
                OwnerId = system.OwnerId,
            };
            UnitCategory volumetricFlowRate = new UnitCategory()
            {
                CompositeEquation = $"${nameof(LibraryResources.Volume)} / ${nameof(LibraryResources.Time)}",
                Name = nameof(LibraryResources.VolumetricFlowRate),
                OwnerId = system.OwnerId,
            };
            var unitCategories = new List<UnitCategory>
                {
                    area, density, energy,
                    enthalpy, entropy, isothermalCompressibility,
                    length, mass, power, pressure, specificVolume,
                    temperature, time, velocity, volume, volumeExpansivity,
                    volumetricFlowRate
                };
            AddToBuilder(modelBuilder, unitCategories, (obj, idx) => obj.UnitCategoryId = idx);
            #endregion
            #region Units
            var siUnitCollection = new List<Unit>();
            var uscsUnitCollection = new List<Unit>();
            var metricUnitCollection = new List<Unit>();
            var imperialUnitCollection = new List<Unit>();
            var energyUnitCollection = new List<Unit>();

            var joules = new Unit()
            {
                Name = nameof(LibraryResources.JoulesFullName),
                Symbol = nameof(LibraryResources.JoulesAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(joules);
            energyUnitCollection.Add(joules);
            
            Unit btus = new Unit()
            {
                Name = nameof(LibraryResources.BTUFullName),
                Symbol = nameof(LibraryResources.BTUAbbrev),
                ConvertFromSi = "$0 / 1055.06",
                ConvertToSi = "$0 * 1055.06",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(btus);
            energyUnitCollection.Add(btus);
            
            Unit kiloCalories = new Unit()
            {
                Name = nameof(LibraryResources.KilocaloriesFullName),
                Symbol = nameof(LibraryResources.KilocaloriesAbbrev),
                ConvertFromSi = "$0 / 4184",
                ConvertToSi = "$0 * 4184",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(kiloCalories);
            energyUnitCollection.Add(kiloCalories);

            Unit kilojoules = new Unit()
            {
                Name = nameof(LibraryResources.KilojoulesFullName),
                Symbol = nameof(LibraryResources.KilojoulesAbbrev),
                ConvertFromSi = "$0 / 1000",
                ConvertToSi = "$0 * 1000",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(kilojoules);
            energyUnitCollection.Add(kilojoules);

            Unit therms = new Unit()
            {
                Name = nameof(LibraryResources.ThermsFullName),
                Symbol = nameof(LibraryResources.ThermsAbbrev),
                ConvertFromSi = "$0 / 1.055e+8",
                ConvertToSi = "$0 * 1.055e+8",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(therms);
            energyUnitCollection.Add(therms);

            var lengthUnitCollection = new List<Unit>();

            Unit meter = new Unit()
            {
                Name = nameof(LibraryResources.MeterFullName),
                Symbol = nameof(LibraryResources.MeterAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(meter);
            lengthUnitCollection.Add(meter);

            Unit feet = new Unit()
            {
                Name = nameof(LibraryResources.FeetFullName),
                Symbol = nameof(LibraryResources.FeetAbbrev),
                ConvertFromSi = "$0 * 3.28084",
                ConvertToSi = "$0 / 3.28084",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(feet);
            lengthUnitCollection.Add(feet);

            Unit inches = new Unit()
            {
                Name = nameof(LibraryResources.InchesFullName),
                Symbol = nameof(LibraryResources.InchesAbbrev),
                ConvertFromSi = "$0 * 39.3701",
                ConvertToSi = "$0 / 39.3701",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(inches);
            lengthUnitCollection.Add(inches);

            Unit miles = new Unit()
            {
                Name = nameof(LibraryResources.MilesFullName),
                Symbol = nameof(LibraryResources.MilesAbbrev),
                ConvertFromSi = "$0 / 1609.34",
                ConvertToSi = "$0 * 1609.34",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(miles);
            lengthUnitCollection.Add(miles);

            Unit millimeters = new Unit()
            {
                Name = nameof(LibraryResources.MillimetersFullName),
                Symbol = nameof(LibraryResources.MillimetersAbbrev),
                ConvertFromSi = "$0 * 1e3",
                ConvertToSi = "$0 / 1e3",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(millimeters);
            lengthUnitCollection.Add(millimeters);

            Unit centimeters = new Unit()
            {
                Name = nameof(LibraryResources.CentimetersFullName),
                Symbol = nameof(LibraryResources.CentimetersAbbrev),
                ConvertFromSi = "$0 * 1e2",
                ConvertToSi = "$0 / 1e2",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(centimeters);
            lengthUnitCollection.Add(centimeters);

            Unit kilometers = new Unit()
            {
                Name = nameof(LibraryResources.KilometersFullName),
                Symbol = nameof(LibraryResources.KilometersAbbrev),
                ConvertFromSi = "$0 / 1e3",
                ConvertToSi = "$0 * 1e3",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(kilometers);
            lengthUnitCollection.Add(kilometers);
            lengthUnitCollection.ForEach(x => x.UnitCategoryId = length.UnitCategoryId);

            List<Unit> massUnitCollection = new List<Unit>();

            Unit kilogram = new Unit()
            {
                Name = nameof(LibraryResources.KilogramsFullName),
                Symbol = nameof(LibraryResources.KilogramsAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(kilogram);
            massUnitCollection.Add(kilogram);

            Unit poundsMass = new Unit()
            {
                Name = nameof(LibraryResources.PoundsMassFullName),
                Symbol = nameof(LibraryResources.PoundsMassAbbrev),
                ConvertFromSi = "$0 * 2.20462",
                ConvertToSi = "$0 / 2.20462",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(poundsMass);
            massUnitCollection.Add(poundsMass);

            Unit grams = new Unit()
            {
                Name = nameof(LibraryResources.GramsFullName),
                Symbol = nameof(LibraryResources.GramsAbbrev),
                ConvertFromSi = "$0 * 1000",
                ConvertToSi = "$0 / 1000",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(grams);
            massUnitCollection.Add(grams);

            Unit milligrams = new Unit()
            {
                Name = nameof(LibraryResources.MilligramsFullName),
                Symbol = nameof(LibraryResources.MilligramsAbbrev),
                ConvertFromSi = "$0 * 1e6",
                ConvertToSi = "$0 / 1e6",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(milligrams);
            massUnitCollection.Add(milligrams);

            Unit micrograms = new Unit()
            {
                Name = nameof(LibraryResources.MicrogramsFullName),
                Symbol = nameof(LibraryResources.MicrogramsAbbrev),
                ConvertFromSi = "$0 * 1e9",
                ConvertToSi = "$0 / 1e9",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(micrograms);
            massUnitCollection.Add(micrograms);

            Unit metricTons = new Unit()
            {
                Name = nameof(LibraryResources.MetricTonsFullName),
                Symbol = nameof(LibraryResources.MetricTonsAbbrev),
                ConvertFromSi = "$0 / 1e3",
                ConvertToSi = "$0 * 1e3",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(metricTons);
            massUnitCollection.Add(metricTons);

            Unit ounces = new Unit()
            {
                Name = nameof(LibraryResources.OuncesFullName),
                Symbol = nameof(LibraryResources.OuncesAbbrev),
                ConvertFromSi = "$0 * 35.274",
                ConvertToSi = "$0 / 35.274",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(ounces);
            massUnitCollection.Add(ounces);

            Unit usTons = new Unit()
            {
                Name = nameof(LibraryResources.USTonsFullName),
                Symbol = nameof(LibraryResources.USTonsAbbrev),
                ConvertFromSi = "$0 / 907.185",
                ConvertToSi = "$0 * 907.185",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(usTons);
            massUnitCollection.Add(usTons);

            massUnitCollection.ForEach(x => x.UnitCategoryId = mass.UnitCategoryId);

            List<Unit> powerUnitCollection = new List<Unit>();

            Unit watts = new Unit()
            {
                Name = nameof(LibraryResources.WattsFullName),
                Symbol = nameof(LibraryResources.WattsAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(watts);
            powerUnitCollection.Add(watts);

            Unit horsepower = new Unit()
            {
                Name = nameof(LibraryResources.HorsepowerFullName),
                Symbol = nameof(LibraryResources.HorsepowerAbbrev),
                ConvertFromSi = "$0 / 745.7",
                ConvertToSi = "$0 * 745.7",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(horsepower);
            powerUnitCollection.Add(horsepower);

            Unit kilowatt = new Unit()
            {
                Name = nameof(LibraryResources.KilowattFullName),
                Symbol = nameof(LibraryResources.KilowattAbbrev),
                ConvertFromSi = "$0 / 1000",
                ConvertToSi = "$0 * 1000",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(kilowatt);
            powerUnitCollection.Add(kilowatt);

            powerUnitCollection.ForEach(x => x.UnitCategoryId = power.UnitCategoryId);

            List<Unit> pressureUnitCollection = new List<Unit>();

            Unit pascals = new Unit()
            {
                Name = nameof(LibraryResources.PascalsFullName),
                Symbol = nameof(LibraryResources.PascalsAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(pascals);
            pressureUnitCollection.Add(pascals);

            Unit psi = new Unit()
            {
                Name = nameof(LibraryResources.PoundsForcePerSqInFullName),
                Symbol = nameof(LibraryResources.PoundsForcePerSqInAbbrev),
                ConvertFromSi = "$0 / 6894.76",
                ConvertToSi = "$0 * 6894.76",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(psi);
            pressureUnitCollection.Add(psi);

            Unit atms = new Unit()
            {
                Name = nameof(LibraryResources.AtmospheresFullName),
                Symbol = nameof(LibraryResources.AtmospheresAbbrev),
                ConvertFromSi = "$0 / 101325",
                ConvertToSi = "$0 * 101325",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(atms);
            pressureUnitCollection.Add(atms);

            Unit bar = new Unit()
            {
                Name = nameof(LibraryResources.BarFullName),
                Symbol = nameof(LibraryResources.BarAbbrev),
                ConvertFromSi = "$0 / 1e5",
                ConvertToSi = "$0 * 1e5",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(bar);
            pressureUnitCollection.Add(bar);

            Unit kilopascals = new Unit()
            {
                Name = nameof(LibraryResources.KilopascalsFullName),
                Symbol = nameof(LibraryResources.KilopascalsAbbrev),
                ConvertFromSi = "$0 / 1000",
                ConvertToSi = "$0 * 1000",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(kilopascals);
            pressureUnitCollection.Add(kilopascals);

            Unit torr = new Unit()
            {
                Name = nameof(LibraryResources.TorrFullName),
                Symbol = nameof(LibraryResources.TorrAbbrev),
                ConvertFromSi = "$0 / 133.322",
                ConvertToSi = "$0 * 133.322",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(torr);
            pressureUnitCollection.Add(torr);

            pressureUnitCollection.ForEach(x => x.UnitCategoryId = pressure.UnitCategoryId);

            var temperatureUnitCollection = new List<Unit>();
            Unit kelvin = new Unit()
            {
                Name = nameof(LibraryResources.KelvinFullName),
                Symbol = nameof(LibraryResources.KelvinAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(kelvin);
            temperatureUnitCollection.Add(kelvin);

            Unit rankine = new Unit()
            {
                Name = nameof(LibraryResources.RankineFullName),
                Symbol = nameof(LibraryResources.RankineAbbrev),
                ConvertFromSi = "$0 * 1.8",
                ConvertToSi = "$0 / 1.8",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            uscsUnitCollection.Add(rankine);
            temperatureUnitCollection.Add(rankine);

            Unit fahrenheit = new Unit()
            {
                Name = nameof(LibraryResources.FahrenheitFullName),
                Symbol = nameof(LibraryResources.FahrenheitAbbrev),
                ConvertFromSi = "($0 - 273.15) * 9 / 5 + 32",
                ConvertToSi = "($0 - 32) * 5 / 9 + 273.15",
                IsOnAbsoluteScale = false,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(fahrenheit);
            temperatureUnitCollection.Add(fahrenheit);

            Unit celsius = new Unit()
            {
                Name = nameof(LibraryResources.CelsiusFullName),
                Symbol = nameof(LibraryResources.CelsiusAbbrev),
                ConvertFromSi = "$0 - 273.15",
                ConvertToSi = "$0 + 273.15",
                IsOnAbsoluteScale = false,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(celsius);
            temperatureUnitCollection.Add(celsius);

            temperatureUnitCollection.ForEach(x => x.UnitCategoryId = temperature.UnitCategoryId);

            List<Unit> timeUnitCollection = new List<Unit>();
            Unit seconds = new Unit()
            {
                Name = nameof(LibraryResources.SecondsFullName),
                Symbol = nameof(LibraryResources.SecondsAbbrev),
                ConvertFromSi = "$0",
                ConvertToSi = "$0",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            siUnitCollection.Add(seconds);
            uscsUnitCollection.Add(seconds);
            timeUnitCollection.Add(seconds);

            Unit minutes = new Unit()
            {
                Name = nameof(LibraryResources.MinutesFullName),
                Symbol = nameof(LibraryResources.MinutesAbbrev),
                ConvertFromSi = "$0 / 60",
                ConvertToSi = "$0 * 60",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(minutes);
            imperialUnitCollection.Add(minutes);
            timeUnitCollection.Add(minutes);

            Unit hours = new Unit()
            {
                Name = nameof(LibraryResources.HoursFullName),
                Symbol = nameof(LibraryResources.HoursAbbrev),
                ConvertFromSi = "$0 / 3600",
                ConvertToSi = "$0 * 3600",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(hours);
            imperialUnitCollection.Add(hours);
            timeUnitCollection.Add(hours);

            Unit milliseconds = new Unit()
            {
                Name = nameof(LibraryResources.MillisecondsFullName),
                Symbol = nameof(LibraryResources.MillisecondsAbbrev),
                ConvertFromSi = "$0 * 1e3",
                ConvertToSi = "$0 / 1e3",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(milliseconds);
            imperialUnitCollection.Add(milliseconds);
            timeUnitCollection.Add(milliseconds);

            Unit days = new Unit()
            {
                Name = nameof(LibraryResources.DaysFullName),
                Symbol = nameof(LibraryResources.DaysAbbrev),
                ConvertFromSi = "$0 / (3600 * 24)",
                ConvertToSi = "$0 * 3600 * 24",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(days);
            imperialUnitCollection.Add(days);
            timeUnitCollection.Add(days);

            timeUnitCollection.ForEach(x => x.UnitCategoryId = time.UnitCategoryId);

            List<Unit> volumnUnitCollection = new List<Unit>();

            Unit gallons = new Unit()
            {
                Name = nameof(LibraryResources.GallonsFullName),
                Symbol = nameof(LibraryResources.GallonsAbbrev),
                ConvertFromSi = "$0 * 264.172",
                ConvertToSi = "$0 / 264.172",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            imperialUnitCollection.Add(gallons);
            volumnUnitCollection.Add(gallons);

            Unit liters = new Unit()
            {
                Name = nameof(LibraryResources.LitersFullName),
                Symbol = nameof(LibraryResources.LitersAbbrev),
                ConvertFromSi = "$0 * 1e3",
                ConvertToSi = "$0 / 1e3",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(liters);
            volumnUnitCollection.Add(liters);

            Unit milliliters = new Unit()
            {
                Name = nameof(LibraryResources.MillilitersFullName),
                Symbol = nameof(LibraryResources.MillilitersAbbrev),
                ConvertFromSi = "$0 * 1e6",
                ConvertToSi = "$0 / 1e6",
                IsOnAbsoluteScale = true,
                OwnerId = system.OwnerId,
            };
            metricUnitCollection.Add(milliliters);
            volumnUnitCollection.Add(milliliters);

            volumnUnitCollection.ForEach(x => x.UnitCategoryId = volume.UnitCategoryId);

            IEnumerable<Unit> allUnits = siUnitCollection
                .Union(uscsUnitCollection)
                .Union(metricUnitCollection)
                .Union(imperialUnitCollection);
            AddToBuilder(modelBuilder, allUnits, (x, idx) => x.UnitId = idx);
            #endregion
            #region UnitCategoryUnit
            IEnumerable<UnitSystemUnit> allUnitSystemUnit = siUnitCollection.Select(x => new UnitSystemUnit()
            {
                UnitId = x.UnitId,
                UnitSystemId = siUnits.UnitSystemId
            })
            .Union(uscsUnitCollection.Select(x => new UnitSystemUnit()
            {
                UnitId = x.UnitId,
                UnitSystemId = uscsUnits.UnitSystemId
            }))
            .Union(metricUnitCollection.Select(x => new UnitSystemUnit()
            {
                UnitId = x.UnitId,
                UnitSystemId = metricUnits.UnitSystemId
            }))
            .Union(imperialUnitCollection.Select(x => new UnitSystemUnit()
            {
                UnitId = x.UnitId,
                UnitSystemId = imperialUnits.UnitSystemId
            }))
            .GroupBy(x => new { x.UnitId, x.UnitSystemId })
            .Select(x => new UnitSystemUnit()
            {
                UnitId = x.Key.UnitId,
                UnitSystemId = x.Key.UnitSystemId
            });
            modelBuilder.Entity<UnitSystemUnit>().HasData(allUnitSystemUnit);
            #endregion
            #region ParameterType
            ParameterType integerType = new ParameterType()
            {
                Name = nameof(Int32),
                OwnerId = system.OwnerId,
            };
            ParameterType doubleType = new ParameterType()
            {
                Name = nameof(Double),
                OwnerId = system.OwnerId,
            };
            ParameterType unitCategoryType = new ParameterType()
            {
                Name = nameof(UnitCategory),
                OwnerId = system.OwnerId,
            };
            var parameterType = new List<ParameterType>
                {
                    integerType, doubleType, unitCategoryType
                };
            AddToBuilder(modelBuilder, parameterType, (obj, idx) => obj.ParameterTypeId = idx);
            #endregion
            #region Function
            Function areaFunction = new Function()
            {
                Name = nameof(LibraryResources.Area),
                OwnerId = system.OwnerId,
            };

            Function unitConverter = new Function()
            {
                Name = nameof(LibraryResources.UnitConverter),
                OwnerId = system.OwnerId,
            };
            // https://github.com/jfkonecn/OpenChE/blob/Better_UI/Backend/EngineeringMath/Component/DefaultFunctions/BernoullisEquation.cs
            Function bernoullisEquation = new Function()
            {
                Name = nameof(LibraryResources.BernoullisEquation),
                OwnerId = system.OwnerId,
            };
            Function orificePlate = new Function()
            {
                Name = nameof(LibraryResources.OrificePlate),
                OwnerId = system.OwnerId,
            };


            AddToBuilder(modelBuilder, new List<Function>() { areaFunction, unitConverter, orificePlate, bernoullisEquation }, (obj, idx) => obj.FunctionId = idx);
            FunctionOutputValueLink areaFunctionLink = new FunctionOutputValueLink()
            {
                FunctionId = areaFunction.FunctionId,
                OutputParameterName = nameof(LibraryResources.Area)
            };
            AddToBuilder(modelBuilder, new List<FunctionOutputValueLink>() { areaFunctionLink }, (obj, idx) => obj.FunctionOutputValueLinkId = idx);

            #endregion
            #region Parameters
            var areaParameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Diameter),
                        OwnerId = system.OwnerId,
                        ValueConditions = "$0 >= 0",
                        ParameterTypeId = doubleType.ParameterTypeId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Area),
                        OwnerId = system.OwnerId,
                        ValueConditions = "$0 >= 0",
                        ParameterTypeId = doubleType.ParameterTypeId,
                    }
                };
            areaParameters.ForEach((x) => x.FunctionId = areaFunction.FunctionId);
            var unitConverterParameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.UnitType),
                        OwnerId = system.OwnerId,
                        ParameterTypeId = unitCategoryType.ParameterTypeId,                        
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Input),
                        OwnerId = system.OwnerId,
                        ParameterTypeId = doubleType.ParameterTypeId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Output),
                        OwnerId = system.OwnerId,
                        ParameterTypeId = doubleType.ParameterTypeId,
                    }
                };
            unitConverterParameters.ForEach((x) => x.FunctionId = unitConverter.FunctionId);
            var bernoullisEquationParameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.InletVelocity),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = velocity.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.OutletVelocity),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = velocity.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.InletPressure),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = pressure.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.OutletPressure),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = pressure.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.InletHeight),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = length.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.OutletHeight),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = length.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Density),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = density.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    }
                };
            bernoullisEquationParameters.ForEach((x) => x.FunctionId = bernoullisEquation.FunctionId);

            var orificePlateParameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.DischargeCoefficient),
                        ParameterTypeId = integerType.ParameterTypeId,
                        UnitCategory = null,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.Density),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = density.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.InletPipeArea),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = area.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.OrificeArea),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = area.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.PressureDrop),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = area.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                    new Parameter()
                    {
                        ParameterName = nameof(LibraryResources.VolumetricFlowRate),
                        ParameterTypeId = doubleType.ParameterTypeId,
                        UnitCategoryId = volumetricFlowRate.UnitCategoryId,
                        ValueConditions = "$0 >= 0",
                        OwnerId = system.OwnerId,
                    },
                };
            orificePlateParameters.ForEach((x) => x.FunctionId = orificePlate.FunctionId);
            var AllParameters = orificePlateParameters
                .Union(bernoullisEquationParameters)
                .Union(unitConverterParameters)
                .Union(areaParameters);
            AddToBuilder(modelBuilder, AllParameters, (x, idx) => x.ParameterId = idx);
            #endregion
            #region ParameterValueLinks
            var unitConverterParameterValueLinks = new List<ParameterValueLink>()
                        {
                            new ParameterValueLink()
                            {
                                ParameterId = unitConverterParameters
                                    .Where(x => x.ParameterName == nameof(LibraryResources.Input))
                                    .Single()
                                    .ParameterId
                            },
                            new ParameterValueLink()
                            {
                                ParameterId = unitConverterParameters
                                    .Where(x => x.ParameterName == nameof(LibraryResources.Output))
                                    .Single()
                                    .ParameterId
                            }
                        };
            var allParameterValueLinks = unitConverterParameterValueLinks;
            AddToBuilder(modelBuilder, allParameterValueLinks, (x, idx) => x.ParameterValueLinkId = idx);
            #endregion
            #region FunctionOutputLinks
            var areaFunctionOutputLinks = AllParameters
                .Where(x => x.UnitCategoryId == area.UnitCategoryId)
                .Select(x => new ParameterFunctionOutputValueLink()
                {
                    FunctionOutputValueLinkId = areaFunctionLink.FunctionOutputValueLinkId,
                    ParameterId = x.ParameterId,
                }).ToList();
            var allFunctionOutputLinks = areaFunctionOutputLinks;
            AddToBuilder(modelBuilder, allFunctionOutputLinks, (x, idx) => x.ParameterFunctionOutputValueLinkId = idx);
            #endregion
            #region Equations

            var areaEquations = new List<Equation>()
            {
                new Equation()
                {
                    OutputName = nameof(LibraryResources.Area),
                    Formula = $"{nameof(LibraryResources.Diameter)} ^ 2 * PI() / 4",
                    OwnerId = system.OwnerId,
                }
            };
            areaEquations.ForEach(x => x.FunctionId = areaFunction.FunctionId);
            var unitConverterEquations = new List<Equation>()
            {
                new Equation()
                {
                    OutputName = nameof(LibraryResources.Output),
                    Formula = $"${nameof(LibraryResources.Input)}",
                    OwnerId = system.OwnerId,
                }
            };
            unitConverterEquations.ForEach(x => x.FunctionId = unitConverter.FunctionId);

            var bernoullisEquationEquations = new List<Equation>()
                {
                    new Equation()
                    {
                        Formula = $@"Sqrt(2 * (${LibraryResources.OutletVelocity} ^ 2 / 2 
                            + 9.81 * (${LibraryResources.OutletHeight} - ${LibraryResources.InletHeight} ) 
                            + (${LibraryResources.OutletPressure} - ${LibraryResources.InletPressure} ) 
                                / ${LibraryResources.Density} ))",
                        OutputName = LibraryResources.InletVelocity,
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"Sqrt(2 * (${nameof(LibraryResources.InletVelocity)} ^ 2 / 2 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) 
                                / ${nameof(LibraryResources.Density)}))",
                        OutputName = nameof(LibraryResources.OutletVelocity),
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.OutletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.OutletHeight)} - ${nameof(LibraryResources.InletHeight)}) 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.InletPressure),
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"${nameof(LibraryResources.Density)} 
                            * (${nameof(LibraryResources.InletPressure)} / ${nameof(LibraryResources.Density)} 
                            + 9.81 * (${nameof(LibraryResources.InletHeight)} - ${nameof(LibraryResources.OutletHeight)}) 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2)",
                        OutputName = nameof(LibraryResources.OutletPressure),
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"((${nameof(LibraryResources.OutletPressure)} - ${nameof(LibraryResources.InletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.OutletVelocity)} ^ 2 - ${nameof(LibraryResources.InletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.OutletHeight)}",
                        OutputName = nameof(LibraryResources.InletHeight),
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"((${nameof(LibraryResources.InletPressure)} - ${nameof(LibraryResources.OutletPressure)}) / ${nameof(LibraryResources.Density)} 
                            + (${nameof(LibraryResources.InletVelocity)} ^ 2 - ${nameof(LibraryResources.OutletVelocity)} ^ 2) / 2) / 9.81 
                            + ${nameof(LibraryResources.InletHeight)}",
                        OutputName = nameof(LibraryResources.OutletHeight),
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        Formula = $@"($pout - $pin) / (0.5 * ($uin ^ 2 - $uout ^ 2) 
                            + 9.81 * ($hin - ${nameof(LibraryResources.OutletHeight)}))",
                        OutputName = nameof(LibraryResources.Density),
                        OwnerId = system.OwnerId,
                    }
                };
            bernoullisEquationEquations.ForEach(x => x.FunctionId = bernoullisEquation.FunctionId);

            var orificePlateEquations = new List<Equation>()
                {
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.DischargeCoefficient),
                        Formula = $@"${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.InletPipeArea)} * 
                            Sqrt((2 * ${nameof(LibraryResources.PressureDrop)}) / (${nameof(LibraryResources.Density)} * 
                                (${nameof(LibraryResources.InletPipeArea)} ^ 2 / ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))))",
                        OwnerId = system.OwnerId,
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
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.InletPipeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.OrificeArea)} ^ 2) - 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.OrificeArea),
                        Formula = $@"Sqrt(1 / ((1 / ${nameof(LibraryResources.InletPipeArea)} ^ 2) + 
                            ((2 * ${nameof(LibraryResources.PressureDrop)} * ${nameof(LibraryResources.DischargeCoefficient)} ^ 2) / 
                                (${nameof(LibraryResources.VolumetricFlowRate)} ^ 2 * ${nameof(LibraryResources.Density)}))))",
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.PressureDrop),
                        Formula = $@"((${nameof(LibraryResources.VolumetricFlowRate)} / (${nameof(LibraryResources.DischargeCoefficient)} * 
                            ${nameof(LibraryResources.InletPipeArea)})) ^ 2 * 
                            (${nameof(LibraryResources.Density)} * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / 
                                ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1))) / 2",
                        OwnerId = system.OwnerId,
                    },
                    new Equation()
                    {
                        OutputName = nameof(LibraryResources.VolumetricFlowRate),
                        Formula = $@"${nameof(LibraryResources.DischargeCoefficient)} * 
                        ${nameof(LibraryResources.InletPipeArea)} * Sqrt((2 * ${nameof(LibraryResources.PressureDrop)}) 
                            / (${nameof(LibraryResources.Density)} * 
                            (${nameof(LibraryResources.InletPipeArea)} ^ 2 / ${nameof(LibraryResources.OrificeArea)} ^ 2 - 1)))",
                        OwnerId = system.OwnerId,
                    }
                };
            orificePlateEquations.ForEach(x => x.FunctionId = orificePlate.FunctionId);
            AddToBuilder<Equation>(modelBuilder, orificePlateEquations
                .Union(bernoullisEquationEquations)
                .Union(unitConverterEquations)
                .Union(areaEquations), (x, idx) => x.EquationId = idx);
            #endregion

            #region FunctionCategories
            FunctionCategory fluidDynamics = new FunctionCategory()
            {
                Name = nameof(LibraryResources.FluidDynamics),
            };
            FunctionCategory utility = new FunctionCategory()
            {
                Name = nameof(LibraryResources.Utility),
            };
            var functionCategories = new List<FunctionCategory>
                {
                    fluidDynamics, utility
                };

            AddToBuilder(modelBuilder, functionCategories, (obj, idx) => obj.FunctionCategoryId = idx);

            var functionCategoryFunctionMap = new List<FunctionCategoryFunction>()
            {
                new FunctionCategoryFunction()
                {
                    FunctionCategoryId = fluidDynamics.FunctionCategoryId,
                    FunctionId = bernoullisEquation.FunctionId,
                },
                new FunctionCategoryFunction()
                {
                    FunctionCategoryId = fluidDynamics.FunctionCategoryId,
                    FunctionId = orificePlate.FunctionId,
                },
                new FunctionCategoryFunction()
                {
                    FunctionCategoryId = utility.FunctionCategoryId,
                    FunctionId = unitConverter.FunctionId,
                },
                new FunctionCategoryFunction()
                {
                    FunctionCategoryId = utility.FunctionCategoryId,
                    FunctionId = areaFunction.FunctionId,
                },
            };
            modelBuilder.Entity<FunctionCategoryFunction>().HasData(functionCategoryFunctionMap);
            #endregion
        }

}
}
