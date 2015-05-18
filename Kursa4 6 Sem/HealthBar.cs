using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI
{
    class HealthBar
    {
        public Rectangle SourceRect { get; private set; }
        public Vector2 Origin { get; private set; }
        public Vector2 Position { get; private set; }
        public Texture2D Texture { get; private set; }
        private int width = 300;
        private int height = 50;
        private int currentFrame;

        public HealthBar(Vector2 position, Texture2D texture)
        {
            Texture = texture;
            Position = position;

        }

        public void Update(int health)
        {
            currentFrame = GameSettings.MaxHealth - health;
            SourceRect = new Rectangle(0, currentFrame * height, width, height);
            Origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
        }
    }
}
