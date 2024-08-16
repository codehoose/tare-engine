using TareEngine.Parser;

namespace TareEngine.Flags
{
    public interface IFlagCondition
    {
        bool IsMatch(IEnumerable<Word> input);
    }
}
