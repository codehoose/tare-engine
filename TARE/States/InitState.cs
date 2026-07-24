using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;

namespace TARE.States
{
    internal class InitState : IState<AdventureGame>
    {
        private readonly IState<AdventureGame> _nextState;
        private TerminalComponent _term;
        private RoomDescriptionGraphicComponent _roomGraphic;
        private KeyboardBufferComponent _keyboard;
        private IStateMachine<AdventureGame> _stateMachine;

        public static InitState Instance = new InitState();

        private InitState() { }

        public void Enter(IStateMachine<AdventureGame> fsm)
        {
            _stateMachine = fsm;

            _roomGraphic = fsm.Game.AddComponent<RoomDescriptionGraphicComponent>();
            _keyboard = fsm.Game.AddComponent<KeyboardBufferComponent>();

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
