using Microsoft.Xna.Framework;

namespace TareMonoGameBridge.FSM
{
    public interface IState<T> where T : Game
    {
        void Enter(IStateMachine<T> fsm);
        void Exit(IStateMachine<T> fsm);
        void Update(float deltaTime);
    }
}
