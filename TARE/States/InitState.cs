using Microsoft.Xna.Framework;
using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;

namespace TARE.States
{
    internal class InitState : IState<AdventureGame>
    {
        private readonly IState<AdventureGame> _nextState;
        private TerminalComponent _term;
        private IStateMachine<AdventureGame> _stateMachine;

        public static InitState Instance = new InitState();

        private InitState() { }

        public void Enter(IStateMachine<AdventureGame> fsm)
        {
            _stateMachine = fsm;

            var cols = fsm.Game.Config.ScreenCols;
            var rows = fsm.Game.Config.ScreenRows;
            var fontWidth = fsm.Game.Config.FontWidth;
            var fontHeight = fsm.Game.Config.FontHeight;

            _term = fsm.Game.AddComponent<TerminalComponent>(cols, rows, Point.Zero);
            _term.Font = fsm.Game.LoadSpriteSheet("font/ibm-font-large", fontWidth, fontHeight);
            fsm.Game.Terminal = _term;
            fsm.EnterState(DescribeRoomState.Instance);
        }

        public void Exit(IStateMachine<AdventureGame> fsm)
        {
            
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
