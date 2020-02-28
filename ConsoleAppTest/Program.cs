﻿using AsposeTest;
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
            var dataContext = DataContext.Instance;
            var idCounter = new IdentifiersCounter(dataContext);
            var subordinatesCache = new CustomCache<Worker[]>();
            var repository = new WorkersRepository(dataContext, idCounter, subordinatesCache);

            var johnId = repository.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var janeId = repository.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var judiId = repository.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var jamesId = repository.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            for (var i = 0; i < 5000; i++)
            {
                repository.AddManager(null, $"John Doe {i}", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddSales(johnId, $"Jane Doe {i}", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddSales(janeId, $"Judy Doe {i}", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddManager(judiId, $"James Doe {i}", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(jamesId, $"Ivan Ivanov {i}", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(jamesId, $"Piter Peterson {i}", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(null, $"mr. Independent Worker {i}", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var workers = repository.GetAll();
            var workerWagesCache = new CustomCache<decimal>();
            var wageCalculator = new WagesCalculator(repository, workerWagesCache);
            var fullWageSum = wageCalculator.GetTotalWagesSum(date);//4126261777.51122500000000M
        }
    }
}