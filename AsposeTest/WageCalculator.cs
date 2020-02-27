using AsposeTest.enums;
using System;
using System.Linq;

namespace AsposeTest
{
    public class WageCalculator
    {
        private readonly IWorkersRepository _workersRepository;
        public WageCalculator(IWorkersRepository workersRepository) {
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
            switch (worker.Role)
            {
                case RolesEnum.Employee:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Employee * GetWorkerExperience(date, worker);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Employee ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;
                        return worker.BasicWageRate + worker.BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }

                    break;
                case RolesEnum.Manager:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Manager * GetWorkerExperience(date, worker);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Manager ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;

                        var SubordinatesSumSalary = _workersRepository.GetSubordinatesOfFirstLevel(worker.Id).Sum(x => CalculateWorkersWage(date, x));
                        var increaseForSubordinates = SubordinatesSumSalary * (decimal)SubordinatesIncreasePercentage.Manager;
                        return worker.BasicWageRate + worker.BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }

                    break;
                case RolesEnum.Sales:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Sales * GetWorkerExperience(date, worker);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Sales ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;

                        var SubordinatesSumSalary = _workersRepository.GetSubordinatesOfAllLevels(worker.Id).Sum(x => CalculateWorkersWage(date, x));
                        var increaseForSubordinates = SubordinatesSumSalary * (decimal)SubordinatesIncreasePercentage.Sales;
                        return worker.BasicWageRate + worker.BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            int GetWorkerExperience(DateTime date, Worker worker){
                return (int)(date - worker.DateOfEmployment).TotalDays / 365;
            }
        }

    }
}
