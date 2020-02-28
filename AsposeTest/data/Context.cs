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
        private DataContext() => WorkersCollection = new List<Worker>();

        private static DataContext instance = null;
        private static readonly object threadlock = new object();

        public static DataContext Instance
        {
            get
            {

                lock (threadlock)
                {
                    return instance ??= new DataContext();
                }
            }
        }



        public List<Worker> WorkersCollection { get; set; }
    }
}
