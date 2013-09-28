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
        internal ProcessAdapter ProcessAdapter { get; private set; }

        internal LocalAppProcessHandler(ProcessAdapter process)
        {
            if (process.StartInfo.FileName == null || process.StartInfo.FileName.Length == 0)
            {
                throw new ArgumentException("FileName cannot be empty string", "process.StartInfo.FileName");
            }

            ProcessAdapter = process;

            ProcessAdapter.StartInfo.UseShellExecute = false;
            ProcessAdapter.StartInfo.RedirectStandardInput = true;
            ProcessAdapter.StartInfo.RedirectStandardError = true;
            ProcessAdapter.StartInfo.RedirectStandardOutput = true;

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
