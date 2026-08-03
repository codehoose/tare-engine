namespace TareEngine.Parser
{
    public class NoWord : MetaWord
    {
        private static NoWord _instance;

        public static NoWord Instance
        {
            get
            {
                if (_instance == null) _instance = new NoWord();
                return _instance;
            }
        }

        private NoWord() : base("-")
        {
        }
    }
}
