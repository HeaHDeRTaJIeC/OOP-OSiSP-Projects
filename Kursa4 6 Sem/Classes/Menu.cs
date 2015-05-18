using Microsoft.Xna.Framework;

namespace MortalKombatXI.Classes
{
    class Menu
    {
        public MenuItem Item;
        public Color MenuColor { get; set; }

        public Menu(MenuItem item, Color color)
        {
            Item = item;
            MenuColor = color;
        }
    }
}
