using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TARE
{
    internal class Terminal
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteSheet _spriteSheet;
        private readonly int _columns;
        private readonly int _rows;
        private readonly Point _point;
        private Point _cursor;
        private char[] _buffer;
        private bool _writeLine;
        private bool _scroll = true;
        private bool _acceptInput = false;

        public char CursorShape { get; set; } = '_';

        public event EventHandler Scrolled;

        public bool Scroll
        {
            get => _scroll;
            set => _scroll = value;
        }

        public Terminal(SpriteBatch spriteBatch, SpriteSheet spriteSheet, int columns, int rows, Point point)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            _columns = columns;
            _rows = rows;
            _point = point;
            _buffer = new char[columns * rows];
        }

        public void Clear()
        {
            _buffer = new char[_columns * _rows];
            _cursor = new Point(0, 0);
        }

        public void Backspace()
        {
            var current = _cursor;

            _cursor -= new Point(1, 0);
            if (_cursor.X < 0)
            {
                _cursor.Y--;
                _cursor.X = _columns - 1;
                if (_cursor.Y < 0)
                {
                    _cursor.Y = 0;
                    _cursor.X = 0;
                }
            }

            int index = _cursor.X + _cursor.Y * _columns;
            _buffer[index] = (char)0;
        }

        public void Write(string str)
        {
            if (_cursor.Y >= _rows)
            {
                _cursor = new(0, _rows - 1);
                ShiftUp();
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\r')
                {
                    _cursor.X = 0;
                    continue;
                }
                else if (str[i] == '\n')
                {
                    _cursor.Y++;
                    if (_cursor.Y >= _rows)
                    {
                        _cursor = new(0, _rows - 1);
                        ShiftUp();
                    }
                    continue;
                }

                int index = _cursor.X + _cursor.Y * _columns;
                _buffer[index] = str[i];
                _cursor += new Point(1, 0);
                if (_cursor.X == _columns)
                {
                    if (!_scroll)
                    {
                        _cursor.X = _columns - 1;
                        continue;
                    }

                    _cursor = new Point(0, _cursor.Y + 1);
                    if (_cursor.Y == _rows)
                    {
                        ShiftUp();
                        // Move everything down   
                        _cursor.Y = _rows - 1;
                    }
                }
            }

            // Move to next line
            if (_writeLine && _cursor.Y != _rows)
            {
                _cursor = new Point(0, _cursor.Y + 1);
            }

            _writeLine = false;
        }

        public void WriteLine(string str)
        {
            _writeLine = true;
            Write(str);
        }

        public void Draw(GameTime gameTime)
        {
            var pt = _point;
            for (int y = 0; y < _rows; y++)
            {
                _spriteSheet.DrawString(_spriteBatch, pt, _buffer.SubArray(y * _columns, _columns).ToText());
                pt += new Point(0, _spriteSheet.CellHeight);
            }
        }

        private void ShiftUp()
        {
            var temp = new char[_columns * _rows];
            Array.Copy(_buffer, _columns, temp, 0, _columns * (_rows - 1));
            _buffer = temp;
            Scrolled?.Invoke(this, EventArgs.Empty);
        }
    }
}
