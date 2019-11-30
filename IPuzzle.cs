namespace Advent
{
    public interface IPuzzle
    {
        string Name {get;}

        void Run(string input);
    }
}