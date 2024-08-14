namespace TARE.Engine.Parser
{
    internal static class ParserDictionaryFactory
    {
        public static ParserDictionary Create()
        {
            ParserDictionary dictionary = new ParserDictionary();

            // Directions
            dictionary.Directions.Add(new DirectionWord("North", "n"));
            dictionary.Directions.Add(new DirectionWord("South", "s"));
            dictionary.Directions.Add(new DirectionWord("East", "e"));
            dictionary.Directions.Add(new DirectionWord("West", "w"));
            dictionary.Directions.Add(new DirectionWord("Down", "d"));
            dictionary.Directions.Add(new DirectionWord("Up", "u"));

            // Move
            dictionary.Verbs.Add(new VerbWord("Go"));

            // Examine
            dictionary.Verbs.Add(new VerbWord("Examine"));

            // Look
            dictionary.Verbs.Add(new VerbWord("Look"));

            // Take
            dictionary.Verbs.Add(new VerbWord("Take", "Get", "Grab", "Pick Up"));

            // Inventory
            dictionary.Verbs.Add(new VerbWord("Inventory", "I"));
            dictionary.Verbs.Add(new VerbWord("Drop"));

            return dictionary;
        }
    }
}
