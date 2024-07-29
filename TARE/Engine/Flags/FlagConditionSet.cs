using System;
using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class FlagConditionSet : IFlagConditionSet
    {
        private string _text;
        private string _hint;
        private Action _action;
        private IEnumerable<IFlagCondition> _conditions;

        public Action Action => _action;

        public string Text => _text;
        public string Hint => _hint;

        public string Slug { get; }

        public FlagConditionSet(string slug, string text, string hint, IEnumerable<IFlagCondition> conditions, Action action)
        {
            Slug = slug; 
            _text = text;
            _hint = hint;
            _conditions = conditions;
            _action = action;
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
