namespace TARE.Engine.Parser
{
    internal class ParserDictionary
    {
        public WordCollection<DirectionWord> Directions { get; set; } = new WordCollection<DirectionWord>();
        public WordCollection<VerbWord> Verbs { get; set; } = new WordCollection<VerbWord>();
        public WordCollection<NounWord> Nouns { get; set; } = new WordCollection<NounWord>();

        public Word FindWord(string word)
        {
            var direction = Directions.Find(word);
            if (direction != null) return direction;

            var verb = Verbs.Find(word);
            if (verb != null) return verb;

            var noun = Nouns.Find(word);
            if (noun != null) return noun;

            return new InvalidWord(word);
        }
    }
}
