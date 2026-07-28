using System;
using System.Collections.Generic;
using System.Text;

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
            //var keyboard = _game.GetComponent<KeyboardBufferComponent>();
            //keyboard.Enabled = false;
            fsm.Game.ToggleKeyboard(false);
        }

        public void Update(float deltaTime)
        {

        }
    }
}
