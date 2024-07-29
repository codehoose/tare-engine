using System;
using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal interface IFlagConditionSet
    {
        public Action Action { get; }
        public string Text { get; }
        public string Slug { get; }
        public string Hint { get; }

        bool IsMatch(IEnumerable<Word> input);
    }
}
