using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal class CommandSpawnable : ISpawnable
    {
        private Process _process;
        private Task<string> _errorRead = null;
        private Task<string> _stdRead = null;
               
            
        internal CommandSpawnable(Process process)
        {
            if (process.StartInfo.FileName == null || process.StartInfo.FileName.Length == 0)
            {
                throw new ArgumentException("FileName cannot be empty string", "_process.StartInfo.FileName");
            }

            _process = process;
            
       }

        public void Init() { }

        public void Write(string command)
        {
            if (_errorRead == null || _errorRead.IsCanceled || _errorRead.IsCompleted || _errorRead.IsFaulted)
            {
                _process.StandardError.DiscardBufferedData();
            }
            if (_stdRead == null || _stdRead.IsCanceled || _stdRead.IsCompleted || _stdRead.IsFaulted)
            {
                _process.StandardOutput.DiscardBufferedData();
            }
            _process.StandardInput.Write(command);
        }

        public async Task<string> ReadAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            RecreateErrorReadTask();
            RecreateStdReadTask();
            tasks.Add(_errorRead);
            tasks.Add(_stdRead);

            var ret = await Task<string>.WhenAny<string>(tasks).ConfigureAwait(false);
            return await ret.ConfigureAwait(false);
        }

        private void RecreateErrorReadTask()
        {
            if (_errorRead == null || _errorRead.IsCanceled || _errorRead.IsCompleted || _errorRead.IsFaulted)
            {
                char[] tmp = new char[256];
                _errorRead = CreateStringAsync(tmp, _process.StandardError.ReadAsync(tmp, 0, 256));
            }
        }

        private void RecreateStdReadTask()
        {
            if (_stdRead == null || _stdRead.IsCanceled || _stdRead.IsCompleted || _stdRead.IsFaulted)
            {
                char[] tmp = new char[256];
                _stdRead = CreateStringAsync(tmp, _process.StandardOutput.ReadAsync(tmp, 0, 256));
            }
        }

        private async Task<string> CreateStringAsync(char[] c, Task<int> n)
        {
            return new string(c, 0, await n.ConfigureAwait(false)); 
        }


        public string Read()
        {
            char[] tmp = new char[256];
            int n = _process.StandardOutput.Read(tmp, 0, 256);
            return new string(tmp, 0, n);
        }
    }
}
