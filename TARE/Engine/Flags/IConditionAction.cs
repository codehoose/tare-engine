using System;
using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal interface IConditionAction
    {
        public Action Action { get; }
        public string Text { get; }
        public string Slug { get; }

        bool IsMatch(IEnumerable<Word> input);
    }
}
