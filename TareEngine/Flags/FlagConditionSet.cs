namespace TareEngine.Flags
{
    using TareEngine.Parser;
    using System.Collections.Generic;

    public class FlagConditionSet : IFlagCondition
    {
        private readonly GameFlags _flags;
        private readonly string _flagName;

        public FlagConditionSet(GameFlags flags, string flagName)
        {
            _flags = flags;
            _flagName = flagName;
        }

        public bool IsMatch(IEnumerable<Word> input)
        {
            return _flags.IsTruthy(_flagName);
        }
    }
}
