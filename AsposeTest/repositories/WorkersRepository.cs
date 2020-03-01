using AsposeTest.cache;
using AsposeTest.data;
using AsposeTest.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AsposeTest
{
    public interface IWorkersRepository
    {
        long AddEmployee(long? chiefId, string name, DateTime? emploumentDate);
        long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);
        long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);
        IEnumerable<Worker> GetAll();
        Worker GetById(long id);
        List<Worker> GetSubordinatesOfAllLevels(long Id);
        List<Worker> GetSubordinatesOfFirstLevel(long Id);
    }

    public class WorkersRepository : IWorkersRepository
    {
        private readonly IDataContext _context;
        private static object _lockObj = new object();

        public WorkersRepository(IDataContext context)
        {
            _context = context;
        }


        public long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            lock (_lockObj)
            {
                return _context.Add(new Worker()
                {
                    ChiefId = chiefId,
                    Name = name,
                    Role = RolesEnum.Employee,
                    BasicWageRate = WageRates.BaseWage,
                    DateOfEmployment = emploumentDate ?? DateTime.Now
                });
            }
        }

        public long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            lock (_lockObj)
            {
                var id = _context.Add(new Worker()
                {
                    ChiefId = chiefId,
                    Name = name,
                    Role = RolesEnum.Manager,
                    BasicWageRate = WageRates.BaseWage,
                    DateOfEmployment = emploumentDate ?? DateTime.Now
                });

                //TODO bind subordinates

                //TODO inject calculate wage logic

                return id;
            }
        }

        public long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            lock (_lockObj)
            {
                var id = _context.Add(new Worker()
                {
                    ChiefId = chiefId,
                    Name = name,
                    Role = RolesEnum.Sales,
                    BasicWageRate = WageRates.BaseWage,
                    DateOfEmployment = emploumentDate ?? DateTime.Now
                });

                //TODO bind subordinates

                //TODO inject calculate wage logic
                return id;
            }
        }

        public IEnumerable<Worker> GetAll() {
            lock (_lockObj)
            {
                return _context.WorkersCollection.ToArray();
            }
        }
        public Worker GetById(long id)
        {
            lock (_lockObj)
            {
                return _context.WorkersCollection.First(worker => worker.Id == id);
            }
        }
        public List<Worker> GetSubordinatesOfFirstLevel(long id)
        {
            lock (_lockObj)
            {
                _context.SubordinatesCache.TryGetValue(id, out var subordinates);
                return subordinates ?? new List<Worker>();
            }
        }

        public List<Worker> GetSubordinatesOfAllLevels(long Id)
        {
            lock (_lockObj)
            {
                List<Worker> result = new List<Worker>();
                RecurcivelyGetSubordinates(Id, result);
                return result;//.Distinct();
            }
        }

        private void RecurcivelyGetSubordinates(long id, List<Worker> result)
        {
            if (!_context.SubordinatesCache.TryGetValue(id, out var subordinates))
            {
                return;
            }

            foreach (var s in subordinates)
            {
                RecurcivelyGetSubordinates(s.Id, result);
            }
            result.AddRange(subordinates);
        }
    }
}
