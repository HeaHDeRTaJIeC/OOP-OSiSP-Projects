using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MortalKombatXI.Classes;

namespace MortalKombatXI.Screens
{
    class MenuScreen : Screen
    {
        private readonly Menu[] menu;
        private readonly Color activeColor = Color.DarkMagenta;
        private readonly Color passiveColor = Color.White;

        private readonly Texture2D   menuBackground;
        private readonly MenuItem    menuGameName;
        private readonly Rectangle   menuBackgroundR;

        public MenuScreen(Game game, MenuItem[] items, Texture2D background, Rectangle backgroundRect) : base(game)
        {
            menuBackground = background;
            menuGameName = items[3];
            menuBackgroundR = backgroundRect;
            menu = new Menu[3];
            menu[0] = new Menu(items[0], activeColor);
            menu[1] = new Menu(items[1], passiveColor);
            menu[2] = new Menu(items[2], passiveColor);
        }

        public void GetKey(int i)
        {
            //i=1 - Start game
            //i=2 - Help
            //i=3 - Exit
            if (i == 1)
            {
                menu[0].MenuColor = activeColor;
                menu[1].MenuColor = passiveColor;
                menu[2].MenuColor = passiveColor;

            }
            if (i == 2)
            {
                menu[0].MenuColor = passiveColor;
                menu[1].MenuColor = activeColor;
                menu[2].MenuColor = passiveColor;
            }
            if (i == 3)
            {
                menu[0].MenuColor = passiveColor;
                menu[1].MenuColor = passiveColor;
                menu[2].MenuColor = activeColor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //mortal kombat
            /*ScreenSpriteBatch.DrawString(
                spriteFont,
                outString,
                stringPosition,
                Color.DarkRed,
                0,
                stringOrigin,
                1.0f,
                SpriteEffects.None,
                0.5f);*/
            ScreenSpriteBatch.Draw(menuBackground, menuBackgroundR, Color.White);
            ScreenSpriteBatch.DrawString(
                menuGameName.MenuFont,
                menuGameName.MenuItemName,
                menuGameName.MenuItemPosition,
                Color.DarkRed,
                0f,
                menuGameName.MenuItemOrigin,
                1f,
                SpriteEffects.None,
                0.5f);
            for (int i = 0; i < 3; i++)
                ScreenSpriteBatch.DrawString(
                    menu[i].Item.MenuFont, 
                    menu[i].Item.MenuItemName,
                    menu[i].Item.MenuItemPosition,
                    menu[i].MenuColor,
                    0f,
                    menu[i].Item.MenuItemOrigin,
                    1f,
                    SpriteEffects.None, 
                    0.5f);
            
            base.Draw(gameTime);
        }
    }
}
