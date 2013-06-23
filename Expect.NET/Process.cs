using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    interface Process
    {
        void write(string command);
        async Task<string> readAsync();

    }
}
