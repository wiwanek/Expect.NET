namespace ExpectNet
{
    public interface ISpawnable
    {
        void Init();

        void Write(string command);

        string Read();

        System.Threading.Tasks.Task<string> ReadAsync();
    }
}
