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
        internal IProcess Process { get; private set; }

        internal LocalAppProcessHandler(IProcess process)
        {
            if (process.StartInfo.FileName == null || process.StartInfo.FileName.Length == 0)
            {
                throw new ArgumentException("FileName cannot be empty string", "process.StartInfo.FileName");
            }

            Process = process;

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardOutput = true;

            process.Start();
        }

        public void write(string command)
        {
            Process.Write(command);
        }

        public async Task<string> readAsync()
        {
            return await Process.ReadAsync();
        }
    }
}
