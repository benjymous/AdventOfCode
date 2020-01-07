namespace Advent
{
    public interface IPuzzle
    {
        string Name {get;}

        void Run(string input, ILogger log);
    }

    public interface ILogger
    {
        void WriteLine(string log=null);
    }
}