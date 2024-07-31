using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class FlagConditionNotSet : IFlagCondition
    {
        private readonly GameFlags _flags;
        private readonly string _flagName;

        public FlagConditionNotSet(GameFlags flags, string flagName)
        {
            _flags = flags;
            _flagName = flagName;
        }

        public bool IsMatch(IEnumerable<Word> input)
        {
            return !_flags.IsTruthy(_flagName);
        }
    }
}
