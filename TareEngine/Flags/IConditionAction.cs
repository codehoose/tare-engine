namespace TareEngine.Flags
{
    using TareEngine.Parser;

    public interface IConditionAction
    {
        public Action Action { get; }
        public string Text { get; }
        public string BlockedText { get; }
        public string Slug { get; }

        bool IsMatch(IEnumerable<Word> input);
    }
}
