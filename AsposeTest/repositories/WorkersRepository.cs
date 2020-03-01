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
        private ReaderWriterLockSlim RWLock = new ReaderWriterLockSlim();
        public WorkersRepository(IDataContext context)
        {
            _context = context;
        }


        public long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            RWLock.EnterWriteLock();
            try
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
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            RWLock.EnterWriteLock();
            try
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
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            RWLock.EnterWriteLock();
            try
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
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public IEnumerable<Worker> GetAll() => _context.WorkersCollection;
        public Worker GetById(long id)
        {
            RWLock.EnterReadLock();
            try
            {
                return _context.WorkersCollection.First(worker => worker.Id == id);
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }
        public List<Worker> GetSubordinatesOfFirstLevel(long id)
        {
            RWLock.EnterReadLock();
            try
            {
                _context.SubordinatesCache.TryGetValue(id, out var subordinates);
                return subordinates ?? new List<Worker>();
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        public 
            List<Worker> GetSubordinatesOfAllLevels(long Id)
        {
            RWLock.EnterReadLock();
            try
            {
                List<Worker> result = new List<Worker>();
                RecurcivelyGetSubordinates(Id, result);
                return result;//.Distinct();
            }
            finally
            {
                RWLock.ExitReadLock();
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
        ~WorkersRepository()
        {
            if (RWLock != null) RWLock.Dispose();
        }
    }
}
