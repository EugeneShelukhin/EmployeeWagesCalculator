using AsposeTest;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Tests
{
    public class PublicApiTests : TestBase
    {
        private IPublicAPI _publicAPI;

        [SetUp]
        public void Setup()
        {
            _publicAPI = new PublicAPI();
        }

        [Test]
        public void GetWagesForEmployee()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //act
            var result = _publicAPI.CalculateWorkersWages(date, piterId);
            //assert
            Assert.That(result, Is.EqualTo(112000));
        }

        [Test]
        public void GetWagesForManager()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //act
            var result = _publicAPI.CalculateWorkersWages(date, jamesId);
            //assert
            Assert.That(result, Is.EqualTo(131135));
        }

        [Test]
        public void GetWagesForSales()
        {
            //arrange
            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //act
            var result = _publicAPI.CalculateWorkersWages(date, judiId);
            //assert
            Assert.That(result, Is.EqualTo(108074.40500000M));
        }
    }
}