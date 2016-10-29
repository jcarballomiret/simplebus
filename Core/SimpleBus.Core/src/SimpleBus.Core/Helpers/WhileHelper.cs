using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBus.Core.Helpers
{
    public class WhileHelper
    {
        public static void While(ParallelOptions parallelOptions,
            Func<bool> condition,
            Action<ParallelLoopState> body)
        {
            Parallel.ForEach(new InfinitePartitioner(), parallelOptions,
                (ignored, loopState) =>
                {
                    if (condition()) body(loopState);
                    else loopState.Stop();
                });
        }
    }
}
