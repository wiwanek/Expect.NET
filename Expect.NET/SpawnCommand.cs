using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    public class SpawnCommand : Spawn
    {
        public SpawnCommand(string command)
            : base(new CommandBackendFactory(command))
        {
        }

        public SpawnCommand(string command, string arguments)
            : base(new CommandBackendFactory(command, arguments))
        {
        }

    }
}
