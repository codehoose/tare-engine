namespace TareEngine.Parser.Matches
{
    internal interface IMatchAction
    {
        bool IsMatch(Word first, Word second);
        ParserResult RunAction(Engine engine, Word first, Word second);
    }
}