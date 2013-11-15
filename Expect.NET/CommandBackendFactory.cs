using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    class CommandBackendFactory : IBackendFactory
    {
        private string command;
        private string arguments;

        internal CommandBackendFactory(string command, string arguments)
        {
            this.command = command;
            this.arguments = arguments;
        }

        internal CommandBackendFactory(string command) : this(command, "")
        {
        }

        public IBackend createBackend()
        {
            Process process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            return new CommandBackend(process);
        }
    }
}
