using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    class SpawnCommand : Spawn
    {
        SpawnCommand(string command)
            : base(new CommandBackendFactory(command))
        {
        }

        SpawnCommand(string command, string arguments)
            : base(new CommandBackendFactory(command, arguments))
        {
        }

    }
}
