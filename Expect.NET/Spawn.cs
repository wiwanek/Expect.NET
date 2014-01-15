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
            _backend = backendFactory.CreateBackend();
        }

        public delegate void ExpectedHandler();
        public delegate void ExpectedHandlerWithOutput(string output);
        public void Send(string command) { _backend.Write(command); }
        public void Expect(string query, ExpectedHandler handler)
        {
            Expect(query, (s) => handler());
        }
        public void Expect(string query, ExpectedHandlerWithOutput handler) 
        {
            Task timeoutTask = null;
            if (_timeout > 0)
            {
                timeoutTask = Task.Delay(_timeout);
            }
            _output = "";
            bool expectedQueryFound = false;
            while (!expectedQueryFound)
            {
                Task<string> task = _backend.ReadAsync();
                IList<Task> tasks = new List<Task>();
                tasks.Add(task);
                if (timeoutTask != null)
                {
                    tasks.Add(timeoutTask);
                }
                Task<Task> any = Task.WhenAny(tasks);
                any.Wait();
                if (task == any.Result)
                {
                    task.Wait();
                    _output += task.Result;
                    expectedQueryFound = Regex.Match(_output, query).Success;
                    if (expectedQueryFound)
                    {
                        handler(_output);
                    }
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }

        [Obsolete("Use GetTimeout()")]
        public int getTimeout()
        {
            return GetTimeout();
        }

        public int GetTimeout()
        {
            return _timeout;
        }

        [Obsolete("Use SetTimeout()")]
        public void setTimeout(int timeout)
        {
            SetTimeout(timeout);
        }

        public void SetTimeout(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException("timeout", "Value must be larger than zero");
            }
            _timeout = timeout;
        }

        private IBackend _backend;
        private string _output;
        private int _timeout = 2500;

    }
}
