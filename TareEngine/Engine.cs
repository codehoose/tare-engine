using System;
using System.Reflection;
using System.Text;
using TareEngine.Flags;
using TareEngine.Models;
using TareEngine.Parser;
using TareEngine.Parser.Matches;
using TareEngine.Serialization;

namespace TareEngine
{
    public class Engine
    {
        private readonly TheParser _parser;
        private readonly Dictionary<string, Room> _rooms = new();
        private readonly Dictionary<string, Func<IEnumerable<Word>, ParserResult>> _methods = new();
        private readonly List<Item> _items = new();
        private readonly List<IMatchAction> _actions = new();
        private readonly List<Item> _inventory = new();
        private GameFlags _flags;

        public TheParser Parser => _parser;
        public Room CurrentRoom { get; private set; }
        public string LastMessage { get; private set; }
        public string LastError { get; private set; }
        public IList<Item> Inventory => _inventory;
        public GameFlags Flags => _flags;

        public Engine()
        {
            _parser = new TheParser(this);
        }

        public void Init()
        {
            var gameData = GameDataSerializer.GetData("thedata.json");
            LoadRooms(gameData.rooms);
            LoadItems(gameData.items);
            LoadFlags(gameData.flags);
            LoadActions(gameData.actions);
            //MakeMatches();
        }

        public ParserResult Parse(string input)
        {
            LastError = "";
            LastMessage = "";

            List<Word> tokens = new List<Word>();
            ParserResult result = _parser.Parse(input, out tokens);

            // Run through preconditions
            if (_flags.TryGetPreCondition(tokens, out var preCondition))
            {
                if (preCondition != null)
                {
                    preCondition.Action();
                    LastMessage = preCondition.Text;
                    _flags.Increment(GameFlags.PlayerMoveCount);
                    return result;
                }
            }

            if (result == ParserResult.Error && string.IsNullOrEmpty(LastError))
            {
                LastError = "I don't know what that means!";
            }
            else if (result != ParserResult.Error)
            {
                Flags.Run(tokens);
            }

            _flags.Increment(GameFlags.PlayerMoveCount);
            return result;
        }

        private ParserResult ShowError(IEnumerable<Word> _)
        {
            return ParserResult.Error;
        }

        private void LoadRooms(SerializedRoomCollection rooms)
        {
            foreach (var room in rooms.rooms.Select(CreateRoom))
            {
                _rooms.Add(room.Slug, room);
            }

            CurrentRoom = string.IsNullOrEmpty(rooms.startRoom) ? _rooms.Values.FirstOrDefault() : _rooms[rooms.startRoom];
        }

        private void LoadFlags(SerializedFlag[] flags)
        {
            _flags = new GameFlags(this, flags);
        }

        private void LoadActions(SerializedAction[] actions)
        {
            _methods.Add(nameof(ChangeRoom), ChangeRoom);
            _methods.Add(nameof(ExamineItem), ExamineItem);
            _methods.Add(nameof(TakeItem), TakeItem);
            _methods.Add(nameof(DescribeRoom), DescribeRoom);
            _methods.Add(nameof(DescribeInventory), DescribeInventory);
            _methods.Add(nameof(DropItem), DropItem);
            _methods.Add(nameof(ShowScore), ShowScore);
            _methods.Add(nameof(ShowError), ShowError);

            foreach (var action in actions)
            {
                IMatch first = GetMatch(action.words[0]);
                IMatch second = GetMatch(action.words[1]);
                Func<IEnumerable<Word>, ParserResult> func = GetFunction(action.action);
                _actions.Add(new MatchAction(first, second, func));
            }
        }

        private IMatch GetMatch(string word)
        {
            if (word.StartsWith("!"))
            {
                return new SpecificWordMatch(_parser.FindWord(word.Substring(1)));
            }else if (word == "-")
            {
                return new NullWorldMatch();
            }
            else
            {
                Type wordTypeMatch = typeof(WordTypeMatch<>);
                Type genericArgument = _parser.Dictionary.WordTypes.FirstOrDefault(w => w.Name == word + "Word");
                Type instantiableType = wordTypeMatch.MakeGenericType(new Type[] { genericArgument });
                return (IMatch)Activator.CreateInstance(instantiableType);
            }
        }

        private Func<IEnumerable<Word>, ParserResult> GetFunction(string word)
        {
            if (_methods.TryGetValue(word, out var func))
                return func;

            throw new ArgumentException($"Method '{word}' not found");
        }

        private void LoadItems(SerializedItem[] items)
        {
            foreach (var item in items)
            {
                var tmp = new Item(item.slug, item.description, item.examine, item.name, item.words, item.flags);
                if (!string.IsNullOrEmpty(item.initial))
                {
                    _rooms[item.initial].Items.Add(tmp);
                }
                _items.Add(tmp);
                _parser.Dictionary.Nouns.Add(tmp.Word);
            }
        }

