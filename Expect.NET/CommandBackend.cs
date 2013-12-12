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
        private Task<string> errorRead = null;
        private Task<string> stdRead = null;
               
            
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
            if (errorRead == null || errorRead.IsCanceled || errorRead.IsCompleted || errorRead.IsFaulted)
            {
                process.StandardError.DiscardBufferedData();
            }
            if (stdRead == null || stdRead.IsCanceled || stdRead.IsCompleted || stdRead.IsFaulted)
            {
                process.StandardOutput.DiscardBufferedData();
            }
            process.StandardInput.Write(command);
        }

        public async Task<string> readAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            RecreateErrorReadTask();
            RecreateStdReadTask();
            tasks.Add(errorRead);
            tasks.Add(stdRead);

            var ret = await Task<string>.WhenAny<string>(tasks);
            return await ret;
        }

        private void RecreateErrorReadTask()
        {
            if (errorRead == null || errorRead.IsCanceled || errorRead.IsCompleted || errorRead.IsFaulted)
            {
                char[] tmp = new char[256];
                errorRead = CreateStringAsync(tmp, process.StandardError.ReadAsync(tmp, 0, 256));
            }
        }

        private void RecreateStdReadTask()
        {
            if (stdRead == null || stdRead.IsCanceled || stdRead.IsCompleted || stdRead.IsFaulted)
            {
                char[] tmp = new char[256];
                stdRead = CreateStringAsync(tmp, process.StandardOutput.ReadAsync(tmp, 0, 256));
            }
        }

        private async Task<string> CreateStringAsync(char[] c, Task<int> n)
        {
            return new string(c, 0, await n); 
        }
    }
}
