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
        public void send(string command) { process.write(command); }
        public void expect(string query, ExpectedHandler handler) 
        {
            Match match = Regex.Match(process.readAsync().Result, query);
            if (match.Success)
            {
                handler();
            }
        }

        private Process process;
    }
}
