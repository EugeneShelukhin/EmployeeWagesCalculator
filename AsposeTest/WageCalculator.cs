using System;
using System.Linq;

namespace AsposeTest
{
    class WageCalculator
    {
        public static decimal GetFullWageSum(DateTime date)
        {
            return WorkersTree.WorkersCollection.Sum(worker => worker.CalculateWages(date));
        }

        public static decimal GetWageOfWorker(long Id, DateTime date)
        {
            return WorkersTree.WorkersCollection.First(worker => worker.Id == Id).CalculateWages(date);
        }


    }
}
