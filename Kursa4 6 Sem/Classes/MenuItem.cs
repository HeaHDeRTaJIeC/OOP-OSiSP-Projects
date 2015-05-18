using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI.Classes
{
    class MenuItem
    {
        public SpriteFont MenuFont { get; set; }
        public Vector2 MenuItemPosition { get; set; }
        public Vector2 MenuItemOrigin { get; set; }
        public String MenuItemName { get; set; }
    }
}