        private Room CreateRoom(SerializedRoom room)
        {
            var exits = room.exits.Select(kvp => new RoomExit(_parser.Dictionary.Directions.Find(kvp.Key), kvp.Value)).ToList();
            foreach (var exit in exits)
            {
                if (room.blockers == null) break;
                var blocked = room.blockers.Keys.Select(k => k.Equals(exit.Exit.Primary, System.StringComparison.OrdinalIgnoreCase) ? room.blockers[k] : null)
                    .Where(s => s != null).ToList();
                if (blocked.Count() == 1)
                {
                    exit.Blocked = blocked[0];
                }
            }
            return new Room(room.shortname, room.description, room.slug, room.graphic, room.graphicFlag, exits);
        }

        internal bool FlagNonZero(string flag) => _flags.IsTruthy(flag);

        internal Item GetItem(string itemSlug)
        {
            return _items.FirstOrDefault(i => i.Slug == itemSlug);
        }

        public string[] GetExits()
        {
            return CurrentRoom.Exits.Where(e => FlagNonZero(e.Blocked))
                                    .Select(e => e.Exit.Primary).ToArray();
        }

        internal ParserResult PatternMatch(Word word1, Word word2)
        {
            var action = _actions.FirstOrDefault(a => a.IsMatch(word1, word2));
            if (action == null)
            {
                LastError = "What does that mean?";
                return ParserResult.Error;
            }
            return action.RunAction(this, word1, word2);
        }

        private ParserResult ChangeRoom(IEnumerable<Word> words)
        {
            var directionWord = words.FirstOrDefault(w => w is DirectionWord);
            var direction = CurrentRoom.Exits.FirstOrDefault(e => e.Exit == directionWord);
            if (direction == null || (!string.IsNullOrEmpty(direction.Blocked) && !_flags.IsTruthy(direction.Blocked)))
            {
                if (direction != null)
                {
                    LastError = _flags.GetText(direction.Blocked) ?? "You can't go that way!";
                }
                else
                {
                    LastError = $"You can't go {directionWord.Primary.ToLower()}.";
                }
                return ParserResult.Error;
            }
            CurrentRoom = _rooms[direction.Slug];
            return ParserResult.ChangeRoom;
        }

        private ParserResult ShowScore(IEnumerable<Word> words)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Score:");
            sb.AppendLine($"  Moves: {_flags.GetValue(GameFlags.PlayerMoveCount)}");
            LastMessage = sb.ToString();
            return ParserResult.ShowLastMessage;
        }

        private ParserResult DropItem(IEnumerable<Word> words)
        {
            var noun = words.FirstOrDefault(w => w is NounWord);
            if (_inventory.Count(i => noun == i.Word) > 0)
            {
                LastMessage = $"You drop {noun.Primary}.";
                var item = _inventory.FirstOrDefault(i => noun == i.Word);
                _inventory.Remove(item);
                CurrentRoom.Items.Add(item);
                return ParserResult.ShowLastMessage;
            }
            else
            {
                LastError = $"You're not carrying {noun.Primary}.";
                return ParserResult.Error;
            }
        }

        private ParserResult DescribeInventory(IEnumerable<Word> words)
        {
            if (_inventory.Count == 0)
            {
                LastMessage = "You are not carrying anything.";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("You are carrying:");
                foreach (var item in _inventory)
                {
                    sb.AppendLine($"  - {item.Name}");
                }
                LastMessage = sb.ToString();
            }
            return ParserResult.ShowLastMessage;
        }

        private ParserResult DescribeRoom(IEnumerable<Word> words)
        {
            return ParserResult.DescribeRoom;
        }

        private ParserResult ExamineItem(IEnumerable<Word> words)
        {
            LastMessage = "";
            var nounWord = words.FirstOrDefault(w => w is NounWord) ?? new Word("that");
            var item = CurrentRoom.Items.FirstOrDefault(i => i.Word == nounWord);
            if (item == null)
            {
                // Not in current location, maybe the player meant an item they
                // were carrying
                item = _inventory.FirstOrDefault(i => i.Word == nounWord);
            }
            if (item == null)
            {
                LastError = $"I don't see {nounWord.Primary} here.";
                return ParserResult.CannotSeeItem;
            }
            LastMessage = item.Examine;
            return ParserResult.ShowLastMessage;
        }

        private ParserResult TakeItem(IEnumerable<Word> words)
        {
            LastError = "";
            var nounWord = words.FirstOrDefault(w => w is NounWord) ?? new Word("that");
            var item = CurrentRoom.Items.FirstOrDefault(i => i.Word == nounWord);
            if (item == null)
            {
                LastError = $"I don't see {nounWord.Primary} here.";
                return ParserResult.CannotSeeItem;
            }
            else
            {
                var tmp = CurrentRoom.Items.First(i => i.Word == nounWord);
                if ((tmp.Flags & ObjectFlags.CannotCarry) == ObjectFlags.CannotCarry)
                {
                    LastError = $"Try as you might, you cannot pick up the {tmp.Word.Primary}";
                    return ParserResult.Error;
                }

                CurrentRoom.Items.Remove(tmp);
                _inventory.Add(tmp);
                LastMessage = $"You picked up the {nounWord.Primary}.";
                return ParserResult.ShowLastMessage;
            }
        }
    }
}
