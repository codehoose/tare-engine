using System;
using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Models
{
    internal class Item
    {
        public string Slug { get; }
        public string Name { get; }
        public string Description { get; }
        public string Examine { get; }

        public NounWord Word { get; }

        public ObjectFlags Flags { get; }

        public Item(string slug, string description, string examine, string name, List<string> words, IEnumerable<string> flags)
        {
            Slug = slug;
            Description = description;
            Examine = examine;
            Name = name;

            if (words?.Count == 1)
            {
                Word = new NounWord(words[0]);
            }
            else if (words?.Count > 1)
            {
                string firstWord = words[0];
                string[] sub = new string[words.Count - 1];
                string[] src = words.ToArray();
                Array.Copy(src, 1, sub, 0, words.Count - 1);
                Word = new NounWord(firstWord, sub);
            }

            Flags = ObjectFlags.None;
            if (flags == null) return;
            foreach(var flag in flags)
            {
                ObjectFlags f = Enum.Parse<ObjectFlags>(flag, true);
                Flags |= f;
            }
        }
    }
}
