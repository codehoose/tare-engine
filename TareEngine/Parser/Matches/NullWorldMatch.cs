namespace TareEngine.Parser.Matches
{
    public class NullWorldMatch : IMatch
    {
        public bool IsMatch(Word word)
        {
            return word == null;            
        }
    }
}
