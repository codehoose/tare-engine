namespace TareEngine.States
{
    public interface IState<T> where T : IAdventureGame
    {
        void Enter(IStateMachine<T> fsm);
        void Exit(IStateMachine<T> fsm);
        void Update(float deltaTime);
    }
}
