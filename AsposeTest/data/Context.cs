using System.Collections.Generic;
using System.Threading;

namespace AsposeTest.data
{

    public interface IDataContext
    {
        List<Worker> WorkersCollection { get; }
        Dictionary<long, List<Worker>> SubordinatesCache { get; }
        long Add(Worker worker);
    }

    public class DataContext : IDataContext
    {
        private DataContext()
        {
            WorkersCollection = new List<Worker>();
            SubordinatesCache = new Dictionary<long, List<Worker>>();
            _identifiersCounter = new IdentifiersCounter();
        }

        private static DataContext instance = null;
        private readonly IIdentifiersCounter _identifiersCounter;
        private static readonly object Singletonlock = new object();

        public static DataContext Instance
        {
            get
            {

                lock (Singletonlock)
                {
                    return instance ??= new DataContext();
                }
            }
        }


        public List<Worker> WorkersCollection { get; }
        public Dictionary<long, List<Worker>> SubordinatesCache { get; }
        public long Add(Worker worker)
        {
            var id = _identifiersCounter.IssueNewIdentifier();
            worker.Id = id;
            WorkersCollection.Add(worker);
            AddSubrdinatesToCache(worker);
            return id;
        }

        private void AddSubrdinatesToCache(Worker worker)
        {

            if (!worker.ChiefId.HasValue)
            {
                return;
            }

            if (SubordinatesCache.TryGetValue(worker.ChiefId.Value, out var collection))
            {
                collection.Add(worker);
            }
            else
            {
                SubordinatesCache.Add(worker.ChiefId.Value, new List<Worker>() { worker });
            }

        }
    }
}
