using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ExpectNet
{
    class Session : ISession
    {
        private ISpawnable _spawnable;
        private string _output;
        private int _timeout = 2500;

        internal Session(ISpawnable spawnable)
        {
            _spawnable = spawnable;
        }

        public void Send(string command)
        {
            _spawnable.Write(command);
        }

        public void Expect(string query, ExpectedHandler handler)
        {
            Expect(query, (s) => handler());
        }

        public void Expect(string query, ExpectedHandlerWithOutput handler)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;
            _output = "";
            bool expectedQueryFound = false;
            Task task = Task.Factory.StartNew(() =>
            {
                while (!ct.IsCancellationRequested && !expectedQueryFound)
                {
                    _output += _spawnable.Read();
                    expectedQueryFound = Regex.Match(_output, query).Success;
                }
            }, ct);
            if (task.Wait(_timeout, ct))
            {
                handler(_output);
            }
            else
            {
                tokenSource.Cancel();
                throw new TimeoutException();
            }

        }
        public int Timeout
        {

            get { return _timeout; }


            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Value must be larger than zero");
                }
                _timeout = value;
            }

        }

        public async Task ExpectAsync(string query, ExpectedHandler handler)
        {
            await ExpectAsync(query, s => handler()).ConfigureAwait(false);
        }

        public async Task ExpectAsync(string query, ExpectedHandlerWithOutput handler)
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
                Task<string> task = _spawnable.ReadAsync();
                IList<Task> tasks = new List<Task>();
                tasks.Add(task);
                if (timeoutTask != null)
                {
                    tasks.Add(timeoutTask);
                }
                Task any = await Task.WhenAny(tasks).ConfigureAwait(false);
                if (task == any)
                {
                    _output += await task.ConfigureAwait(false);
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
    }
}
