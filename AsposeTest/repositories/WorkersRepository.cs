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
        Worker[] GetSubordinatesOfAllLevels(long Id);
        Worker[] GetSubordinatesOfFirstLevel(long Id);
    }

    public class WorkersRepository : IWorkersRepository
    {
        private readonly IDataContext _context;
        
        private readonly ICustomCache<Worker[]> _subordinatesCache;
        private ReaderWriterLockSlim RWLock = new ReaderWriterLockSlim();
        public WorkersRepository(IDataContext context, ICustomCache<Worker[]> subordinatesCache)
        {
            _context = context;
            _subordinatesCache = subordinatesCache;
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
        public Worker[] GetSubordinatesOfFirstLevel(long id)
        {
            if (!_subordinatesCache.Get(id, out var subordinates))
            {
                RWLock.EnterReadLock();
                try
                {
                    subordinates = _context.WorkersCollection.Where(w => w.ChiefId == id).ToArray();
                    _subordinatesCache.Add(id, subordinates);
                }
                finally
                {
                    RWLock.ExitReadLock();
                }
            }
            return subordinates;
        }

        public Worker[] GetSubordinatesOfAllLevels(long Id)
        {
            RWLock.EnterReadLock();
            try
            {
                List<Worker> result = new List<Worker>();
                RecurcivelyGetSubordinates(Id, result);
                return result.Distinct().ToArray();
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        private void RecurcivelyGetSubordinates(long id, List<Worker> result)
        {
            if (!_subordinatesCache.Get(id, out var subordinates))
            {
                subordinates = _context.WorkersCollection.Where(w => w.ChiefId == id).ToArray();
                _subordinatesCache.Add(id, subordinates);
            }

            if (subordinates == null)
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
