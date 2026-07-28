namespace TareEngine.States
{
    public class StateMachine : IStateMachine<IAdventureGame>
    {
        private IState<IAdventureGame>? _currentState;
        private readonly IAdventureGame _game;

        public IAdventureGame Game => _game;

        public StateMachine(IAdventureGame game)
        {
            _game = game;
        }

        public void EnterState(IState<IAdventureGame> state)
        {
            if (_currentState != null) _currentState.Exit(this);
            _currentState = state;
            _currentState.Enter(this);
        }

        public void Update(float deltaTime)
        {
            if (_currentState != null)
            {
                _currentState.Update(deltaTime);
            }
        }
    }
}
