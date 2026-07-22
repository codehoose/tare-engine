namespace TareMonoGameBridge.Components
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using TareEngine;

    public class RoomDescriptionGraphicComponent : TareDrawableGameComponent
    {

        private Engine Engine => Game.Engine;
        private Texture2D Graphic => Game.RoomGraphic;

        public RoomDescriptionGraphicComponent(AdventureGame game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (Engine.CurrentRoom.HasGraphic && Graphic != null)
            {
                var rect = new Rectangle(Game.RoomGraphicPosition, new Point(Game.Config.ScreenWidth, 400));
                Game.SpriteBatch.Draw(Graphic, rect, Color.White);
            }
        }
    }
}
