namespace TareEngine.Parser
{
    public class ParserDictionary
    {
        public WordCollection<DirectionWord> Directions { get; set; } = new();
        public WordCollection<VerbWord> Verbs { get; set; } = new();
        public WordCollection<NounWord> Nouns { get; set; } = new();
        public WordCollection<FillerWord> Fillers { get; set; } = new();

        public IEnumerable<Word> GetEnumerator() => Directions.Concat(Verbs).Concat(Nouns).Concat(Fillers);

        public Word FindWord(string word)
        {
            var direction = Directions.Find(word);
            if (direction != null) return direction;

            var verb = Verbs.Find(word);
            if (verb != null) return verb;

            var noun = Nouns.Find(word);
            if (noun != null) return noun;

            var filler = Fillers.Find(word);
            if (filler != null) return filler;

            return new InvalidWord(word);
        }
    }
}
