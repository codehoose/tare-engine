using System.Collections.Generic;
using System.Linq;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class CarryCondition : IFlagCondition
    {
        private readonly string _item;
        private readonly TareEngine _engine;

        public CarryCondition(string item, TareEngine engine)
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
