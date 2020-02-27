using System;
using System.Collections.Generic;
using System.Text;

namespace AsposeTest.data
{
    public interface IDataContext
    {
        List<Worker> WorkersCollection { get; set; }//TODO List to concurrent dictionary
    }

    public class DataContext : IDataContext
    {
        public DataContext() => WorkersCollection = new List<Worker>();

        public List<Worker> WorkersCollection { get; set; }
    }
}
