using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectNet;
using ExpectSshNet;
using System.Text.RegularExpressions;

namespace SshExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = "server";
            string user = "test";
            string password = "test";
            Session session = Expect.Spawn(new SshSpawnable(hostname, user, password));
            session.Expect("$ ", () => session.Send("ls -al\n"));
            session.Expect("$ ", (s) => Console.WriteLine(s));
            session.Send("uname -a\n");
            string text = "";
            session.ExpectAsync("Debian", (s) => text = s);
            bool match = new Regex(".*router.*[Dd]ebian.*").IsMatch(text);
            Console.WriteLine("match=" + match);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
