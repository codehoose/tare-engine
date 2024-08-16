using TareEngine.Parser;

namespace TareEngine.Flags
{
    public interface IConditionAction
    {
        public Action Action { get; }
        public string Text { get; }
        public string Slug { get; }

        bool IsMatch(IEnumerable<Word> input);
    }
}
