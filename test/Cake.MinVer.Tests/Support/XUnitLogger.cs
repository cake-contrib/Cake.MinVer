using System;
using Xunit.Abstractions;
using Cake.Core.Diagnostics;

namespace Cake.MinVer.Tests.Support
{
    internal class XUnitLogger : ICakeLog
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLogger(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            Verbosity = Verbosity.Diagnostic;
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if ((int)verbosity <= (int)Verbosity)
            {
                _testOutputHelper.WriteLine($"[{level}] {format}", args);
            }
        }

        public Verbosity Verbosity { get; set; }
    }
}
