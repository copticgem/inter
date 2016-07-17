using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    static class SynchronizationHelper
    {
        private static int x = 0;

        public static async Task ExecuteOnce(Task action)
        {
            int originalValue = Interlocked.CompareExchange(ref x, 1, 0);
            if (originalValue == 0)
            {
                // First time, execute
                try
                {
                    await action;
                }
                finally
                {
                    // Set back x
                    x = 0;
                }
            }
        }
    }
}
