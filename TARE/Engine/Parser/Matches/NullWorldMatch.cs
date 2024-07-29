namespace TARE.Engine.Parser.Matches
{
    internal class NullWorldMatch : IMatch
    {
        public bool IsMatch(Word word)
        {
            return word == null;            
        }
    }
}
