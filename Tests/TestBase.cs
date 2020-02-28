using AsposeTest;
using AsposeTest.cache;
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
            dataContext = DataContext.Instance;
            idCounter = new IdentifiersCounter(dataContext);
            var subordinatesCache = new CustomCache<Worker[]>();
            repository = new WorkersRepository(dataContext, idCounter, subordinatesCache);
        }

        public long johnId;
        public long janeId;
        public long judiId;
        public long jamesId;
        public long piterId;


        public void LoadWorkers()
        {
            johnId = repository.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            janeId = repository.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            judiId = repository.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            jamesId = repository.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            piterId = repository.AddEmployee(jamesId, "Piter Peterson", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            repository.AddEmployee(jamesId, "Ivan Ivanov", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            repository.AddEmployee(null, "mr. Independent Worker", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        public void LoadManyWorkers()
        {
            johnId = repository.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            janeId = repository.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            judiId = repository.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            jamesId = repository.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            for (var i = 0; i < 1000; i++)
            {
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
