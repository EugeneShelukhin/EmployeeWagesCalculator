﻿using AsposeTest;
using AsposeTest.data;
using System;
using System.Globalization;

namespace Tests
{
    public abstract class TestBase
    {
        private readonly IDataContext dataContext;
        private readonly IIdentifiersCounter idCounter;
        protected readonly IWorkersRepository repository;
        public TestBase()
        {
            dataContext = new DataContext();
            idCounter = new IdentifiersCounter(dataContext);
            repository = new WorkersRepository(dataContext, idCounter);
        }


        public void LoadWorkers()
        {
            var johnId = repository.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var janeId = repository.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var judiId = repository.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var jamesId = repository.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            repository.AddEmployee(jamesId, "Ivan Ivanov", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            repository.AddEmployee(jamesId, "Piter Peterson", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            repository.AddEmployee(null, "mr. Independent Worker", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        public void LoadManyWorkers()
        {
            var johnId = repository.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var janeId = repository.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var judiId = repository.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var jamesId = repository.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            for (var i = 0; i < 1000; i++) {
                repository.AddManager(null, $"John Doe {i}", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddSales(johnId, $"Jane Doe {i}", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddSales(janeId, $"Judy Doe {i}", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddManager(judiId, $"James Doe {i}", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(jamesId, $"Ivan Ivanov {i}", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(jamesId, $"Piter Peterson {i}", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                repository.AddEmployee(null, $"mr. Independent Worker {i}", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
        }
    }
}
