using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal class LocalAppProcessHandler : ProcessHandler
    {
        internal Process Process { get; private set; }

        internal LocalAppProcessHandler(Process process)
        {
            throw new NotImplementedException();
        }

        public void write(string command)
        {
            throw new NotImplementedException();
        }

        public Task<string> readAsync()
        {
            throw new NotImplementedException();
        }
    }
}
