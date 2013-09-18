using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Expect
{
    public class Spawn
    {
        internal Spawn(Process process)
        {
            this.process = process;
        }

        public delegate void ExpectedHandler();
        public delegate void ExpectedHandlerWithOutput(string output);
        public void send(string command) { process.write(command); }
        async public Task expect(string query, ExpectedHandler handler)
        {
            await expect(query, (s) => handler());
        }
        async public Task expect(string query, ExpectedHandlerWithOutput handler) 
        {
            Task timeout = Task.Delay(500);
            output = "";
            bool expectedQueryFound = false;
            while (!expectedQueryFound)
            {
                Task<string> task = process.readAsync();
                if (task == await Task.WhenAny(task, timeout))
                {
                    output += await task;
                    expectedQueryFound = Regex.Match(output, query).Success;
                    if (expectedQueryFound)
                    {
                        handler(output);
                    }
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }

        private Process process;
        private string output;
    }
}
