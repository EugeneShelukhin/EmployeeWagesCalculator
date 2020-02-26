using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AsposeTest
{
    public static class WorkersTree
    {
        public static List<Worker> WorkersCollection = new List<Worker>();

        private static long counter = WorkersCollection.Max(w => w.Id);

        private static long IssueNewIdentifier() => Interlocked.Increment(ref counter);

        public static long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            var id=IssueNewIdentifier();
            WorkersCollection.Add(new Worker()
            {
                Id = id,
                ChiefId = chiefId,
                Name = name,
                Role = RolesEnum.Employee,
                BasicWageRate= WageRates.BaseWage,
                DateOfEmployment= emploumentDate??DateTime.Now
            });
            return id;
        }

        public static long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates=null)
        {
            var id = IssueNewIdentifier();
            WorkersCollection.Add(new Worker()
            {
                Id = id,
                ChiefId = chiefId,
                Name = name,
                Role = RolesEnum.Employee,
                BasicWageRate = WageRates.BaseWage,
                DateOfEmployment = emploumentDate??DateTime.Now
            });

            //TODO bind subordinates

            //TODO inject calculate wage logic

            return id;
        }

        public static long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates=null)
        {
            var id = IssueNewIdentifier();
            WorkersCollection.Add(new Worker()
            {
                Id = id,
                ChiefId = chiefId,
                Name = name,
                Role = RolesEnum.Employee,
                BasicWageRate = WageRates.BaseWage,
                DateOfEmployment = emploumentDate??DateTime.Now
            });

            //TODO bind subordinates

            //TODO inject calculate wage logic
            return id;
        }

        public static Worker[] GetSubordinatesOfFirstLevel(long Id) => WorkersCollection.Where(w => w.ChiefId == Id).ToArray();

        public static Worker[] GetSubordinatesOfAllLevels(long Id)
        {

            List<Worker> result = new List<Worker>();
            RecurcivelyGetSubordinates(Id, result);
            return result.ToArray();//distinct
        }

        private static void RecurcivelyGetSubordinates(long Id, List<Worker> result)
        {
            var subordinates = WorkersCollection.Where(w => w.ChiefId == Id).ToArray();
            if (subordinates == null)
                return;

            foreach (var s in subordinates)
            {
                RecurcivelyGetSubordinates(s.Id, result);
            }
            result.AddRange(subordinates);
        }
    }
}
