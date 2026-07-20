using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TARE.States;
using TareEngine;
using TareMonoGameBridge;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;
using TareMonoGameBridge.Input;

namespace TARE
{
    public class Game1 : AdventureGame
    {
        const int FONT_WIDTH = 18;
        const int FONT_HEIGHT = 32;
        const int SCREEN_ROWS = 25;
        const int SCREEN_COLS = 80;
        const int SCREEN_WIDTH = SCREEN_COLS * FONT_WIDTH;
        const int SCREEN_HEIGHT = SCREEN_ROWS * FONT_HEIGHT;
        
        private TerminalComponent _term;
        private KeyboardBuffer _keyboardBuffer;

        protected override void Initialize()
        {
            _config = new AdventureGameConfig
            {
                FontWidth = FONT_WIDTH,
                FontHeight = FONT_HEIGHT,
                ScreenRows = SCREEN_ROWS,
                ScreenCols = SCREEN_COLS,
                ScreenWidth = SCREEN_WIDTH,
                ScreenHeight = SCREEN_HEIGHT,
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            //_state = TareGameState.DescribeRoom;

            StateMachine.EnterState(InitState.Instance);

            //_term = AddComponent<TerminalComponent>(SCREEN_COLS, SCREEN_ROWS, Point.Zero);
            //_term.Font = LoadSpriteSheet("font/ibm-font-large", FONT_WIDTH, FONT_HEIGHT);

            Terminal.Scrolled += (o, e) =>
            {
                TerminalScrolled();
            };

            //_keyboardBuffer = new KeyboardBuffer();
            //_keyboardBuffer.TextEntered += (o, e) =>
            //{
            //    if (string.IsNullOrEmpty(_keyboardBuffer.Input)) return;

            //    _term.WriteLine("\n");
            //    // PARSE INTO WORDS ....
            //    var result = Engine.Parse(_keyboardBuffer.Input);
            //    if (GraphicChanged())
            //    {
            //        ShowGraphic(false);
            //    }
            //    switch(result)
            //    {
            //        case ParserResult.ChangeRoom:
            //            ClearGraphic();
            //            _state = TareGameState.DescribeRoom;
            //            break;
            //        case ParserResult.Error:
            //            string error = GetLastError();
            //            _term.WriteLine(error);
            //            break;
            //        case ParserResult.CannotSeeItem:
            //            _term.WriteLine(Engine.LastError);
            //            break;
            //        case ParserResult.ShowLastMessage:
            //            _term.WriteLine(Engine.LastMessage);
            //            break;
            //        case ParserResult.DescribeRoom:
            //            DescribeRoom(true);
            //            break;
            //    }

            //    WhatNext();
            //};
            //_keyboardBuffer.Backspace += (o, e) =>
            //{
            //    _term.Backspace();
            //};
            //_keyboardBuffer.CharacterEntered += (o, e) => _term.Write(e.ToString());

            Graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            Graphics.ApplyChanges();
        }

        protected override void ChangeRoom()
        {
            StateMachine.EnterState(ChangeRoomState.Instance);
        }

        protected override void DescribeRoom(bool isLook = false)
        {
            StateMachine.EnterState(isLook ? DescribeWithLookRoomState.Instance : DescribeRoomState.Instance);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //_keyboardBuffer.Update(gameTime);
            //_term.Update(gameTime);

            base.Update(gameTime);
        }
    }
}