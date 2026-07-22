using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Xml.Linq;
using TareEngine;
using TareEngine.Models;
using TareEngine.Parser;
using TareMonoGameBridge.Components;
using TareMonoGameBridge.FSM;
using TareMonoGameBridge.Graphics;
using TareMonoGameBridge.Input;

namespace TareMonoGameBridge
{
    public class AdventureGame : Game
    {
        private Texture2D _graphic;
        private Point _graphicPos;
        private GraphicsDeviceManager _graphics;
        private Engine _engine;
        private SpriteBatch _spriteBatch;
        protected IStateMachine<AdventureGame> StateMachine { get; }
        protected AdventureGameConfig _config;
        public SpriteBatch SpriteBatch => _spriteBatch;

        public GraphicsDeviceManager Graphics => _graphics;

        public Texture2D RoomGraphic => _graphic;
        public Point RoomGraphicPosition => _graphicPos;

        public Engine Engine => _engine;

        public AdventureGameConfig Config => _config;

        public TerminalComponent Terminal { get; set; }

        public AdventureGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            StateMachine = new StateMachine(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public T AddComponent<T>() where T : TareGameComponent
        {
            var component = (T)Activator.CreateInstance(typeof(T), this);
            Components.Add(component);
            return component;
        }

        public T AddComponent<T>(params object[] args) where T: TareGameComponent
        {
            var array = new object[] { this };

            if (args != null && args.Length > 0)
            {
                array = new object[args.Length + 1];
                array[0] = this;
                Array.Copy(args, 0, array, 1, args.Length);
            }

            var component = (T)Activator.CreateInstance(typeof(T), array);
            Components.Add(component);
            return component;
        }

        public T FindComponent<T>() where T : GameComponent
            => (T)Components.FirstOrDefault(c => c.GetType() == typeof(T));

        public SpriteSheet LoadSpriteSheet(string spriteSheet, int cellWidth, int cellHeight)
            => new SpriteSheet(Content.Load<Texture2D>(spriteSheet), cellWidth, cellHeight);

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _engine = new Engine();
            _engine.Init();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)(gameTime.ElapsedGameTime.Milliseconds);
            StateMachine.Update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            base.Draw(gameTime);
            _spriteBatch.End();
        }

        private bool GraphicChanged()
        {
            string graphicName = Engine.CurrentRoom.Graphic;
            if (!string.IsNullOrEmpty(Engine.CurrentRoom.GraphicFlag))
            {
                int index = Engine.Flags.GetValue(Engine.CurrentRoom.GraphicFlag);
                graphicName = Engine.CurrentRoom.GetGraphic(index);
            }

            string currentGraphicName = _graphic?.Name ?? string.Empty;
            return graphicName != currentGraphicName;
        }

        public void ClearGraphic()
        {
            _graphic = null;
        }

        public bool ShowGraphic(bool resetPosition = true)
        {
            string graphicName = _engine.CurrentRoom.Graphic;
            if (!string.IsNullOrEmpty(_engine.CurrentRoom.GraphicFlag))
            {
                int index = _engine.Flags.GetValue(_engine.CurrentRoom.GraphicFlag);
                graphicName = _engine.CurrentRoom.GetGraphic(index);
            }

            if (!_engine.CurrentRoom.HasGraphic) return false;

            if (_graphic != null && _graphic.Name == graphicName) return true;

            ClearGraphic();

            _graphic = Content.Load<Texture2D>(graphicName);
            if (resetPosition) _graphicPos = Point.Zero;
            return true;
        }

        protected virtual void TerminalScrolled()
        {
            if (!_engine.CurrentRoom.HasGraphic || _graphic == null) return;
            _graphicPos.Y -= _config.FontHeight;
            if (_graphicPos.Y < -_graphic.Height - _config.FontHeight)
            {
                ClearGraphic();
            }
        }

        public void WhatNext()
        {
            Terminal.WriteLine("What next?");
            Terminal.WriteLine("");
            //_keyboardBuffer.Clear();
            Terminal.GotoXY(0, Config.ScreenRows - 1);
            Terminal.Write("> ");
        }

        protected virtual void ChangeRoom() { }

        protected virtual void DescribeRoom(bool isLook = false) { }

        private string GetLastError()
        {
            string lastError = Engine.LastError;
            if (string.IsNullOrEmpty(lastError)) return "I didn't understand that";
            return lastError;
        }

        public void HandleInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            Terminal.WriteLine("\n");
            // PARSE INTO WORDS ....
            var result = _engine.Parse(input);
            if (GraphicChanged())
            {
                ShowGraphic(false);
            }
            switch (result)
            {
                case ParserResult.ChangeRoom:
                    ChangeRoom();
                    break;
                case ParserResult.Error:
                    string error = GetLastError();
                    Terminal.WriteLine(error);
                    break;
                case ParserResult.CannotSeeItem:
                    Terminal.WriteLine(Engine.LastError);
                    break;
                case ParserResult.ShowLastMessage:
                    Terminal.WriteLine(Engine.LastMessage);
                    break;
                case ParserResult.DescribeRoom:
                    DescribeRoom(true);
                    break;
            }
        }
    }
}
