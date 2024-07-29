using System.Collections.Generic;
using System.Linq;

namespace TARE.Engine.Parser
{
    internal class TheParser
    {
        private readonly ParserDictionary _dictionary;
        private readonly TareEngine _engine;

        public ParserDictionary Dictionary => _dictionary;

        public TheParser(TareEngine engine)
        {
            _dictionary = ParserDictionaryFactory.Create();
            _engine = engine;
        }

        public Word FindWord(string word) => _dictionary.FindWord(word);

        public ParserResult Parse(string input, out List<Word> tokens)
        {
            var words = input.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(_dictionary.FindWord)
                .Where(IsValidWord)
                .ToList();

            tokens = words;

            if (words.Count(w => w is InvalidWord) > 0) return ParserResult.Error;

            if (words.Count == 0) return ParserResult.Error;
            if (words.Count == 1) return _engine.PatternMatch(words[0], null);

            if (words.Count >= 0 && words.Count <= 2)
            {
                return _engine.PatternMatch(words[0], words[1]);
            }

            return ParserResult.Error;
        }

        private bool IsValidWord(Word word)
        {
            //if (word == null) return false;


            return true;
        }
    }
}
