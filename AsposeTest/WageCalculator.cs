using AsposeTest.enums;
using System;
using System.Linq;

namespace AsposeTest
{
    public class WageCalculator
    {
        private readonly IWorkersRepository _workersRepository;
        public WageCalculator(IWorkersRepository workersRepository)
        {
            _workersRepository = workersRepository;
        }

        public decimal GetFullWageSum(DateTime date)
        {
            return _workersRepository.GetAll().Sum(worker => CalculateWorkersWage(date, worker));
        }

        public decimal GetWageOfWorker(long Id, DateTime date)
        {
            return CalculateWorkersWage(date, _workersRepository.GetById(Id));
        }

        public decimal CalculateWorkersWage(DateTime date, Worker worker)
        {
            return GetWageWithExperience(date, worker) + getIncreaseForSubordinates(date, worker);
        }

        //TODO перенести
        private decimal GetWageWithExperience(DateTime date, Worker worker)
        {
            float annualPercent;
            float maxPercent;
            switch (worker.Role)
            {
                case RolesEnum.Employee:
                    maxPercent = MaxAnnualIncreasePercentage.Employee;
                    annualPercent = AnnualIncreasePercentage.Employee;
                    break;
                case RolesEnum.Manager:
                    maxPercent = MaxAnnualIncreasePercentage.Manager;
                    annualPercent = AnnualIncreasePercentage.Manager;
                    break;
                case RolesEnum.Sales:
                    maxPercent = MaxAnnualIncreasePercentage.Sales;
                    annualPercent = AnnualIncreasePercentage.Sales;
                    break;
                default:
                    throw new NotImplementedException();
            }


            var annualIncreasePercentage = annualPercent * GetWorkerExperience(date, worker);
            annualIncreasePercentage = annualIncreasePercentage < maxPercent? annualIncreasePercentage : maxPercent;
            var wageWithExperience = worker.BasicWageRate * (1 + ((decimal)annualIncreasePercentage / 100));
            return wageWithExperience;

            int GetWorkerExperience(DateTime date, Worker worker)
            {
                return (int)(date - worker.DateOfEmployment).TotalDays / 365;
            }
        }

        private decimal getIncreaseForSubordinates(DateTime date, Worker worker) {
            Worker[] subordinaries;
            float percent;
            switch (worker.Role)
            {
                case RolesEnum.Employee:
                    return 0;
                    break;
                case RolesEnum.Manager:
                    subordinaries = _workersRepository.GetSubordinatesOfFirstLevel(worker.Id);
                    percent = SubordinatesIncreasePercentage.Manager;
                    break;
                case RolesEnum.Sales:
                    subordinaries = _workersRepository.GetSubordinatesOfAllLevels(worker.Id);
                    percent = SubordinatesIncreasePercentage.Sales;
                    break;
                default:
                    throw new NotImplementedException();
            }
            var SubordinatesSumSalary = subordinaries.Sum(x => CalculateWorkersWage(date, x));
            var increaseForSubordinates = SubordinatesSumSalary * ((decimal) percent / 100);
            return increaseForSubordinates;
        }
    }
}
