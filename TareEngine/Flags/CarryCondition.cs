using TareEngine.Parser;

namespace TareEngine.Flags
{
    public class CarryCondition : IFlagCondition
    {
        private readonly string _item;
        private readonly Engine _engine;

        public CarryCondition(string item, Engine engine)
        {
            _item = item;
            _engine = engine;
        }

        public bool IsMatch(IEnumerable<Word> input)
        {
            return _engine.Inventory.Count(i => i.Slug == _item) >= 0;
        }
    }
}
