namespace TareEngine.Parser.Matches
{
    public class MatchAction : IMatchAction
    {
        private readonly IMatch _first;
        private readonly IMatch _second;
        private readonly Func<IEnumerable<Word>, ParserResult> _action;

        public MatchAction(IMatch first, IMatch second, Func<IEnumerable<Word>, ParserResult> action)
        {
            _first = first;
            _second = second;
            _action = action;
        }

        public bool IsMatch(Word first, Word second)
        {
            return _first.IsMatch(first) && _second.IsMatch(second);
        }

        public ParserResult RunAction(Engine engine, Word first, Word second)
        {
            if (_first.IsMatch(first) && _second.IsMatch(second))
            {
                return _action(new Word[] { first, second });
            }

            return ParserResult.Error;
        }
    }
}
