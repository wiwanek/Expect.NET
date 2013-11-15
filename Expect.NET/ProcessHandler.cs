using System.Threading.Tasks;

namespace Expect
{
    internal interface ProcessHandler
    {
        void write(string command);
        Task<string> readAsync();

    }
}
