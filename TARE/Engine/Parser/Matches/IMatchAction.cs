namespace TARE.Engine.Parser.Matches
{
    internal interface IMatchAction
    {
        bool IsMatch(Word first, Word second);
        ParserResult RunAction(TareEngine engine, Word first, Word second);
    }
}