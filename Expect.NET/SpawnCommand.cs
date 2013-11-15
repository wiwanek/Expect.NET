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
            : base(new LocalAppProcessHandler(new ProcessAdapter(new Process())))
        {
        }
    }
}
