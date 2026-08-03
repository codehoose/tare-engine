namespace TareEngine.Flags
{
    using TareEngine.Parser;
    using System.Collections.Generic;
    using System;

    public class ConditionAction : IConditionAction
    {
        private IEnumerable<IFlagCondition> _conditions;

        public Action? Action { get; }

        public string Text { get; }
        public string BlockedText { get; }

        public string Slug { get; }

        public ConditionAction(string slug, string text, string blockedText, IEnumerable<IFlagCondition> conditions, Action? action)
        {
            Slug = slug; 
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
