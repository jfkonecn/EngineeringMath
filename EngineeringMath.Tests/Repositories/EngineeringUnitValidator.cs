using EngineeringMath.EngineeringModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Tests.Repositories
{
    public class EngineeringUnitValidator
    {

        public string Symbol { get; set; } = null;
        public string FullName { get; set; } = null;
        public List<EngineeringUnitSystem> UnitSystems { get; } = new List<EngineeringUnitSystem>();
        public double CurrentUnitValue { get; set; } = double.NaN;
        public double SiValue { get; set; } = double.NaN;
        public string Owner { get; set; } = "SYSTEM";

        /// <summary>
        /// Uses all properties to validate the passed Engineering unit
        /// </summary>
        public void AssertUnitValid(IEnumerable<EngineeringUnit> units)
        {
            var unit = units
                .Where(x => x.Name == FullName)
                .SingleOrDefault();
            Assert.NotNull(unit);
            Assert.AreEqual(unit.Symbol, Symbol);
            Assert.AreEqual(unit.Name, FullName);
            Assert.AreEqual(UnitSystems.Count(), unit.UnitSystems.Where(actual => UnitSystems.Where(expected => {
                return actual.Name == expected.Name && actual.Abbreviation == expected.Abbreviation && actual.OwnerName == expected.OwnerName;
            }).Count() == 1).Count());

            Assert.That(unit.ConvertFromSi.Evaluate(SiValue), Is.EqualTo(CurrentUnitValue).Within(0.1).Percent);
            Assert.That(unit.ConvertToSi.Evaluate(CurrentUnitValue), Is.EqualTo(SiValue).Within(0.1).Percent);
            Assert.AreEqual(unit.OwnerName, Owner);
        }
    }
}
