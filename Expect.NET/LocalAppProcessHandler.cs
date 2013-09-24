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

        internal LocalAppProcessHandler(string startApplicationCommand, string arguments)
        {
            //throw new NotImplementedException();
        }

        public LocalAppProcessHandler(string startApplicationCommand) : this(startApplicationCommand, "")
        {
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
