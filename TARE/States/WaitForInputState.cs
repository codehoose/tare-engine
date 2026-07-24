using TareEngine;
using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;

namespace TARE.States
{
    internal class WaitForInputState : IState<AdventureGame>
    {
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

            var keyboard = _game.GetComponent<KeyboardBufferComponent>();
            keyboard.Enabled = true;
            keyboard.ClearBuffer();
        }

        public void Exit(IStateMachine<AdventureGame> fsm)
        {
            var keyboard = _game.GetComponent<KeyboardBufferComponent>();
            keyboard.Enabled = false;
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
