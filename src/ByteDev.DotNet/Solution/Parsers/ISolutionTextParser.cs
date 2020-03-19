namespace ByteDev.DotNet.Solution.Parsers
{
    internal interface ISolutionTextParser<out TReturned>
    {
        TReturned Parse(string slnText);
    }
}