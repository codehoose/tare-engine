namespace TareMonoGameBridge.Components
{
    using Microsoft.Xna.Framework;
    using TareEngine.Parser;
    using TareMonoGameBridge.Input;

    public class KeyboardBufferComponent : TareGameComponent
    {
        private KeyboardBuffer _keyboardBuffer;

        public KeyboardBufferComponent(AdventureGame game) : base(game)
        {
            Enabled = false;
        }

        public void ClearBuffer() => _keyboardBuffer.Clear();

        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;

            _keyboardBuffer?.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            _keyboardBuffer = new KeyboardBuffer();
            _keyboardBuffer.TextEntered += (o, e) =>
            {
                if (string.IsNullOrEmpty(_keyboardBuffer.Input)) return;

                Game.Terminal.WriteLine("\n");
                // PARSE INTO WORDS ....
                var result = Game.Engine.Parse(_keyboardBuffer.Input);
                if (Game.HasGraphicChanged())
                {
                    Game.ShowGraphic(false);
                }
                switch (result)
                {
                    case ParserResult.ChangeRoom:
                        Game.ClearGraphic();
                        Game.EnterDescribeRoomState();
                        break;
                    case ParserResult.Error:
                        string error = Game.GetLastError();
                        Game.Terminal.WriteLine(error);
                        break;
                    case ParserResult.CannotSeeItem:
                        Game.Terminal.WriteLine(Game.Engine.LastError);
                        break;
                    case ParserResult.ShowLastMessage:
                        Game.Terminal.WriteLine(Game.Engine.LastMessage);
                        break;
                    case ParserResult.DescribeRoom:
                        Game.DescribeRoom(true);
                        break;
                }

                _keyboardBuffer.Clear(); // clear the buffer for n+1 actions in same location
                if (result != ParserResult.ChangeRoom && result != ParserResult.DescribeRoom)
                {
                    Game.WhatNext();
                }
            };
            _keyboardBuffer.Backspace += (o, e) =>
            {
                Game.Terminal.Backspace();
            };
            _keyboardBuffer.CharacterEntered += (o, e) => Game.Terminal.Write(e.ToString());
        }
    }
}
