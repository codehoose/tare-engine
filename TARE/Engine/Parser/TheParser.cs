using SharpDX.MediaFoundation;
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

        private IEnumerable<Word> CreateWords(string[] words)
        {
            return words.Select(word => new Word(word));
        }

        public ParserResult Parse(string input, out List<Word> tokens)
        {
            // Tokenize the input and remove the filler words like 'the' and 'a'
            var words = input.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                              .Where(w => _dictionary.FindWord(w) is not FillerWord)
                              .ToList();

            // find the double words
            int index = 0;
            while(index < words.Count)
            {
                if (index == words.Count) break;
                var tmp = index < words.Count - 1 ? $"{words[index]} {words[index + 1]}" : words[index];
                var word = _dictionary.FindWord(tmp);
                if (word != null && word is not InvalidWord)
                {
                    if (index < words.Count) words.RemoveAt(index);
                    if (index < words.Count) words.RemoveAt(index);
                    words.Insert(index, word.Primary);
                }
                index++;
            }

            tokens = words.Select(_dictionary.FindWord)
                          .Where(IsValidWord)
                          .ToList();

            if (tokens.Count(w => w is InvalidWord) > 0) return ParserResult.Error;

            if (tokens.Count == 0) return ParserResult.Error;
            if (tokens.Count == 1) return _engine.PatternMatch(tokens[0], null);

            if (tokens.Count >= 0 && tokens.Count <= 2)
            {
                return _engine.PatternMatch(tokens[0], tokens[1]);
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
