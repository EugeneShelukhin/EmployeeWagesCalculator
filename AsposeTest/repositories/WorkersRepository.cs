using AsposeTest.data;
using AsposeTest.enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IIdentifiersCounter _identifiersCounter;
        public WorkersRepository(IDataContext context, IIdentifiersCounter identifiersCounter)
        {
            _context = context;
            _identifiersCounter = identifiersCounter;
        }

        public long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            var id = _identifiersCounter.IssueNewIdentifier();
            _context.WorkersCollection.Add(new Worker()
            {
                Id = id,
                ChiefId = chiefId,
                Name = name,
                Role = RolesEnum.Employee,
                BasicWageRate = WageRates.BaseWage,
                DateOfEmployment = emploumentDate ?? DateTime.Now
            });
            return id;
        }

        public long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            var id = _identifiersCounter.IssueNewIdentifier();
            _context.WorkersCollection.Add(new Worker()
            {
                Id = id,
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

        public long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            var id = _identifiersCounter.IssueNewIdentifier();
            _context.WorkersCollection.Add(new Worker()
            {
                Id = id,
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

        public IEnumerable<Worker> GetAll() => _context.WorkersCollection;
        public Worker GetById(long id) => _context.WorkersCollection.First(worker => worker.Id == id);
        public Worker[] GetSubordinatesOfFirstLevel(long Id) => _context.WorkersCollection.Where(w => w.ChiefId == Id).ToArray();

        public Worker[] GetSubordinatesOfAllLevels(long Id)
        {
            List<Worker> result = new List<Worker>();
            RecurcivelyGetSubordinates(Id, result);
            return result.Distinct().ToArray();
        }

        private void RecurcivelyGetSubordinates(long Id, List<Worker> result)
        {

            var subordinates = _context.WorkersCollection.Where(w => w.ChiefId == Id).ToArray();
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
