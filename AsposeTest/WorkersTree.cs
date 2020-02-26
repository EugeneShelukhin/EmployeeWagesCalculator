using System;
using System.Collections.Generic;
using System.Linq;

namespace AsposeTest
{
    public static class WorkersTree
    {
        public static List<Worker> WorkersCollection = new List<Worker>();

        public static decimal GetFullWageSum(DateTime date)
        {
            return WorkersCollection.Sum(worker => worker.CalculateWages(date));
        }

        public static decimal GetWageOfWorker(long Id, DateTime date)
        {
            return WorkersCollection.First(worker => worker.Id==Id).CalculateWages(date);
        }

        public static Worker[] GetSubordinatesOfFirstLevel(long Id) => WorkersCollection.Where(w => w.ChiefId == Id).ToArray();

        public static Worker[] GetSubordinatesOfAllLevels(long Id) {

            List<Worker> result = new List<Worker>();
            RecurcivelyGetSubordinates(Id, result);
            return result.ToArray();//distinct
        }

        private static void RecurcivelyGetSubordinates(long Id, List<Worker> result) {
            var subordinates = WorkersCollection.Where(w => w.ChiefId == Id).ToArray();
            if (subordinates == null)
                return;

            foreach (var s in subordinates) {
                RecurcivelyGetSubordinates(s.Id, result);
            }
            result.AddRange(subordinates);
        }
    }
}
