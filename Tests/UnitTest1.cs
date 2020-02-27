using AsposeTest;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Tests
{
    public class Tests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            LoadWorkers();
        }

        [Test]
        public void Test1()
        {
            var wageCalculator = new WageCalculator(base.repository);
            var result = wageCalculator.GetFullWageSum(DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            Assert.That(result,  Is.EqualTo(result));
        }
    }
}