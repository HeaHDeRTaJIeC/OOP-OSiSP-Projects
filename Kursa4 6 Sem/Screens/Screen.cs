using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI
{
    class Screen : DrawableGameComponent
    {
        public SpriteBatch ScreenSpriteBatch;

        public Screen(Game game) : base(game)
        {
            Visible = false;
            Enabled = false;
            ScreenSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        public void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public void Hide()
        {
            Visible = false;
            Enabled = false;
        }
    }
}
