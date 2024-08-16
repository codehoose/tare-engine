namespace TareEngine.Parser.Matches
{
    public class WordTypeMatch<T> : IMatch where T: Word
    {
        public bool IsMatch(Word word)
        {
            return word is T;
        }
    }
}
