using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal class LocationCondition : IFlagCondition
    {
        private readonly string _location;
        private TareEngine _engine;

        public LocationCondition(string location, TareEngine engine)
        {
            _location = location;
            _engine = engine;
        }
        public bool IsMatch(IEnumerable<Word> input) => _location == _engine.CurrentRoom.Slug;
    }
}
