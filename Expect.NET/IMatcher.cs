using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectNet.NET
{
    interface IMatcher
    {
        bool IsMatch(string text);
    }
}
