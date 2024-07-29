using System;
using System.Collections.Generic;
using System.Linq;
using TARE.Engine.Parser;
using TARE.Engine.Serialization;

namespace TARE.Engine.Flags
{
    internal class GameFlags
    {
        private readonly Dictionary<string, int> _flags = new();
        private readonly TareEngine _engine;
        private readonly List<IFlagConditionSet> _setConditions = new();

        public bool IsTruthy(string flagName)
        {
            if (string.IsNullOrEmpty(flagName)) return true;
            return _flags.ContainsKey(flagName) && _flags[flagName] > 0;
        }

        public int GetValue(string flagName)
        {
            _flags.TryGetValue(flagName, out int value);
            return value;
        }

        public string GetHint(string flagName)
        {
            return _setConditions.FirstOrDefault(s => s.Slug == flagName)?.Hint;
        }

        public void Run(IEnumerable<Word> words)
        {
            foreach (var cond in _setConditions)
            {
                if (cond.IsMatch(words)) cond.Action();
            }
        }

        public GameFlags(TareEngine engine)
        {
            _engine = engine;
            LoadFlags();
        }

        private void LoadFlags()
        {
            var flags = FlagsSerializer.ReadFlags("flags.json");
            foreach (var flag in flags.flags)
            {
                // Can be duplicates because the slug can appear for TAKE X and DROP X
                _flags.TryAdd(flag.slug, 0);
                AddConditions(flag.slug, flag.set);
            }
        }

        private void AddConditions(string slug, SerializedFlagSet set)
        {
            Action action = null;
            var type = string.IsNullOrEmpty(set.type) ? "set" : set.type;
            switch(set.type)
            {
                case "set":
                    action = () => _flags[slug] = 1;
                    break;
                case "reset":
                    action = () => _flags[slug] = 0;
                    break;
            }

            List<IFlagCondition> conditions = new List<IFlagCondition>();
            if (!string.IsNullOrEmpty(set.location)) conditions.Add(new LocationCondition(set.location, _engine));
            if (!string.IsNullOrEmpty(set.verb)) conditions.Add(new WordMatchCondition(_engine.Parser.Dictionary.FindWord(set.verb)));
            if (!string.IsNullOrEmpty(set.noun)) conditions.Add(new WordMatchCondition(_engine.Parser.Dictionary.FindWord(set.noun)));
            if (!string.IsNullOrEmpty(set.carry)) conditions.Add(new CarryCondition(set.carry, _engine));

            var cond = new FlagConditionSet(slug, set.text, set.hint, conditions, action);
            _setConditions.Add(cond);
        }
    }
}
