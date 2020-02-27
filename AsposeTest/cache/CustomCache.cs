using System.Collections.Generic;
using System.Threading;

namespace AsposeTest.cache
{
    public interface ICustomCache<T>
    {
        void Add(long id, T wage);
        bool Get(long id, out T value);
    }

    public class CustomCache<T> : ICustomCache<T>
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        Dictionary<long, T> _cache { get; set; } = new Dictionary<long, T>();

        public void Add(long id, T value)
        {
            cacheLock.EnterWriteLock();
            try
            {
                _cache.TryAdd(id, value);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public bool Get(long id, out T value)
        {
            cacheLock.EnterReadLock();
            var isExists = _cache.TryGetValue(id, out value);
            cacheLock.ExitReadLock();
            return isExists;
        }

        ~CustomCache()
        {
            if (cacheLock != null) cacheLock.Dispose();
        }
    }
}
