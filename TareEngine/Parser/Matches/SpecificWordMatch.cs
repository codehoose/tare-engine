namespace TareEngine.Parser.Matches
{
    public class SpecificWordMatch : IMatch
    {
        private readonly Word _word;

        public bool IsMatch(Word word)
        {
            return word == _word;
        }

        public SpecificWordMatch(Word word)
        {
            _word = word;
        }
    }
}
