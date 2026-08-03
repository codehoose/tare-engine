namespace TareEngine.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDictionary
    {
        public WordCollection<DirectionWord> Directions { get; set; } = new WordCollection<DirectionWord>();
        public WordCollection<VerbWord> Verbs { get; set; } = new WordCollection<VerbWord>();
        public WordCollection<NounWord> Nouns { get; set; } = new WordCollection<NounWord>();
        public WordCollection<FillerWord> Fillers { get; set; } = new WordCollection<FillerWord>();
        public WordCollection<MetaWord> Metas { get; set; } = new WordCollection<MetaWord>();

        public IEnumerable<Word> GetEnumerator() => Directions.Concat(Verbs).Concat(Nouns).Concat(Fillers);

        public Type[] WordTypes = new Type[]
        {
            typeof(DirectionWord),
            typeof(VerbWord),
            typeof(NounWord),
            typeof(FillerWord),
            typeof(MetaWord)
        };

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

            var meta = Metas.Find(word);
            if (meta != null) return meta;

            return new InvalidWord(word);
        }
    }
}
