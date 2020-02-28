using System;
using System.Collections.Generic;
using System.Text;

namespace AsposeTest.data
{
    
    public interface IDataContext
    {
        List<Worker> WorkersCollection { get; }
        long Add(Worker worker);
    }

    public class DataContext : IDataContext
    {
        private DataContext() { WorkersCollection = new List<Worker>();
            _identifiersCounter = new IdentifiersCounter();
        }

        private static DataContext instance = null;
        private readonly IIdentifiersCounter _identifiersCounter;
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

        public List<Worker> WorkersCollection { get; }
        public long Add(Worker worker)
        {
            var id = _identifiersCounter.IssueNewIdentifier();
            worker.Id = id;
            WorkersCollection.Add(worker);
            return id;
        }
    }
}
