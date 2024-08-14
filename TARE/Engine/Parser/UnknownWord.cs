namespace TARE.Engine.Parser
{
    internal class UnknownWord : Word
    {
        public UnknownWord(string word, params string[] secondaries) : base(word, secondaries)
        {
        }
    }
}
