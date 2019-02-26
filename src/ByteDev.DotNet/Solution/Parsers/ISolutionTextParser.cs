namespace ByteDev.DotNet.Solution.Parsers
{
    public interface ISolutionTextParser<T>
    {
        T Parse(string slnText);
    }
}