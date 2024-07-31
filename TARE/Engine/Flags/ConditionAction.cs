using System;
using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class ConditionAction : IConditionAction
    {
        private IEnumerable<IFlagCondition> _conditions;

        public Action Action { get; }

        public string Text { get; }

        public string Slug { get; }

        public ConditionAction(string slug, string text, IEnumerable<IFlagCondition> conditions, Action action)
        {
            Slug = slug; 
            Text = text;
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
