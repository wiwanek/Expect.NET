using ExpectNet;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsxpectSshNet
{
    public class SshSpawnable : ISpawnable
    {
        private StreamWriter writer;
        private StreamReader reader;
        private string server;
        private string user;
        private string password;

        SshSpawnable(string server, string user, string password)
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
            return reader.ReadLine();
            
        }

        public async Task<string> ReadAsync()
        {
            return await reader.ReadLineAsync(); 
        }

        public void Write(string command)
        {
            writer.Write(command);
            writer.Flush();
        }
    }
}
