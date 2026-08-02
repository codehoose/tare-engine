namespace TareEngine.Flags
{
    using TareEngine.Parser;

    public class WordMatchCondition : IFlagCondition
    {
        private Word _word;

        public WordMatchCondition(Word word) => _word = word;

        public bool IsMatch(IEnumerable<Word> input)
        {
            return input.Any(w => w.Primary.Equals(_word.Primary));
        }
    }
}
