namespace TareEngine.States
{
    public class InitState : IState<IAdventureGame>
    {
        public static InitState Instance = new InitState();

        private InitState() { }

        public void Enter(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.Init();
        }

        public void Exit(IStateMachine<IAdventureGame> fsm)
        {

        }

        public void Update(float deltaTime)
        {

        }
    }
}
