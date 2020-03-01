using AsposeTest.cache;
using AsposeTest.data;

namespace AsposeTest.core
{
    //sorry for this class
    public interface ISimpleResolver
    {
        WorkersRepository ResolveRepository();
        WagesCalculator ResolveWagesCalculator();
    }

    public class SimpleResolver : ISimpleResolver
    {
        public WorkersRepository ResolveRepository()
        {
            var dataContext = DataContext.Instance;
            return new WorkersRepository(dataContext);
        }

        public WagesCalculator ResolveWagesCalculator()
        {
            var workerWagesCache = new CustomCache<decimal>();
            return new WagesCalculator(ResolveRepository(), workerWagesCache);
        }
    }
}
