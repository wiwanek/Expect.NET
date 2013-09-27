using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    // Stupid class to disable Start function in tests...
    internal class ProcessWrapper
    {
        internal virtual Process Process { get; set; }
        internal ProcessWrapper() { }
        internal ProcessWrapper(Process process)
        {
            Process = process;
        }

        internal virtual bool Start() { return Process.Start(); }
    }
}
