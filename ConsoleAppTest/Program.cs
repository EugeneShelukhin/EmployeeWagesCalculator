using AsposeTest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dllApi = new PublicAPI();

            //parallel insert test

            var johnId = dllApi.AddManager(null, "John Doe", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var janeId = dllApi.AddSales(johnId, "Jane Doe", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var judiId = dllApi.AddSales(janeId, "Judy Doe", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var jamesId = dllApi.AddManager(judiId, "James Doe", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            List<Task> AddUsersTaskList = new List<Task>();
            for (var j = 0; j < 50; j++)
            {
                AddUsersTaskList.Add(
                    Task.Run(() =>
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            dllApi.AddManager(null, $"John Doe {i}", DateTime.ParseExact("2010-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddSales(johnId, $"Jane Doe {i}", DateTime.ParseExact("2011-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddSales(janeId, $"Judy Doe {i}", DateTime.ParseExact("2012-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddManager(judiId, $"James Doe {i}", DateTime.ParseExact("2013-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddEmployee(jamesId, $"Ivan Ivanov {i}", DateTime.ParseExact("2014-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddEmployee(jamesId, $"Piter Peterson {i}", DateTime.ParseExact("2015-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            dllApi.AddEmployee(null, $"mr. Independent Worker {i}", DateTime.ParseExact("2016-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                    })
                    );

            }
            Task.WaitAll(AddUsersTaskList.ToArray());

            //parallel calculation test

            var date = DateTime.ParseExact("2020-02-24", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var fullWageSum = dllApi.GetTotalWagesSum(date);

            List<Task> calculationTasks = new List<Task>();
            for (var j = 0; j < 20; j++)
            {
                calculationTasks.Add(Task.Run(() =>
                {
                    var fullWageSum = dllApi.GetTotalWagesSum(date);
                    Console.WriteLine($"result {fullWageSum}");
                    //expected value 4126261777.51122500000000M
                }));
            }
            Task.WaitAll(calculationTasks.ToArray());
        }
    }
}
