using AsposeTest.cache;
using AsposeTest.enums;
using AsposeTest.WagesCalculatorDecorator;
using System;
using System.Linq;

namespace AsposeTest
{
    public interface IWagesCalculator
    {
        decimal CalculateWorkersWages(DateTime date, long id);
        decimal CalculateWorkersWages(DateTime date, Worker worker);
        decimal GetTotalWagesSum(DateTime date);
    }

    public class WagesCalculator : IWagesCalculator
    {
        private readonly IWorkersRepository _workersRepository;
        private readonly ICustomCache<decimal> _workerWageCache;
        public WagesCalculator(IWorkersRepository workersRepository, ICustomCache<decimal> workerWageCache)
        {
            _workersRepository = workersRepository;
            _workerWageCache = workerWageCache;
        }

        public decimal GetTotalWagesSum(DateTime date)
        {
            return _workersRepository.GetAll().AsParallel().Sum(worker => CalculateWorkersWages(date, worker));
        }

        public decimal CalculateWorkersWages(DateTime date, long id)
        {
            return CalculateWorkersWages(date, _workersRepository.GetById(id));
        }

        public decimal CalculateWorkersWages(DateTime date, Worker worker)
        {
            if (_workerWageCache.Get(worker.Id, out var value))
            {
                return value;
            }


            switch (worker.Role)
            {
                case RolesEnum.Employee:
                    {
                        var calc = new WorkerWagesClalculatorDecorator(worker);
                        calc = new EmploeeEncreaseForExperience(worker, calc);
                        calc = new EmploeeEncreaseForSubordinates(worker, calc);
                        value = calc.GetWages(date);
                        break;
                    }
                case RolesEnum.Manager:
                    {
                        var calc = new WorkerWagesClalculatorDecorator(worker);
                        calc = new ManagerEncreaseForExperience(worker, calc);
                        calc = new ManagerEncreaseForSubordinates(worker, calc, _workersRepository, this);
                        value = calc.GetWages(date);
                        break;
                    }

                case RolesEnum.Sales:
                    {
                        var calc = new WorkerWagesClalculatorDecorator(worker);
                        calc = new SalesEncreaseForExperience(worker, calc);
                        calc = new SalesEncreaseForSubordinates(worker, calc, _workersRepository, this);
                        value = calc.GetWages(date);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
            _workerWageCache.Add(worker.Id, value);
            return value;
        }


    }
}
