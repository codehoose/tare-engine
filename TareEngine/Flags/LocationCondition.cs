using TareEngine.Parser;

namespace TareEngine.Flags
{
    public class LocationCondition : IFlagCondition
    {
        private readonly string _location;
        private Engine _engine;

        public LocationCondition(string location, Engine engine)
        {
            _location = location;
            _engine = engine;
        }
        public bool IsMatch(IEnumerable<Word> input) => _location == _engine.CurrentRoom.Slug;
    }
}
