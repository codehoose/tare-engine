using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TARE
{
    internal class KeyboardBuffer
    {
        const float CooldownMS = 50;

        KeyboardState _previous = new KeyboardState();
        string _input = "";
        float cooldown = 0;

        public event EventHandler TextEntered;

        public event EventHandler<char> CharacterEntered;
        public event EventHandler Backspace;

        public string Input => _input;

        public void Clear()
        {
            _previous = new KeyboardState();
            _input = "";
            cooldown = 0;
        }

        public void Update(GameTime gameTime)
        {
            cooldown -= (float)(gameTime.ElapsedGameTime.Milliseconds);
            if (cooldown > 0) return;

            KeyboardState state = Keyboard.GetState();
            if (state.TryConvertKeyboardInput(_previous, out char input))
            {
                if (input == '\b' && _input.Length > 0)
                {
                    _input = _input.Substring(0, _input.Length - 1);
                    Backspace?.Invoke(this, new EventArgs());
                }
                else if (input == '\n')
                {
                    TextEntered?.Invoke(this, new EventArgs());
                }
                else
                {
                    _input += input;
                    CharacterEntered?.Invoke(this, input);
                }

                cooldown = CooldownMS;
            }
            
            _previous = state;
        }
    }
}
