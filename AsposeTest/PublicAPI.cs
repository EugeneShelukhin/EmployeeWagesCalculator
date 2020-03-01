using AsposeTest.cache;
using AsposeTest.data;
using System;

namespace AsposeTest
{
    public interface IPublicAPI
    {
        /// <summary>
        /// Add Worker with role Employee
        /// </summary>
        /// <param name="chiefId">Chef's id</param>
        /// <param name="name">worker's name</param>
        /// <param name="emploumentDate">date of employment</param>
        /// <returns>new worker's id</returns>
        long AddEmployee(long? chiefId, string name, DateTime? emploumentDate);

        /// <summary>
        /// Add Worker with role Manager
        /// </summary>
        /// <param name="chiefId">Chef's id</param>
        /// <param name="name">worker's name</param>
        /// <param name="emploumentDate">date of employment</param>
        /// <param name="subordinates">List of ids of subordinates of the 1st level</param>
        /// <returns>new worker's id</returns>
        long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);

        /// <summary>
        /// Add Worker with role Sales
        /// </summary>
        /// <param name="chiefId">Chef's id</param>
        /// <param name="name">worker's name</param>
        /// <param name="emploumentDate">date of employment</param>
        /// <param name="subordinates">List of ids of subordinates of the 1st level</param>
        /// <returns>new worker's id</returns>
        long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);

        /// <summary>
        /// Calculate worker's wages by worker id
        /// </summary>
        /// <param name="date">date of wages withdrawal</param>
        /// <param name="workerId">worker's id</param>
        /// <returns>worker's wages value</returns>
        decimal CalculateWorkersWages(DateTime date, long workerId);

        /// <summary>
        /// Calculate worker's wages by worker data model
        /// </summary>
        /// <param name="date">date of wages withdrawal</param>
        /// <param name="worker">worker model</param>
        /// <returns>worker's wages value</returns>
        decimal CalculateWorkersWages(DateTime date, Worker worker);

        /// <summary>
        /// Calculate total sum of wages of all workers in company
        /// </summary>
        /// <param name="date">date of wages withdrawal</param>
        /// <returns>Total sum</returns>
        decimal GetTotalWagesSum(DateTime date);
    }

    public class PublicAPI : IPublicAPI
    {
        private readonly IWorkersRepository _repository;
        public PublicAPI()
        {
            var dataContext = DataContext.Instance;
            _repository = new WorkersRepository(dataContext);
        }

        public long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            return _repository.AddEmployee(chiefId, name, emploumentDate);
        }

        public long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            return _repository.AddManager(chiefId, name, emploumentDate, subordinates);
        }

        public long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            return _repository.AddSales(chiefId, name, emploumentDate, subordinates);
        }
        public decimal CalculateWorkersWages(DateTime date, long workerId)
        {
            var workerWagesCache = new CustomCache<decimal>();
            return new WagesCalculator(_repository, workerWagesCache).CalculateWorkersWages(date, workerId);
        }
        public decimal CalculateWorkersWages(DateTime date, Worker worker)
        {
            var workerWagesCache = new CustomCache<decimal>();
            return new WagesCalculator(_repository, workerWagesCache).CalculateWorkersWages(date, worker);
        }
        public decimal GetTotalWagesSum(DateTime date)
        {
            var workerWagesCache = new CustomCache<decimal>();
            return new WagesCalculator(_repository, workerWagesCache).GetTotalWagesSum(date);
        }


    }
}
