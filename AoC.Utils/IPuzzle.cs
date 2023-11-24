namespace AoC
{
    public interface IPuzzle
    {
        void Run(string input, ILogger log);
    }

    public interface ILogger
    {
        void WriteLine(string log = null);
    }
}