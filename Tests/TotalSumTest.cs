using AsposeTest;
using AsposeTest.cache;
using AsposeTest.data;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;

namespace Tests
{
    public class Tests: TestBase
    {
        
        [SetUp]
        public void Setup()
        {
        }

        //check that total sum equals to the sum of wages calculated for each worker one by one
        [Test]
        public void GetTotalWagesSum()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var workers = repository.GetAll();
            //act
            var wageCalculator = new WagesCalculator(repository, new CustomCache<decimal>());
            var oneByOneSum=workers.Sum(worker => wageCalculator.CalculateWorkersWages(date, worker));
            var fullWageSum = wageCalculator.GetTotalWagesSum(date);

            //assert
            Assert.That(oneByOneSum, Is.EqualTo(fullWageSum));
            Assert.That(Math.Round(oneByOneSum, 2),  Is.EqualTo(825155.03));
        }
    }
}