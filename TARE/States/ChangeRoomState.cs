using TareMonoGameBridge;
using TareMonoGameBridge.FSM;

namespace TARE.States
{
    internal class ChangeRoomState : IState<AdventureGame>
    {
        public static ChangeRoomState Instance = new ChangeRoomState();
        private ChangeRoomState() { }

        public void Enter(IStateMachine<AdventureGame> fsm)
        {
            fsm.Game.ClearGraphic();
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
