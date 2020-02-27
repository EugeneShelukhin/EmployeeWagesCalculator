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
        }

        //подсчитываем зарплаты каждого и сравниваем со значением общей зарплаты
        [Test]
        public void Test1()
        {
            //arrange
            LoadWorkers();
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var workers = repository.GetAll();
            //act
            var wageCalculator = new WageCalculator(base.repository, new CustomCache<decimal>());
            var oneByOneSum=workers.Sum(worker => wageCalculator.CalculateWorkersWage(date, worker));
            var fullWageSum = wageCalculator.GetFullWageSum(date);

            //assert
            Assert.That(oneByOneSum, Is.EqualTo(fullWageSum));
            Assert.That(Math.Round(oneByOneSum, 2),  Is.EqualTo(825155.03));
        }
    }
}