using AsposeTest;
using AsposeTest.cache;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;

namespace Tests
{
    public class Tests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            LoadWorkers();
        }

        //check that total sum equals to sum of wages calculated one by one for each worker
        [Test]
        public void GetTotalWagesSum()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var workers = repository.GetAll();
            //act
            var wageCalculator = new WagesCalculator(base.repository, new CustomCache<decimal>());
            var oneByOneSum=workers.Sum(worker => wageCalculator.CalculateWorkersWages(date, worker));
            var fullWageSum = wageCalculator.GetTotalWagesSum(date);

            //assert
            Assert.That(oneByOneSum, Is.EqualTo(fullWageSum));
            Assert.That(Math.Round(oneByOneSum, 2),  Is.EqualTo(825155.03));
        }

        [Test]
        public void GetWagesForEmployee() {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var wageCalculator = new WagesCalculator(base.repository, new CustomCache<decimal>());
            //act
            var result = wageCalculator.CalculateWorkersWages(date, base.piterId);
            //assert
            Assert.That(result, Is.EqualTo(112000));
        }

        [Test]
        public void GetWagesForManager()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var wageCalculator = new WagesCalculator(base.repository, new CustomCache<decimal>());
            //act
            var result = wageCalculator.CalculateWorkersWages(date, base.jamesId);
            //assert
            Assert.That(result, Is.EqualTo(131135));
        }

        [Test]
        public void GetWagesForSales()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var wageCalculator = new WagesCalculator(base.repository, new CustomCache<decimal>());
            //act
            var result = wageCalculator.CalculateWorkersWages(date, base.judiId);
            //assert
            Assert.That(result, Is.EqualTo(108074.40500000M));
        }
    }
}