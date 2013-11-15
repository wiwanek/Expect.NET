using System.Diagnostics;

namespace Expect
{
    public static class SpawnFactory
    {
        public static Spawn spawnCommand(string command) {
            return spawnCommand(command, "");                
        }
        public static Spawn spawnCommand(string command, string arguments)
        {   
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            ProcessHandler handler = new LocalAppProcessHandler(new ProcessAdapter(process));
            Spawn spawn = new Spawn(handler);
            return spawn;
        }

        public static Spawn spawnTelnet(string hostname) { return null; }
        public static Spawn spawnTelnet(string hostname, int port) { return null; }
        public static Spawn spawnSsh(string hostname) { return null; }
        public static Spawn spawnSsh(string hostname, int port) { return null; }
        public static Spawn spawnSsh(string hostname, int port, string username) { return null; }
        public static Spawn spawnSsh(string hostname, int port, string username, string password) { return null; }

    }
}
