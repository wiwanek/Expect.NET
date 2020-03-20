using System;
using ExpectNet;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.LinuxExample();
        }

        public void LinuxExample()
        {
            try
            {
                Console.WriteLine("ExampleApp");
                Session spawn = Expect.Spawn(new ProcessSpawnable("pwsh"));
                spawn.Expect(">", s => Console.WriteLine(""));
                spawn.Timeout = 60000;
                spawn.Send("ping 8.8.8.8 -c 4\n");
                string output = string.Empty;
                spawn.Expect("ping statistics", s => output = s);
                Console.WriteLine(output);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
