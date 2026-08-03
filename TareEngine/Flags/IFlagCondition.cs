namespace TareEngine.Flags
{
    using TareEngine.Parser;
    using System.Collections.Generic;

    public interface IFlagCondition
    {
        bool IsMatch(IEnumerable<Word> input);
    }
}
