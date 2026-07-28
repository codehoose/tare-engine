using TareEngine.Parser;

namespace TareEngine.Flags
{
    public class CarryCondition : IFlagCondition
    {
        private readonly string _item;
        private readonly Engine _engine;
        private readonly bool _isNotCarried;

        public CarryCondition(string item, Engine engine)
        {
            _isNotCarried = item.StartsWith("!");
            _item = item.StartsWith("!") ? item.Substring(1) : item;
            _engine = engine;
        }

        public bool IsMatch(IEnumerable<Word> input)
        {
            return _isNotCarried ? _engine.Inventory.Count(i => i.Slug == _item) == 0 : _engine.Inventory.Count(i => i.Slug == _item) > 0;
        }
    }
}
