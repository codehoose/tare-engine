using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using TARE.Engine;
using TARE.Engine.Parser;

namespace TARE
{
    public class Game1 : Game
    {
        const int FONT_WIDTH = 18;
        const int FONT_HEIGHT = 32;
        const int SCREEN_ROWS = 25;
        const int SCREEN_COLS = 80;

        private TareGameState _state;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteSheet _font;
        private Texture2D _graphic;
        private TareEngine _engine;
        private Terminal _term;
        private Terminal _input;
        private KeyboardBuffer _keyboardBuffer;
        private Point _graphicPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = new SpriteSheet(Content.Load<Texture2D>("font/ibm-font-large"), FONT_WIDTH, FONT_HEIGHT);
            _engine = new TareEngine();
            _engine.Init();

            _state = TareGameState.DescribeRoom;
            _term = new Terminal(_spriteBatch, _font, SCREEN_COLS, SCREEN_ROWS - 1, Point.Zero);
            _term.Scrolled += (o, e) =>
            {
                if (!_engine.CurrentRoom.HasGraphic || _graphic == null) return;
                _graphicPos.Y -= _font.CellHeight;
                if (_graphicPos.Y < -_graphic.Height - _font.CellHeight)
                {
                    ClearGraphic();
                }
            };
            _input = new Terminal(_spriteBatch, _font, 80, 1, new Point(0, 24 * _font.CellHeight));
            _keyboardBuffer = new KeyboardBuffer();
            _keyboardBuffer.TextEntered += (o, e) =>
            {
                _term.WriteLine(_keyboardBuffer.Input);
                // PARSE INTO WORDS ....
                var result = _engine.Parse(_keyboardBuffer.Input);
                if (GraphicChanged())
                {
                    ShowGraphic(false);
                }
                switch(result)
                {
                    case ParserResult.ChangeRoom:
                        ClearGraphic();
                        _state = TareGameState.DescribeRoom;
                        break;
                    case ParserResult.Error:
                        string error = GetLastError();
                        _term.WriteLine(error);
                        break;
                    case ParserResult.CannotSeeItem:
                        _term.WriteLine(_engine.LastError);
                        break;
                    case ParserResult.ShowLastMessage:
                        _term.WriteLine(_engine.LastMessage);
                        break;
                    case ParserResult.DescribeRoom:
                        DescribeRoom(true);
                        break;
                }

                _term.WriteLine("");
                _term.WriteLine("What next?");

                _input.Clear();
                _keyboardBuffer.Clear();
                _input.Write("> ");
            };
            _keyboardBuffer.Backspace += (o, e) =>
            {
                _input.Backspace();
            };
            _keyboardBuffer.CharacterEntered += (o, e) => _input.Write(e.ToString());

            _graphics.PreferredBackBufferWidth = SCREEN_COLS * FONT_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_ROWS * FONT_HEIGHT;
            _graphics.ApplyChanges();
        }

        private string GetLastError()
        {
            string lastError = _engine.LastError;
            if (string.IsNullOrEmpty(lastError)) return "I didn't understand that";
            return lastError;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _keyboardBuffer.Update(gameTime);

            if (_state == TareGameState.DescribeRoom)
            {
                _term.Clear();
                DescribeRoom();
                _term.WriteLine("");
                _term.WriteLine("What next?");
                _state = TareGameState.WaitForInput;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _term.Draw(gameTime);
            if (_engine.CurrentRoom.HasGraphic && _graphic != null)
            {
                var rect = new Rectangle(_graphicPos, new Point(1280, 400));
                _spriteBatch.Draw(_graphic, rect, Color.White);
            }
            _input.Draw(gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool GraphicChanged()
        {
            string graphicName = _engine.CurrentRoom.Graphic;
            if (!string.IsNullOrEmpty(_engine.CurrentRoom.GraphicFlag))
            {
                int index = _engine.Flags.GetValue(_engine.CurrentRoom.GraphicFlag);
                graphicName = _engine.CurrentRoom.GetGraphic(index);
            }

            string currentGraphicName = _graphic?.Name ?? string.Empty;
            return graphicName != currentGraphicName;
        }

        private bool ShowGraphic(bool resetPosition = true)
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

        private void DescribeRoom(bool isLook = false)
        {
            if (!isLook)
            {
                bool bumpText = ShowGraphic();
                if (bumpText)
                {
                    // That's 13 rows
                    _term.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n"); 
                }
            }
            _term.WriteLine(_engine.CurrentRoom.Description);
            DescribeItems();
            _term.WriteLine("");
            DescribeExits();

            _input.Clear();
            _input.Write("> ");
        }

        private void DescribeItems()
        {
            if (_engine.CurrentRoom.Items.Count == 0) return;
            _term.Write("You can see: ");
            var items = _engine.CurrentRoom.Items.Select(i => i.Name).ToArray();
            _term.WriteLine(GetJoined(items));
        }

        private void DescribeExits()
        {
            _term.Write("Exits: ");
            var exits = _engine.GetExits();
            _term.WriteLine(GetJoined(exits));
        }

        private string GetJoined(string[] items, string multiple = ", ", string twoItems = " and ")
        {
            var joiner = items.Length > 2 ? ", " : " and ";
            return  string.Join(joiner, items);
        }

        private void ClearGraphic()
        {
            _graphic = null;
        }
    }
}