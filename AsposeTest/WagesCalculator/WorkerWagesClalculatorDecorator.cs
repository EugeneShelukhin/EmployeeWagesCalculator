using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsposeTest.WagesCalculatorDecorator
{
    public class WorkerWagesClalculatorDecorator
    {
        public WorkerWagesClalculatorDecorator(Worker worker)
        {
            this.worker = worker;
        }
        public Worker worker { get; protected set; }
        public virtual decimal GetWages(DateTime date) {
            return worker.BasicWageRate;
        }
    }
   
    public abstract class ClalculatorDecorator : WorkerWagesClalculatorDecorator
    {
        protected WorkerWagesClalculatorDecorator workerWagesClalculator;
        public ClalculatorDecorator(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator) : base(worker)
        {
            this.workerWagesClalculator = workerWagesClalculator;
        }
    }

    public class EmploeeEncreaseForExperience : ClalculatorDecorator
    {
        public EmploeeEncreaseForExperience(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator)
            : base(worker, workerWagesClalculator)
        { }

        public override decimal GetWages(DateTime date)
        {
            var maxPercent = MaxAnnualIncreasePercentage.Employee;
            var annualPercent = AnnualIncreasePercentage.Employee;
            var annualIncreasePercentage = annualPercent * worker.GetWorkerExperience(date);
            annualIncreasePercentage = annualIncreasePercentage < maxPercent ? annualIncreasePercentage : maxPercent;
            return workerWagesClalculator.GetWages(date) * (1 + ((decimal)annualIncreasePercentage / 100));
        }
    }

    public class ManagerEncreaseForExperience : ClalculatorDecorator
    {
        public ManagerEncreaseForExperience(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator)
            : base(worker, workerWagesClalculator)
        { }

        public override decimal GetWages(DateTime date)
        {
            var maxPercent = MaxAnnualIncreasePercentage.Manager;
            var annualPercent = AnnualIncreasePercentage.Manager;
            var annualIncreasePercentage = annualPercent * worker.GetWorkerExperience(date);
            annualIncreasePercentage = annualIncreasePercentage < maxPercent ? annualIncreasePercentage : maxPercent;
            return workerWagesClalculator.GetWages(date) * (1 + ((decimal)annualIncreasePercentage / 100));
        }
    }

    public class SalesEncreaseForExperience : ClalculatorDecorator
    {
        public SalesEncreaseForExperience(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator)
            : base(worker, workerWagesClalculator)
        { }

        public override decimal GetWages(DateTime date)
        {
            var maxPercent = MaxAnnualIncreasePercentage.Sales;
            var annualPercent = AnnualIncreasePercentage.Sales;
            var annualIncreasePercentage = annualPercent * worker.GetWorkerExperience(date);
            annualIncreasePercentage = annualIncreasePercentage < maxPercent ? annualIncreasePercentage : maxPercent;
            return workerWagesClalculator.GetWages(date) * (1 + ((decimal)annualIncreasePercentage / 100));
        }
    }

    public class EmploeeEncreaseForSubordinates : ClalculatorDecorator
    {
        public EmploeeEncreaseForSubordinates(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator)
            : base(worker, workerWagesClalculator)
        { }

        public override decimal GetWages(DateTime date)
        {//Emploee doesn't have subordinates 
            return workerWagesClalculator.GetWages(date);
        }
    }

    public class ManagerEncreaseForSubordinates : ClalculatorDecorator
    {
        private readonly IWorkersRepository _workersRepository;
        private readonly IWagesCalculator _wagesCalculator;
        public ManagerEncreaseForSubordinates(Worker worker, 
            WorkerWagesClalculatorDecorator workerWagesClalculator, 
            IWorkersRepository workersRepository,
            IWagesCalculator wagesCalculator)
            : base(worker, workerWagesClalculator)
        {
            _workersRepository = workersRepository;
            _wagesCalculator = wagesCalculator;
        }

        public override decimal GetWages(DateTime date)
        {
            var subordinaries = _workersRepository.GetSubordinatesOfFirstLevel(worker.Id);
            var percent = SubordinatesIncreasePercentage.Manager;
            var SubordinatesSumSalary = subordinaries.Sum(x => _wagesCalculator.CalculateWorkersWages(date, x));
            var increaseForSubordinates = SubordinatesSumSalary * ((decimal)percent / 100);

            return workerWagesClalculator.GetWages(date)+ increaseForSubordinates;
        }
    }

    public class SalesEncreaseForSubordinates : ClalculatorDecorator
    {
        private readonly IWorkersRepository _workersRepository;
        private readonly IWagesCalculator _wagesCalculator;
        public SalesEncreaseForSubordinates(Worker worker, WorkerWagesClalculatorDecorator workerWagesClalculator, 
            IWorkersRepository workersRepository,
            IWagesCalculator wagesCalculator)
            : base(worker, workerWagesClalculator)
        {
            _workersRepository = workersRepository;
            _wagesCalculator = wagesCalculator;
        }

        public override decimal GetWages(DateTime date)
        {
            var subordinaries = _workersRepository.GetSubordinatesOfAllLevels(worker.Id);
            var percent = SubordinatesIncreasePercentage.Sales;
            var SubordinatesSumSalary = subordinaries.Sum(x => _wagesCalculator.CalculateWorkersWages(date, x));
            var increaseForSubordinates = SubordinatesSumSalary * ((decimal)percent / 100);

            return workerWagesClalculator.GetWages(date) + increaseForSubordinates;
        }
    }
}
