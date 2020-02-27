using AsposeTest.data;
using System.Linq;
using System.Threading;

namespace AsposeTest
{
    public interface IIdentifiersCounter
    {
        long IssueNewIdentifier();
    }

    public class IdentifiersCounter : IIdentifiersCounter
    {
        public IdentifiersCounter(IDataContext context)
        {
            counter = context.WorkersCollection.Count > 0 ? context.WorkersCollection.Max(w => w.Id) : 0;
        }

        private long counter;

        public long IssueNewIdentifier() => Interlocked.Increment(ref counter);
    }
}
