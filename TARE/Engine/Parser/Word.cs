namespace TARE.Engine.Parser
{
    internal class Word
    {
        public string Primary { get; }
        public string[] Secondaries { get; }

        public bool IsMatch(string word)
        {
            if (Primary.ToLower() == word.ToLower()) return true;

            foreach(string s in Secondaries)
            {
                if (s.ToLower() == word.ToLower()) return true;
            }

            return false;
        }

        public Word(string word, params string[] secondaries)
        {
            Primary = word;
            Secondaries = secondaries;
        }
    }
}
