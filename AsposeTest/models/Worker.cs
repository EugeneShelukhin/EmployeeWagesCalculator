﻿using AsposeTest.enums;
using System;

namespace AsposeTest
{
    public class Worker
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public decimal BasicWageRate { get; set; }
        public long? ChiefId { get; set; }
        public RolesEnum Role { get; set; }

        public int GetWorkerExperience(DateTime date)
        {
            return (int)(date - DateOfEmployment).TotalDays / 365;
        }
    }
}