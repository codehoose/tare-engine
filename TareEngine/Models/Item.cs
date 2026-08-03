namespace TareEngine.Models
{
    using TareEngine.Parser;
    using System.Collections.Generic;
    using System;

    public class Item
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

            if (words?.Count >= 1)
            {
                var word = words[0];
                var secondary = Array.Empty<string>();

                if (words?.Count >= 2)
                {
                    secondary = new string[words.Count - 1];
                    string[] src = words.ToArray();
                    Array.Copy(src, 1, secondary, 0, words.Count - 1);
                }
                Word = new NounWord(word, secondary);
            }
            else
            {
                throw new ArgumentException($"'{nameof(words)}' parameter must contain at least one entry");
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
