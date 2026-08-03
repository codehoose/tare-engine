namespace TareMonoGameBridge
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Linq;
    using TareEngine;
    using TareEngine.Models;
    using TareEngine.Serialization;
    using TareEngine.States;
    using TareMonoGameBridge.Components;
    using TareMonoGameBridge.Graphics;

    public class AdventureGame : Game, IAdventureGame
    {
        private Texture2D _graphic;
        private Point _graphicPos;
        private GraphicsDeviceManager _graphics;
        private readonly IGameDataSerializer _gameDataSerializer;
        private Engine _engine;
        private SpriteBatch _spriteBatch;
        private KeyboardBufferComponent _keyboard;

        protected AdventureGameConfig _config;
        public SpriteBatch SpriteBatch => _spriteBatch;

        public GraphicsDeviceManager Graphics => _graphics;

        public IStateMachine<IAdventureGame> StateMachine { get; }
        public Texture2D RoomGraphic => _graphic;
        public Point RoomGraphicPosition => _graphicPos;

        public Engine Engine => _engine;

        public AdventureGameConfig Config => _config;

        public TerminalComponent Terminal { get; set; }

        public void ClearTerminal() => Terminal?.Clear();

        public void ClearKeyboard() => _keyboard?.ClearBuffer();

        public void ToggleKeyboard(bool enableKeyboard)
        {
            if (_keyboard == null) return;
            _keyboard.Enabled = enableKeyboard;
        }

        public virtual void Init()
        {
            AddComponent<RoomDescriptionGraphicComponent>();
            _keyboard = AddComponent<KeyboardBufferComponent>();
        }

        public AdventureGame(IGameDataSerializer gameDataSerializer)
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameDataSerializer = gameDataSerializer;
            StateMachine = new StateMachine(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public T GetComponent<T>() where T: TareGameComponent
        {
            return (T)Components.FirstOrDefault(c => c is T);
        }

        public T AddComponent<T>() where T : TareGameComponent
        {
            var component = (T)Activator.CreateInstance(typeof(T), this);
            Components.Add(component);
            component.Initialize();
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
            component.Initialize();
            return component;
        }

        public T FindComponent<T>() where T : GameComponent
            => (T)Components.FirstOrDefault(c => c.GetType() == typeof(T));

        public SpriteSheet LoadSpriteSheet(string spriteSheet, int cellWidth, int cellHeight)
            => new SpriteSheet(Content.Load<Texture2D>(spriteSheet), cellWidth, cellHeight);

        public virtual void EnterDescribeRoomState() => DescribeRoom();

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            SetupTerminal();

            _engine = new Engine();
            _engine.Init(_gameDataSerializer);
        }

        private void SetupTerminal()
        {
            var cols = Config.ScreenCols;
            var rows = Config.ScreenRows;
            var fontWidth = Config.FontWidth;
            var fontHeight = Config.FontHeight;

            Terminal = AddComponent<TerminalComponent>(cols, rows, Point.Zero);
            Terminal.Font = LoadSpriteSheet("font/ibm-font-large", fontWidth, fontHeight);
            Terminal.Scrolled += (o, e) => TerminalScrolled();
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

        public bool HasGraphicChanged()
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
            Terminal.GotoXY(0, Config.ScreenRows - 1);
            Terminal.Write("> ");
        }

        protected virtual void ChangeRoom()
        {
            StateMachine.EnterState(ChangeRoomState.Instance);
        }

        public void DescribeRoom(bool isLook = false)
        {
            if (!isLook)
            {
                Terminal.Clear();
                bool bumpText = ShowGraphic();
                if (bumpText)
                {
                    // That's 13 rows
                    Terminal.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                }
            }
            Terminal.WriteLine(_engine.CurrentRoom.Description);
            DescribeItems();
            Terminal.WriteLine("");
            DescribeExits();
        }

        private void DescribeItems()
        {
            if (_engine.CurrentRoom.Items.Count == 0) return;
            Terminal.Write("You can see: ");
            var items = _engine.CurrentRoom.Items.Where(i => (i.Flags & ObjectFlags.Hidden) != ObjectFlags.Hidden).Select(i => i.Name).ToArray();
            Terminal.WriteLine(GetJoined(items));
        }

        private void DescribeExits()
        {
            Terminal.Write("Exits: ");
            var exits = _engine.GetExits();
            Terminal.WriteLine(GetJoined(exits));
        }
        private string GetJoined(string[] items, string multiple = ", ", string twoItems = " and ")
        {
            var joiner = items.Length > 2 ? ", " : " and ";
            return string.Join(joiner, items);
        }

        public string GetLastError()
        {
            var lastError = Engine.LastError;
            if (string.IsNullOrEmpty(lastError)) return "I didn't understand that";
            return lastError;
        }
    }
}
