using Expect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Spawn spawn = new SpawnCommand("cmd.exe");
                spawn.expect(">", s => Console.WriteLine("got: " + s));
                spawn.send("dir c:\\\n");
                spawn.expect("Program", (s) => Console.WriteLine("found: " + s));
                spawn.send("asdsdf\n");
                spawn.expect(">", (s) => Console.WriteLine("found: " + s));
                spawn.send("cd c:\\\n");
                spawn.expect(@">", s => spawn.send("cd Users\n"));
                spawn.expect(@"c:\\Users>", s => Console.WriteLine("done\n" + s));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
