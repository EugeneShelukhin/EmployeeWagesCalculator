using System;
using System.Linq;

namespace AsposeTest
{
    public enum RolesEnum
    {
        Employee,
        Manager,
        Sales
    }

    public interface IWorker
    {
        long Id { get; set; }
        string Name { get; set; }
        DateTime DateOfEmployment { get; set; }
        Decimal BasicWageRate { get; set; }
        long? ChiefId { get; set; }
        RolesEnum Role { get; set; }
        decimal CalculateWages(DateTime date);
    }



    public class Worker : IWorker
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public decimal BasicWageRate { get; set; }
        public long? ChiefId { get; set; }
        public RolesEnum Role { get; set; }

        public decimal CalculateWages(DateTime date)
        {
            switch (Role)
            {
                case RolesEnum.Employee:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Employee * ((date - DateOfEmployment).TotalDays / 365);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Employee ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;
                        return BasicWageRate + BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }

                    break;
                case RolesEnum.Manager:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Manager * ((date - DateOfEmployment).TotalDays / 365);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Manager ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;

                        var SubordinatesSumSalary = WorkersTree.GetSubordinatesOfFirstLevel(Id).Sum(x => x.CalculateWages(date));
                        var increaseForSubordinates = SubordinatesSumSalary * (decimal)SubordinatesIncreasePercentage.Manager;
                        return BasicWageRate + BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }

                    break;
                case RolesEnum.Sales:
                    {
                        var annualIncreasePercentage = AnnualIncreasePercentage.Sales * ((DateTime.Now - DateOfEmployment).TotalDays / 365);
                        annualIncreasePercentage = annualIncreasePercentage < MaxAnnualIncreasePercentage.Sales ? annualIncreasePercentage : MaxAnnualIncreasePercentage.Employee;

                        var SubordinatesSumSalary = WorkersTree.GetSubordinatesOfAllLevels(Id).Sum(x => x.CalculateWages(date));
                        var increaseForSubordinates = SubordinatesSumSalary * (decimal)SubordinatesIncreasePercentage.Sales;
                        return BasicWageRate + BasicWageRate * (decimal)annualIncreasePercentage / 100;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }





}
