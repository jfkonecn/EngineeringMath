using EngineeringMath.Loggers;
using EngineeringMath.Model;
using EngineeringMath.Repositories;
using EngineeringMath.Resources;
using EngineeringMath.Results;
using Moq;
using NUnit.Framework;
using StringMath;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Tests.Mocks;

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class EngineeringUnitCategoryRepositoryTest
    {
        private Mock<IReadonlyRepository<UnitCategory>> UnitCategoryRepositoryMock { get; set; }
        private EngineeringUnitCategoryRepository SUT { get; set; }

        private IEnumerable<UnitCategory> MasterList { get; set; }
        private MockResult<IEnumerable<UnitCategory>> ResultList { get; } = new MockResult<IEnumerable<UnitCategory>>();
        public string SystemString { get; } = "System";

        [SetUp]
        public void ClassSetup()
        {
            UnitCategoryRepositoryMock = new Mock<IReadonlyRepository<UnitCategory>>();
            SUT = new EngineeringUnitCategoryRepository(
                UnitCategoryRepositoryMock.Object, 
                new StringEquationFactory(), 
                new ConsoleLogger());
        }


        [Test]
        public void ShouldHandleCorruptedCompositeUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            var result = SUT.GetById("BadComposite");

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleCorruptedUnitsInUnitCategory()
        {
            // arrange
            SetupMockRepositories(true);

            // act
            var result = SUT.GetById("BadUnits");

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleRepositoryError()
        {
            // arrange
            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<UnitCategory, bool>>()))
                .Returns(new RepositoryResult<IEnumerable<UnitCategory>>(RepositoryStatusCode.internalError, null));

            // act
            var result = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.internalError, result.StatusCode);
            Assert.IsNull(result.ResultObject);
        }

        [Test]
        public void ShouldHandleGetAllUnitCategories()
        {
            // arrange
            SetupMockRepositories();

            // act
            var result = SUT.GetAll();

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.IsNotNull(result.ResultObject);
            Assert.AreEqual(MasterList.Count(), result.ResultObject.Count());
        }

        [Test]
        public void ShouldGetSimpleEngineeringUnit()
        {
            // arrange 
            SetupMockRepositories();

            // act
            var result = SUT.GetById(nameof(LibraryResources.Length));
            var meters = result.ResultObject?.Units
                .Where(x => x.Name == nameof(LibraryResources.MeterFullName))
                .SingleOrDefault();

            var feet = result.ResultObject?.Units
                .Where(x => x.Name == nameof(LibraryResources.FeetFullName))
                .SingleOrDefault();

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.AreEqual(nameof(LibraryResources.Length), result.ResultObject.Name);
            Assert.AreEqual(7, result.ResultObject.Units.Count());
            Assert.NotNull(meters);
            Assert.AreEqual(20.2, meters.ConvertFromSi.Evaluate(20.2));
            Assert.AreEqual(20.2, meters.ConvertToSi.Evaluate(20.2));
            Assert.AreEqual(meters.OwnerName, SystemString);
            Assert.AreEqual(meters.Symbol, nameof(LibraryResources.MeterAbbrev));
            Assert.IsNotNull(meters
                .UnitSystems.Where(x => x == nameof(LibraryResources.SIFullName))
                .SingleOrDefault());
            Assert.NotNull(feet);
            Assert.That(feet.ConvertFromSi.Evaluate(20.2), Is.EqualTo(66.27297).Within(0.1).Percent);
            Assert.That(feet.ConvertToSi.Evaluate(20.2), Is.EqualTo(6.15696).Within(0.1).Percent);
            Assert.AreEqual(feet.OwnerName, SystemString);
            Assert.AreEqual(feet.Symbol, nameof(LibraryResources.FeetAbbrev));
            Assert.IsNotNull(feet
                .UnitSystems.Where(x => x == nameof(LibraryResources.USCSFullName))
                .SingleOrDefault());
        }



        private void SetupMockRepositories(bool addBadData = false)
        {
            Owner system = new Owner()
            {
                Name = SystemString
            };
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
                Children = new List<UnitSystem>
                {
                    uscsUnits
                }
            };
            UnitSystem metricUnits = new UnitSystem()
            {
                Abbreviation = nameof(LibraryResources.MetricAbbrev),
                Name = nameof(LibraryResources.MetricFullName),
                Owner = system,
                Children = new List<UnitSystem>
                {
                    siUnits
                }
            };
            #endregion
            #region UnitCategory

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
            UnitCategory length = new UnitCategory()
            {
                Name = "Length",
                Owner = system,
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MeterFullName),
                        Symbol = nameof(LibraryResources.MeterAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
                        {
                            metricUnits
                        },
                        Owner = system
                    }
                },
            };
            UnitCategory temperature = new UnitCategory()
            {
                Name = nameof(LibraryResources.Temperature),
                Owner = system,
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.KelvinFullName),
                        Symbol = nameof(LibraryResources.KelvinAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
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
                        UnitSystems = new List<UnitSystem>
                        {
                            metricUnits,
                            imperialUnits
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

            UnitCategory corruptedCompositeEquation = new UnitCategory()
            {
                CompositeEquation = "Im bad",
                Name = "BadComposite",
                Owner = system,
            };
            UnitCategory corruptedUnits = new UnitCategory()
            {
                Name = "BadUnits",
                Owner = system,
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.SecondsFullName),
                        Symbol = nameof(LibraryResources.SecondsAbbrev),
                        ConvertFromSi = "$0",
                        ConvertToSi = "$0",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                    new Unit()
                    {
                        Name = "Bad unit",
                        Symbol = "yep",
                        ConvertFromSi = "bad equation",
                        ConvertToSi = "bad equation",
                        IsOnAbsoluteScale = true,
                        UnitSystems = new List<UnitSystem>
                        {
                            siUnits,
                            uscsUnits
                        },
                        Owner = system
                    },
                }
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

            #endregion

            MasterList = new List<UnitCategory>()
            {
                length,
                temperature,
                time,
                specificVolume,
                mass,
                volume
            };

            if (addBadData)
            {
                ((List<UnitCategory>)MasterList).Add(corruptedCompositeEquation);
                ((List<UnitCategory>)MasterList).Add(corruptedUnits);
            }

            UnitCategoryRepositoryMock
                .Setup(x => x.GetAllWhere(It.IsAny<Func<UnitCategory, bool>>()))
                .Callback<Func<UnitCategory, bool>>(CreateResult)
                .Returns(ResultList);

        }

        private void CreateResult(Func<UnitCategory, bool> whereCondition)
        {
            var result = MasterList.Where(whereCondition);
            if(result.Count() == 0)
            {
                ResultList.StatusCode = RepositoryStatusCode.objectNotFound;
                ResultList.ResultObject = null;
            }
            else
            {
                ResultList.StatusCode = RepositoryStatusCode.success;
                ResultList.ResultObject = result;
            }

        }
    }
}
