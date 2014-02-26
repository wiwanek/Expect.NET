using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expect
{
    public interface ISpawnable
    {
        void Init();

        void Write(string command);

        string Read();

        System.Threading.Tasks.Task<string> ReadAsync();
    }
}
