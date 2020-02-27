using AsposeTest.enums;
using System;

namespace AsposeTest
{
    public struct Worker //struct?
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public decimal BasicWageRate { get; set; }
        public long? ChiefId { get; set; }
        public RolesEnum Role { get; set; }
    }
}