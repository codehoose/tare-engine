using Microsoft.Xna.Framework;

namespace TareMonoGameBridge.FSM
{
    public interface IStateMachine<T> where T: Game
    {
        public T Game { get; }
        void EnterState(IState<T> state);
        void Update(float deltaTime);
    }
}
