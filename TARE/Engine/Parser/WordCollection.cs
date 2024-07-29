using System.Collections.Generic;
using System.Linq;

namespace TARE.Engine.Parser
{
    internal class WordCollection<T> where T: Word
    {
        public static string NotFound = nameof(NotFound);
        private readonly List<T> _word = new List<T>();

        public void Add(T word)
        {
            _word.Add(word);
        }

        public bool IsValid(string word)
        {
            var found = _word.FirstOrDefault(w => w.IsMatch(word));
            return found != null;
        }

        public Word Find(string word)
        {
            return _word.FirstOrDefault(w => w.IsMatch(word));
        }
    }
}
