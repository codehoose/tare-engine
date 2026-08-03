using TareMonoGameBridge;

namespace TARE
{
    public class Game1 : AdventureGame
    {
        const int FONT_WIDTH = 18;
        const int FONT_HEIGHT = 32;
        const int SCREEN_ROWS = 25;
        const int SCREEN_COLS = 80;
        const int SCREEN_WIDTH = SCREEN_COLS * FONT_WIDTH;
        const int SCREEN_HEIGHT = SCREEN_ROWS * FONT_HEIGHT;

        public Game1() : base(new TareEngine.Serialization.GameDataSerializer())
        {
        }

        protected override void Initialize()
        {
            _config = new AdventureGameConfig
            {
                FontWidth = FONT_WIDTH,
                FontHeight = FONT_HEIGHT,
                ScreenRows = SCREEN_ROWS,
                ScreenCols = SCREEN_COLS,
                ScreenWidth = SCREEN_WIDTH,
                ScreenHeight = SCREEN_HEIGHT,
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            Graphics.ApplyChanges();
        }
    }
}