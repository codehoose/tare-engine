namespace TareEngine.States
{
    public abstract class DescribeRoomBaseState : IState<IAdventureGame>
    {
        private bool _isLook;

        public DescribeRoomBaseState(bool isLook) => _isLook = isLook;

        public void Enter(IStateMachine<IAdventureGame> fsm)
        {
            fsm.Game.ClearTerminal();
            fsm.Game.DescribeRoom(_isLook);
            fsm.Game.WhatNext();
            fsm.EnterState(WaitForInputState.Instance);
        }

        public void Exit(IStateMachine<IAdventureGame> fsm)
        {
        }

        public void Update(float deltaTime)
        {
        }
    }
}
