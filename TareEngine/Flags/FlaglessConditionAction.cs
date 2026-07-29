using TareEngine.Parser;

namespace TareEngine.Flags
{
    internal class FlaglessConditionAction : IConditionAction
    {
        private IEnumerable<IFlagCondition> _conditions;
        public Action Action { get; }
        public string Text { get; }
        public string BlockedText { get; }

        public string Slug => string.Empty;

        public FlaglessConditionAction(string text, string blockedText, IEnumerable<IFlagCondition> conditions, Action action)
        {
            Text = text;
            BlockedText = blockedText;
            _conditions = conditions;
            Action = action;
        }

        public bool IsMatch(IEnumerable<Word> input)
        {
            bool isMatch = true;
            foreach (var condition in _conditions)
            {
                isMatch &= condition.IsMatch(input);
            }
            return isMatch;
        }
    }
}
