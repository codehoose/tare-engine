namespace TARE.Engine.Parser.Matches
{
    internal class WordTypeMatch<T> : IMatch where T: Word
    {
        public bool IsMatch(Word word)
        {
            return word is T;
        }
    }
}
