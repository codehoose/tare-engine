namespace TareEngine.States
{
    public class InitGameState : IState<IAdventureGame>
    {
        public static InitGameState Instance = new InitGameState();

        private InitGameState() { }

        public void Enter(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.Init();
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
