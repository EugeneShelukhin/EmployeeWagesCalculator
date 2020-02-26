using AsposeTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public abstract class TestBase
    {
        public void LoadWorkers() {
            var johnId=WorkersTree.AddManager(null, "John Doe");
            var janeId=WorkersTree.AddSales(johnId, "Jane Doe");
            var judiId=WorkersTree.AddSales(janeId, "Judy Doe");
            var jamesId = WorkersTree.AddManager(judiId, "James Doe");
            WorkersTree.AddEmployee(jamesId, "Ivan Ivanov");
            WorkersTree.AddEmployee(jamesId, "Piter Peterson");
            WorkersTree.AddEmployee(null, "mr. Independent Worker");
        } 
    }
}
