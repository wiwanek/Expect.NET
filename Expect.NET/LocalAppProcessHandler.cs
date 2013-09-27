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
        internal ProcessWrapper ProcessWrapper { get; private set; }

        internal LocalAppProcessHandler(ProcessWrapper process)
        {
            if (process.Process.StartInfo.FileName == null || process.Process.StartInfo.FileName.Length == 0)
            {
                throw new ArgumentException("FileName cannot be empty string", "process.StartInfo.FileName");
            }

            ProcessWrapper = process;

            ProcessWrapper.Process.StartInfo.UseShellExecute = false;
            ProcessWrapper.Process.StartInfo.RedirectStandardInput = true;
            ProcessWrapper.Process.StartInfo.RedirectStandardError = true;
            ProcessWrapper.Process.StartInfo.RedirectStandardOutput = true;

            process.Start();
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
