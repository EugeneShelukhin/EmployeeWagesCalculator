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
                        var wageWithExperience = worker.BasicWageRate * (1 + ((decimal)annualIncreasePercentage / 100));
                        return wageWithExperience;
                    }

                    break;
                case RolesEnum.Manager:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Manager * GetWorkerExperience(date, worker);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Manager ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Manager;
                        var wageWithExperience = worker.BasicWageRate * (1 + ((decimal)annualIncreasePercentage / 100));

                        var SubordinatesSumSalary = _workersRepository.GetSubordinatesOfFirstLevel(worker.Id).Sum(x => CalculateWorkersWage(date, x));
                        var increaseForSubordinates = SubordinatesSumSalary * ((decimal)SubordinatesIncreasePercentage.Manager / 100);
                        return wageWithExperience + increaseForSubordinates;
                    }
                    break;
                case RolesEnum.Sales:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Sales * GetWorkerExperience(date, worker);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Sales ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Sales;
                        var wageWithExperience = worker.BasicWageRate * (1 + ((decimal)annualIncreasePercentage / 100));

                        var SubordinatesSumSalary = _workersRepository.GetSubordinatesOfAllLevels(worker.Id).Sum(x => CalculateWorkersWage(date, x));
                        var increaseForSubordinates = SubordinatesSumSalary *  ((decimal)SubordinatesIncreasePercentage.Sales / 100);
                        return wageWithExperience + increaseForSubordinates;
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
