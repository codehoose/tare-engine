using System;
using System.Collections.Generic;
using System.Linq;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class WordMatchCondition : IFlagCondition
    {
        private Word _word;

        public WordMatchCondition(Word word) => _word = word;

        public bool IsMatch(IEnumerable<Word> input)
        {
            return input.Contains(_word);
        }
    }
}
