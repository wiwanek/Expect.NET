using ExpectNet;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectSshNet
{
    public class SshSpawnable : ISpawnable
    {
        private StreamWriter writer;
        private StreamReader reader;
        private string server;
        private string user;
        private string password;

        public SshSpawnable(string server, string user, string password)
        {
            this.server = server;
            this.user = user;
            this.password = password;
        }

        public void Init()
        {
            SshClient ssh = new SshClient(server, user, password);
            ssh.Connect();
            var stream = ssh.CreateShellStream("dumb", 80, 24, 800, 600, 1024);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
        }

        public string Read()
        {
            char[] buffer = new char[1024];
            int read = reader.Read(buffer, 0, 1024);
            return new string(buffer, 0, read);
            
        }

        public async Task<string> ReadAsync()
        {
            char[] buffer = new char[1024];
            int read = await reader.ReadAsync(buffer, 0, 1024).ConfigureAwait(false);
            return new string(buffer, 0, read);
        }

        public void Write(string command)
        {
            writer.Write(command);
            writer.Flush();
        }
    }
}
