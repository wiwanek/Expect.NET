using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    public abstract class Spawn
    {
        public void send(string command) { }
        public void expect(string query) { }
    }
}
