namespace TareEngine.States
{
    public class ChangeRoomState : IState<IAdventureGame>
    {
        public static ChangeRoomState Instance = new ChangeRoomState();
        private ChangeRoomState() { }

        public void Enter(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.ClearGraphic();
            fsm.EnterState(DescribeRoomState.Instance);
        }

        public void Exit(IStateMachine<IAdventureGame> fsm)
        {

        }

        public void Update(float deltaTime)
        {

        }
    }
}
