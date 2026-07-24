namespace TareMonoGameBridge.Components
{
    using Microsoft.Xna.Framework;
    using System;

    public class TareDrawableGameComponent : TareGameComponent, IDrawable
    {
        private int _drawOrder;
        private bool _visible;

        public new AdventureGame Game => (AdventureGame)base.Game;

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                OnDrawOrderChanged();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnVisibleChanged();
            }
        }

        public TareDrawableGameComponent(AdventureGame game) : base(game)
        {
            _visible = true;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Draw(GameTime gameTime)
        {

        }

        private void OnVisibleChanged() => VisibleChanged?.Invoke(this, EventArgs.Empty);

        private void OnDrawOrderChanged() => DrawOrderChanged?.Invoke(this, EventArgs.Empty);
    }
}
