using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Expect
{
    public class Spawn
    {
        internal Spawn(IBackendFactory backendFactory)
        {
            this.backend = backendFactory.createBackend();
        }

        public delegate void ExpectedHandler();
        public delegate void ExpectedHandlerWithOutput(string output);
        public void send(string command) { backend.write(command); }
        async public Task expect(string query, ExpectedHandler handler)
        {
            await expect(query, (s) => handler());
        }
        async public Task expect(string query, ExpectedHandlerWithOutput handler) 
        {
            Task timeoutTask = null;
            if (timeout > 0)
            {
                timeoutTask = Task.Delay(timeout);
            }
            output = "";
            bool expectedQueryFound = false;
            while (!expectedQueryFound)
            {
                Task<string> task = backend.readAsync();
                IList<Task> tasks = new List<Task>();
                tasks.Add(task);
                if (timeoutTask != null)
                {
                    tasks.Add(timeoutTask);
                }
                if (task == await Task.WhenAny(tasks))
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

        public int getTimeout()
        {
            return timeout;
        }

        public void setTimeout(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException("timeout", "Value must be larger than zero");
            }
            this.timeout = timeout;
        }

        private IBackend backend;
        private string output;
        private int timeout = 500;

    }
}
