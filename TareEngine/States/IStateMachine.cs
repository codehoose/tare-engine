namespace TareEngine.States
{
    public interface IStateMachine<T> where T : IAdventureGame
    {
        public T Game { get; }
        void EnterState(IState<T> state);
        void Update(float deltaTime);
    }
}
