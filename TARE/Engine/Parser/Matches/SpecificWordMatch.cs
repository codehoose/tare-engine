namespace TARE.Engine.Parser.Matches
{
    internal class SpecificWordMatch : IMatch
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
