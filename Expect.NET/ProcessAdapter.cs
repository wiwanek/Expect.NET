using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal class ProcessAdapter : IProcess
    {
        private Process process;

        internal ProcessAdapter(Process process)
        {
            this.process = process;
        }

        public ProcessStartInfo StartInfo { get { return process.StartInfo; } set { process.StartInfo = value; } }
        public bool Start() { return process.Start(); }

        public async Task<string> ReadAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            tasks.Add(process.StandardError.ReadLineAsync());
            tasks.Add(process.StandardOutput.ReadLineAsync());

            Task<string> ret = await Task<string>.WhenAny<string>(tasks);
            return await ret;
        }

        public void Write(string command)
        {
            process.StandardInput.Write(command);
        }
    }
}
