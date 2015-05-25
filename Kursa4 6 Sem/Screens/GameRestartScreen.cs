using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MortalKombatXI.Classes;

namespace MortalKombatXI.Screens
{
    class GameRestartScreen : Screen
    {
        private readonly Menu[] restartMenu;

        public GameRestartScreen(Game game, MenuItem[] items) : base(game)
        {
            restartMenu = new Menu[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                restartMenu[i] = new Menu(items[i], GameSettings.menuColor);
            }
            restartMenu[0].MenuColor = GameSettings.activeColor;
        }

        public void GetKey(int i)
        {
            //i=1 - Restart
            //i=2 - Exit
            if (i == 1)
            {
                restartMenu[0].MenuColor = GameSettings.activeColor;
                restartMenu[1].MenuColor = GameSettings.menuColor;
            }
            if (i == 2)
            {
                restartMenu[0].MenuColor = GameSettings.menuColor;
                restartMenu[1].MenuColor = GameSettings.activeColor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < restartMenu.Length; i++)
            ScreenSpriteBatch.DrawString(
                restartMenu[i].Item.MenuFont,
                restartMenu[i].Item.MenuItemName,
                restartMenu[i].Item.MenuItemPosition,
                restartMenu[i].MenuColor,
                0f,
                restartMenu[i].Item.MenuItemOrigin,
                1f,
                SpriteEffects.None, 
                0.5f
                );
            base.Draw(gameTime);
        }
    }
}
