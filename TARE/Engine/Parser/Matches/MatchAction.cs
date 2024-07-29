using System;
using System.Collections.Generic;

namespace TARE.Engine.Parser.Matches
{
    internal class MatchAction : IMatchAction
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

        public ParserResult RunAction(TareEngine engine, Word first, Word second)
        {
            if (_first.IsMatch(first) && _second.IsMatch(second))
            {
                return _action(new Word[] { first, second });
            }

            return ParserResult.Error;
        }
    }
}
