using System.Diagnostics;
using System.IO;

namespace ExpectNet
{
    interface IProcess
    {
        ProcessStartInfo StartInfo { get; }
        StreamReader StandardOutput { get; }
        StreamReader StandardError { get; }
        StreamWriter StandardInput { get; }

        void Start();
    }
}
