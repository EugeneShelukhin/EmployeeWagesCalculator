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
        public IdentifiersCounter(long startFrom=0)
        {
            counter = startFrom;
        }

        private long counter;

        public long IssueNewIdentifier() => Interlocked.Increment(ref counter);
    }
}
