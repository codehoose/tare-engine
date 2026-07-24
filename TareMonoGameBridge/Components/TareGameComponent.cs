namespace TareMonoGameBridge.Components
{
    using Microsoft.Xna.Framework;

    public class TareGameComponent : GameComponent
    {
        public new AdventureGame Game => (AdventureGame)base.Game;

        public TareGameComponent(AdventureGame game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
