using TareEngine;
using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;
using TareMonoGameBridge.Input;

namespace TARE.States
{
    internal class WaitForInputState : IState<AdventureGame>
    {
        private KeyboardBuffer _keyboardBuffer;
        private TerminalComponent _term;
        private Engine _engine;
        private AdventureGame _game;

        public static WaitForInputState Instance = new WaitForInputState();

        private WaitForInputState() { }

        public void Enter(IStateMachine<AdventureGame> fsm)
        {
            _term = fsm.Game.Terminal;
            _engine = fsm.Game.Engine;
            _game = fsm.Game;

            if (_keyboardBuffer == null)
            {
                _keyboardBuffer = new KeyboardBuffer();
                _keyboardBuffer.TextEntered += (o, e) =>
                {
                    fsm.Game.HandleInput(_keyboardBuffer.Input);

                    fsm.Game.WhatNext();
                };
                _keyboardBuffer.Backspace += (o, e) =>
                {
                    _term.Backspace();
                };
                _keyboardBuffer.CharacterEntered += (o, e) => _term.Write(e.ToString());
            }
        }

        public void Exit(IStateMachine<AdventureGame> fsm)
        {

        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
