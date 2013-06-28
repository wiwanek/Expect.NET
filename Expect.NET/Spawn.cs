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
        async public void expect(string query, ExpectedHandler handler) 
        {
            output = "";
            bool expectedQueryFound = false;
            while (!expectedQueryFound)
            {
                output += await process.readAsync();
                expectedQueryFound = Regex.Match(output, query).Success;
                if (expectedQueryFound)
                {
                    handler();
                }
            }
        }

        private Process process;
        private string output;
    }
}
