using ExpectNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace Ping.Tests
{
    [TestClass()]
    public class PingTests
    {
        [TestMethod()]
        public void badOption()
        {
            //given
            Session cmd = Expect.Spawn(new ProcessSpawnable("cmd.exe"));
            cmd.Expect(">", s => Console.WriteLine("got: " + s));

            //when
            cmd.Send("ping -blabla\n");

            //then
            cmd.Expect(">", (output) =>
            {
                StringAssert.Contains(output, "Bad option -blabla");
                StringAssert.Contains(output, "Usage: ping");
                StringAssert.Matches(output, new Regex("-4.*Force using IPv4."));
            });
        }

        [TestMethod()]
        public void printUsage()
        {
            //given
            Session cmd = Expect.Spawn(new ProcessSpawnable("cmd.exe"));
            cmd.Expect(">", s => Console.WriteLine("got: " + s));

            //when
            cmd.Send("ping /?\n");

            //then
            cmd.Expect(">", (output) =>
            {
                StringAssert.DoesNotMatch(output, new Regex("Bad option"));
                StringAssert.Contains(output, "Usage: ping");
                StringAssert.Matches(output, new Regex("-4.*Force using IPv4."));
            });
        }

        [TestMethod()]
        public void ping8888()
        {
            //given
            Session cmd = Expect.Spawn(new ProcessSpawnable("cmd.exe"));
            cmd.Timeout = 5000;
            cmd.Expect(">", s => Console.WriteLine("got: " + s));

            //when
            cmd.Send("ping 8.8.8.8\n");

            //then
            cmd.Expect(">", (output) =>
            {
                StringAssert.Contains(output, "Ping statistics for 8.8.8.8");
            });
        }


    }
}