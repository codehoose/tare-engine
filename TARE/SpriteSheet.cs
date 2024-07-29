using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TARE
{
    internal class SpriteSheet
    {
        private Texture2D _texture;
        private readonly int _rows;
        private readonly int _columns;
        private readonly int _cellHeight;
        private readonly int _cellWidth;

        public int CellHeight => _cellHeight;
        public int CellWidth => _cellWidth;

        public SpriteSheet(Texture2D texture, int cellWidth, int cellHeight)
        {
            _texture = texture;
            _rows = texture.Height / cellHeight;
            _columns = texture.Width / cellWidth;
            _cellHeight = cellHeight;
            _cellWidth = cellWidth;
        }

        public void Draw(SpriteBatch spriteBatch, Point position, int cell)
        {
            var x = _cellWidth * (cell % _columns);
            var y = _cellHeight * (cell / _columns);
            var src = new Rectangle(x, y, _cellWidth, _cellHeight);
            var dest = new Rectangle(position.X, position.Y, _cellWidth, _cellHeight);
            spriteBatch.Draw(_texture, dest, src, Color.White);
        }

        public void DrawString(SpriteBatch spriteBatch, Point position, string text)
        {
            DrawSequence(spriteBatch, position, text.ToArray().Select(ch => (int)ch));
        }

        public void DrawSequence(SpriteBatch spriteBatch, Point position, IEnumerable<int> cells)
        {
            var drawPoint = position;
            foreach (var cell in cells)
            {
                var x = _cellWidth * (cell % _columns);
                var y = _cellHeight * (cell / _columns);
                var src = new Rectangle(x, y, _cellWidth, _cellHeight);
                var dest = new Rectangle(drawPoint.X, drawPoint.Y, _cellWidth, _cellHeight);
                spriteBatch.Draw(_texture, dest, src, Color.White);
                drawPoint += new Point(_cellWidth, 0);
            }
        }
    }
}
