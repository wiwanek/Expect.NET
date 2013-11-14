using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal interface IProcess
    {
        ProcessStartInfo StartInfo { get; set; }
        bool Start();

        Task<string> ReadAsync();
        void Write(string command);
    }
}
