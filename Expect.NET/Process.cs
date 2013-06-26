using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    internal interface Process
    {
        void write(string command);
        Task<string> readAsync();

    }
}
