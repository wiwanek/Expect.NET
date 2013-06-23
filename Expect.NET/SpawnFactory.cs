using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    public static class SpawnFactory
    {
        public static Spawn spawnCommand(string command) { return null; }
        public static Spawn spawnTelnet(string hostname) { return null; }
        public static Spawn spawnTelnet(string hostname, int port) { return null; }
        public static Spawn spawnSsh(string hostname) { return null; }
        public static Spawn spawnSsh(string hostname, int port) { return null; }
        public static Spawn spawnSsh(string hostname, int port, string username) { return null; }
        public static Spawn spawnSsh(string hostname, int port, string username, string password) { return null; }

    }
}
