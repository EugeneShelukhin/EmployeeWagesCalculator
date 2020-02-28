using AsposeTest;
using AsposeTest.cache;
using AsposeTest.data;
using System;
using System.Globalization;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //deep hierarchy
            var dllApi = new PublicAPI();

            var johnId = dllApi.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var janeId = dllApi.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var judiId = dllApi.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var jamesId = dllApi.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            for (var i = 0; i < 5000; i++)
            {
                dllApi.AddManager(null, $"John Doe {i}", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddSales(johnId, $"Jane Doe {i}", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddSales(janeId, $"Judy Doe {i}", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddManager(judiId, $"James Doe {i}", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddEmployee(jamesId, $"Ivan Ivanov {i}", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddEmployee(jamesId, $"Piter Peterson {i}", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                dllApi.AddEmployee(null, $"mr. Independent Worker {i}", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var fullWageSum = dllApi.GetTotalWagesSum(date);
            //expected value 4126261777.51122500000000M
        }
    }
}
