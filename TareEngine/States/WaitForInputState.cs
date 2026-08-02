namespace TareEngine.States
{
    public class WaitForInputState : IState<IAdventureGame>
    {
        public static WaitForInputState Instance = new WaitForInputState();

        private WaitForInputState() { }

        public void Enter(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.ClearKeyboard();
            fsm.Game.ToggleKeyboard(true);
        }

        public void Exit(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.ToggleKeyboard(false);
        }

        public void Update(float deltaTime)
        {

        }
    }
}
