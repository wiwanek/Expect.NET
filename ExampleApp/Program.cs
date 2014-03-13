using Expect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ExampleApp");
                ISession spawn = Expect.Expect.Spawn(new ProcessSpawnable("cmd.exe"));
                spawn.Expect(">", s => Console.WriteLine("got: " + s));
                spawn.Send("dir c:\\\n");
                spawn.Expect("Program", (s) => Console.WriteLine("found: " + s));
                spawn.Send("asdsdf\n");
                spawn.Expect(">", (s) => Console.WriteLine("found: " + s));
                spawn.Send("cd c:\\\n");
                spawn.Expect(@">", s => spawn.Send("cd Users\n"));
                spawn.Expect(@"c:\\Users>", s => Console.WriteLine("done\n" + s));

                // Expect timeouts examples
                spawn.Send("ping 8.8.8.8\n");
                try
                {
                    spawn.Expect("Ping statistics", s => Console.WriteLine(s));
                }
                catch (System.TimeoutException)
                {
                    Console.WriteLine("Timeout 8.8.8.8!");
                }
                spawn.SetTimeout(5000);
                spawn.Send("ping 8.8.4.4\n");
                try
                {
                    spawn.Expect("Ping statistics for 8.8.4.4", s => Console.WriteLine(s));
                    for (int i = 0; i < 6; i++)
                    {
                        Console.WriteLine(i);
                        Thread.Sleep(1000);
                    }
                }
                catch (System.TimeoutException)
                {
                    Console.WriteLine("Timeout 8.8.4.4!");
                }

                Console.WriteLine("Using ExpectAsync");
                spawn.Send("ping 8.8.8.8\n");
                spawn.Send("ping google.com\n");
                try
                {
                    spawn.ExpectAsync("Ping statistics for 8.8.8.8", s => Console.WriteLine(s));
                    for (int i = 0; i < 6; i++)
                    {
                        Console.WriteLine(i);
                        Thread.Sleep(1000);
                    }
                    spawn.ExpectAsync("Ping statistics for", s => Console.WriteLine(s));
                    for (int i = 0; i < 6; i++)
                    {
                        Console.WriteLine(i);
                        Thread.Sleep(1000);
                    }
                
                }
                catch (System.TimeoutException)
                {
                    Console.WriteLine("Timeout 8.8.8.8!");
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
