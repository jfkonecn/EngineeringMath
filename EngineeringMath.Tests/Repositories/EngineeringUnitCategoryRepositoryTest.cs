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

namespace EngineeringMath.Tests.Repositories
{
    [TestFixture]
    public class EngineeringUnitCategoryRepositoryTest
    {
        private Mock<IReadonlyRepository<UnitCategory>> UnitCategoryRepositoryMock { get; set; }
        private EngineeringUnitCategoryRepository SUT { get; set; }

        private IEnumerable<UnitCategory> MasterList { get; set; }
        private RepositoryResult<IEnumerable<UnitCategory>> ResultList { get; set; }

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
        public void ShouldGetSimpleEngineeringUnit()
        {
            // arrange 
            SetupMockRepositories();

            // act
            var result = SUT.GetById(nameof(LibraryResources.Length));

            // assert
            Assert.AreEqual(RepositoryStatusCode.success, result.StatusCode);
            Assert.NotNull(result.ResultObject);
            Assert.AreEqual(result.ResultObject.Name, nameof(LibraryResources.Length));

        }



        private void SetupMockRepositories()
        {
            Owner system = new Owner()
            {
                Name = "System"
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
            UnitCategory length = new UnitCategory()
            {
                Name = "Length",
                Owner = system,
                Units = new List<Unit>
                {
                    new Unit()
                    {
                        Name = nameof(LibraryResources.MeterFullName),
                        Symbol = nameof(LibraryResources.MetricAbbrev),
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
            #endregion

            MasterList = new List<UnitCategory>()
            {
                length,
                temperature,
                time,
                specificVolume
            };

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
                ResultList = new RepositoryResult<IEnumerable<UnitCategory>>(RepositoryStatusCode.objectNotFound, null);
            }
            else
            {
                ResultList = new RepositoryResult<IEnumerable<UnitCategory>>(RepositoryStatusCode.success, result);
            }

        }
    }
}
