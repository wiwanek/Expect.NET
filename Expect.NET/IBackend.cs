using System.Threading.Tasks;

namespace Expect
{
    internal interface IBackend
    {
        void write(string command);
        Task<string> readAsync();

    }
}
