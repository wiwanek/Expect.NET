using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    // Adapter class for mocking Process in tests...
    internal class ProcessAdapter : Process
    {
        internal virtual new ProcessStartInfo StartInfo { get { return base.StartInfo; } set { base.StartInfo = value; } }
        internal virtual new bool Start() { return base.Start(); }
    }
}
