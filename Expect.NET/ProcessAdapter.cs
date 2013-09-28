using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    // Stupid class to disable Start function in tests...
    internal class ProcessAdapter : Process
    {
        internal virtual ProcessStartInfo StartInfo { get; set; }
        internal virtual new bool Start() { return base.Start(); }
    }
}
