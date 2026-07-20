using Microsoft.Xna.Framework;
using TareMonoGameBridge.Extensions;
using TareMonoGameBridge.Input;

namespace TareMonoGameBridge.Components
{
    //public class KeyboardBufferComponent : TareGameComponent
    //{
    //    //private readonly TerminalComponent _term;
    //    private KeyboardBuffer _keyboardBuffer;

    //    public KeyboardBufferComponent(AdventureGame game) : base(game)
    //    {
    //        //_term = game.FindComponent<TerminalComponent>();
    //    }

    //    public override void Update(GameTime gameTime)
    //    {
    //        _keyboardBuffer.Update(gameTime);
    //        base.Update(gameTime);
    //    }

    //    public override void Initialize()
    //    {
    //        _keyboardBuffer = new KeyboardBuffer();
    //        _keyboardBuffer.TextEntered += (o, e) =>
    //        {
    //            if (string.IsNullOrEmpty(_keyboardBuffer.Input)) return;

    //            _term.WriteLine("\n");
    //            // PARSE INTO WORDS ....
    //            var result = _engine.Parse(_keyboardBuffer.Input);
    //            if (GraphicChanged())
    //            {
    //                ShowGraphic(false);
    //            }
    //            switch (result)
    //            {
    //                case ParserResult.ChangeRoom:
    //                    ClearGraphic();
    //                    _state = TareGameState.DescribeRoom;
    //                    break;
    //                case ParserResult.Error:
    //                    string error = GetLastError();
    //                    _term.WriteLine(error);
    //                    break;
    //                case ParserResult.CannotSeeItem:
    //                    _term.WriteLine(_engine.LastError);
    //                    break;
    //                case ParserResult.ShowLastMessage:
    //                    _term.WriteLine(_engine.LastMessage);
    //                    break;
    //                case ParserResult.DescribeRoom:
    //                    DescribeRoom(true);
    //                    break;
    //            }

    //            WhatNext();
    //        };
    //        _keyboardBuffer.Backspace += (o, e) =>
    //        {
    //            _term.Backspace();
    //        };
    //        _keyboardBuffer.CharacterEntered += (o, e) => _term.Write(e.ToString());

    //        base.Initialize();
    //    }
    //}
}
