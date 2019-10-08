namespace ByteDev.DotNet.Solution.Parsers
{
    internal interface ISolutionTextParser<T>
    {
        T Parse(string slnText);
    }
}