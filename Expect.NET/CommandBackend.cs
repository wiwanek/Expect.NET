using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal class CommandBackend : IBackend
    {
        private Process process;

        internal CommandBackend(Process process)
        {
            if (process.StartInfo.FileName == null || process.StartInfo.FileName.Length == 0)
            {
                throw new ArgumentException("FileName cannot be empty string", "process.StartInfo.FileName");
            }

            this.process = process;
       }

        public void write(string command)
        {
            process.StandardInput.Write(command);
        }

        public async Task<string> readAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            tasks.Add(process.StandardError.ReadLineAsync());
            tasks.Add(process.StandardOutput.ReadLineAsync());

            Task<string> ret = await Task<string>.WhenAny<string>(tasks);
            return await ret;
        }
    }
}
