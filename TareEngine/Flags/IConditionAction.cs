namespace TareEngine.Flags
{
    using TareEngine.Parser;
    using System.Collections.Generic;
    using System;

    public interface IConditionAction
    {
        public Action? Action { get; }
        public string Text { get; }
        public string BlockedText { get; }
        public string Slug { get; }

        bool IsMatch(IEnumerable<Word> input);
    }
}
