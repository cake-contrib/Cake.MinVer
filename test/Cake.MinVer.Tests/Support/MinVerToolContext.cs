using System.Threading;

namespace Cake.MinVer.Tests.Support
{
    internal class MinVerToolContext
    {
        private int _executionSequence;

        public int GetExecutionOrder()
        {
            return Interlocked.Increment(ref _executionSequence);
        }
    }
}
