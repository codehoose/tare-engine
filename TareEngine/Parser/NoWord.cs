namespace TareEngine.Parser
{
    public class NoWord : MetaWord
    {
        public static NoWord Instance
        {
            get
            {
                if (field == null) field = new NoWord();
                return field;
            }
        }

        private NoWord() : base("-")
        {
        }
    }
}
