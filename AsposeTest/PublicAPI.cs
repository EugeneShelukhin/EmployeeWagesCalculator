using AsposeTest.cache;
using AsposeTest.core;
using AsposeTest.data;
using System;

namespace AsposeTest
{
    public interface IPublicAPI
    {
        /// <summary>
        /// Add Worker with role Employee
        /// </summary>
        /// <param name="chiefId"></param>
        /// <param name="name"></param>
        /// <param name="emploumentDate"></param>
        /// <returns></returns>
        long AddEmployee(long? chiefId, string name, DateTime? emploumentDate);
        long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);
        long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null);
        decimal CalculateWorkersWages(DateTime date, long workerId);
        decimal CalculateWorkersWages(DateTime date, Worker worker);
        decimal GetTotalWagesSum(DateTime date);
    }

    public class PublicAPI : IPublicAPI
    {
        private readonly ISimpleResolver _resolver;
        public PublicAPI() {
            _resolver = new SimpleResolver();
        }

        public long AddEmployee(long? chiefId, string name, DateTime? emploumentDate)
        {
            return _resolver.ResolveRepository().AddEmployee(chiefId, name, emploumentDate);
        }

        public long AddManager(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            return _resolver.ResolveRepository().AddManager(chiefId, name, emploumentDate, subordinates);
        }

        public long AddSales(long? chiefId, string name, DateTime? emploumentDate, long[] subordinates = null)
        {
            return _resolver.ResolveRepository().AddSales(chiefId, name, emploumentDate, subordinates);
        }
        public decimal CalculateWorkersWages(DateTime date, long workerId)
        {
            return _resolver.ResolveWagesCalculator().CalculateWorkersWages(date, workerId);
        }
        public decimal CalculateWorkersWages(DateTime date, Worker worker)
        {
            return _resolver.ResolveWagesCalculator().CalculateWorkersWages(date, worker);
        }
        public decimal GetTotalWagesSum(DateTime date)
        {
            return _resolver.ResolveWagesCalculator().GetTotalWagesSum(date);
        }

        
    }
}
