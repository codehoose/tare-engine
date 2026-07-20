namespace TareMonoGameBridge.FSM
{
    public class StateMachine : IStateMachine<AdventureGame>
    {
        private IState<AdventureGame>? _currentState;
        private readonly AdventureGame _game;

        public AdventureGame Game => _game;

        public StateMachine(AdventureGame game)
        {
            _game = game;
        }

        public void EnterState(IState<AdventureGame> state)
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
