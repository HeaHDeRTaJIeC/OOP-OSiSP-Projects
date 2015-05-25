using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MortalKombatXI.Classes;

namespace MortalKombatXI.Screens
{
    class GamePauseScreen : Screen
    {
        private readonly Menu[] pauseMenu;

        public GamePauseScreen(Game game, MenuItem[] items) : base(game)
        {
            pauseMenu = new Menu[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                pauseMenu[i] = new Menu(items[i], GameSettings.menuColor);
            }
            pauseMenu[0].MenuColor = GameSettings.activeColor;
        }

        public void GetKey(int i)
        {
            //i=1 - Resume
            //i=2 - Exit
            if (i == 1)
            {
                pauseMenu[0].MenuColor = GameSettings.activeColor;
                pauseMenu[1].MenuColor = GameSettings.menuColor;
            }
            if (i == 2)
            {
                pauseMenu[0].MenuColor = GameSettings.menuColor;
                pauseMenu[1].MenuColor = GameSettings.activeColor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < pauseMenu.Length; i++)
            ScreenSpriteBatch.DrawString(
                pauseMenu[i].Item.MenuFont,
                pauseMenu[i].Item.MenuItemName,
                pauseMenu[i].Item.MenuItemPosition,
                pauseMenu[i].MenuColor,
                0f,
                pauseMenu[i].Item.MenuItemOrigin,
                1f,
                SpriteEffects.None, 
                0.5f
                );
            base.Draw(gameTime);
        }
    }
}
