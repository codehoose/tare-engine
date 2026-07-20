using System.Linq;
using TareEngine;
using TareEngine.Models;
using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;

namespace TARE.States
{
    internal abstract class BaseDescribeRoomState : IState<AdventureGame>
    {
        private TerminalComponent _term;
        private Engine _engine;
        private AdventureGame _game;
        private bool _isLook;

        public BaseDescribeRoomState(bool isLook) => _isLook = isLook;

        public void Enter(IStateMachine<AdventureGame> fsm)
        {
            _term = fsm.Game.Terminal;
            _engine = fsm.Game.Engine;
            _game = fsm.Game;

            _term.Clear();
            DescribeRoom(_isLook);
            fsm.Game.WhatNext();
            fsm.EnterState(WaitForInputState.Instance);
        }

        public void Exit(IStateMachine<AdventureGame> fsm)
        {
        }

        public void Update(float deltaTime)
        {
        }

        private void DescribeRoom(bool isLook = false)
        {
            if (!isLook)
            {
                _term.Clear();
                bool bumpText = _game.ShowGraphic();
                if (bumpText)
                {
                    // That's 13 rows
                    _term.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                }
            }
            _term.WriteLine(_engine.CurrentRoom.Description);
            DescribeItems();
            _term.WriteLine("");
            DescribeExits();
        }

        private void DescribeItems()
        {
            if (_engine.CurrentRoom.Items.Count == 0) return;
            _term.Write("You can see: ");
            var items = _engine.CurrentRoom.Items.Where(i => (i.Flags & ObjectFlags.Hidden) != ObjectFlags.Hidden).Select(i => i.Name).ToArray();
            _term.WriteLine(GetJoined(items));
        }

        private void DescribeExits()
        {
            _term.Write("Exits: ");
            var exits = _engine.GetExits();
            _term.WriteLine(GetJoined(exits));
        }
        private string GetJoined(string[] items, string multiple = ", ", string twoItems = " and ")
        {
            var joiner = items.Length > 2 ? ", " : " and ";
            return string.Join(joiner, items);
        }
    }
}
