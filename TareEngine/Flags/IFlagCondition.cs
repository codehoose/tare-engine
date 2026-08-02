namespace TareEngine.Flags
{
    using TareEngine.Parser;

    public interface IFlagCondition
    {
        bool IsMatch(IEnumerable<Word> input);
    }
}
