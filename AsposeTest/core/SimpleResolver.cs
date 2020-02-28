using AsposeTest.cache;
using AsposeTest.data;
using System;
using System.Collections.Generic;
using System.Text;

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
            var subordinatesCache = new CustomCache<Worker[]>();
            return new WorkersRepository(dataContext, subordinatesCache);
        }

        public WagesCalculator ResolveWagesCalculator()
        {
            var workerWagesCache = new CustomCache<decimal>();
            return new WagesCalculator(ResolveRepository(), workerWagesCache);
        }
    }
}
