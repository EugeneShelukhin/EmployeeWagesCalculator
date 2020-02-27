using System.Collections.Generic;

namespace AsposeTest.cache
{
    public interface ICustomCache<T>
    {
        void Add(long id, T wage);
        bool Get(long id, out T value);
    }

    public class CustomCache<T> : ICustomCache<T>
    {
        Dictionary<long, T> _cache { get; set; } = new Dictionary<long, T>();

        public void Add(long id, T wage)
        {
            _cache.Add(id, wage);
        }
        public bool Get(long id, out T value)
        {
            return _cache.TryGetValue(id, out value);
        }
    }
}
