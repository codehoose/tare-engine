using Microsoft.Xna.Framework;

namespace TareMonoGameBridge.Components
{
    public class TareGameComponent : GameComponent
    {
        public new AdventureGame Game => (AdventureGame)base.Game;

        public TareGameComponent(AdventureGame game) : base(game)
        {
        }
    }
}
