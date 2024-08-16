using System.Collections;

namespace TareEngine.Parser
{
    public class WordCollection<T> : IEnumerable<Word> where T: Word
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

        public IEnumerator<Word> GetEnumerator()
        {
            return _word.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
