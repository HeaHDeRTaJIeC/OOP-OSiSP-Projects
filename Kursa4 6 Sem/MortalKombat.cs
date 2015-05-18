using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MortalKombatXI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MortalKombat : Game
    {
        Texture2D backgroundTexture2D;
        Texture2D arenaTexture;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteAnimation spriteFirst;
        PlayerState stateFirst;
        HealthBar healthFirst;

        SpriteAnimation spriteSecond;
        PlayerState stateSecond;
        HealthBar healthSecond;

        int width;
        int height;

        public MortalKombat()
        {
            graphics = new GraphicsDeviceManager(this);
            width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            GameSettings.FirstPosition.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2;
            GameSettings.FirstPosition.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height*7/11;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {   
            var scorpionDictionary = DictionaryLoader.GetDictionary("Scorpion", Content);
            var subZeroDictionary = DictionaryLoader.GetDictionary("SubZero", Content);
            healthFirst = new HealthBar(new Vector2(400, 100), Content.Load<Texture2D>("DamageBar"));
            healthSecond = new HealthBar(new Vector2(width - 400, 100), Content.Load<Texture2D>("DamageBar"));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //first player
            spriteFirst = new SpriteAnimation(scorpionDictionary, 0, GameSettings.SpriteWidth, GameSettings.SpriteHeigth, false);
            var moves = new PlayerControls();
            moves.GetFirstPlayerKeyboard();
            spriteFirst.moves = moves;
            spriteFirst.Position = new Vector2(GameSettings.FirstPosition.X - 100, GameSettings.FirstPosition.Y);
    
            //second player
            spriteSecond = new SpriteAnimation(subZeroDictionary, 0, GameSettings.SpriteWidth, GameSettings.SpriteHeigth, true);
            moves = new PlayerControls();
            moves.GetSecondPlayerKeyboard();
            spriteSecond.moves = moves;
            spriteSecond.Position = new Vector2(GameSettings.FirstPosition.X + 100, GameSettings.FirstPosition.Y);

            // TODO: use this.Content to load your game content here
            backgroundTexture2D = Content.Load<Texture2D>("BackgroundMenu");
            arenaTexture = Content.Load<Texture2D>("Arenas//TheDeadPoolArena");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape) ||
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
            {
                GameSettings.FightStart = true;
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Back))
            {
                GameSettings.FightStart = false;
            }

            if (GameSettings.FightStart)
            {
                stateFirst = spriteFirst.HandleSpriteMovement(gameTime);
                stateSecond = spriteSecond.HandleSpriteMovement(gameTime);
                // Allows the game to exit


                // TODO: Add your update logic here
                if (stateFirst == PlayerState.Move || stateSecond == PlayerState.Move)
                    CollideDetector.RepairMoveCollision(spriteFirst, spriteSecond, width);
                CollideDetector.HitCollision(spriteFirst, spriteSecond, gameTime);
                healthFirst.Update(spriteFirst.Information.Health);
                healthSecond.Update(spriteSecond.Information.Health);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //spriteSecond.CurrentTexture();

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            if (GameSettings.FightStart)
            {
                spriteBatch.Draw(
                arenaTexture,
                new Rectangle(
                    0,
                    0,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height
                    ),
                Color.White);
                spriteBatch.Draw(
                    spriteFirst.CurrentTexture,
                    spriteFirst.Position,
                    spriteFirst.SourceRect,
                    Color.White,
                    0f,
                    spriteFirst.Origin,
                    1.0f,
                    SpriteEffects.None,
                    0);
                spriteBatch.Draw(
                    spriteSecond.CurrentTexture,
                    spriteSecond.Position,
                    spriteSecond.SourceRect,
                    Color.White,
                    0f,
                    spriteSecond.Origin,
                    1.0f,
                    SpriteEffects.FlipHorizontally,
                    0);
                spriteBatch.Draw(
                    healthFirst.Texture,
                    healthFirst.Position,
                    healthFirst.SourceRect,
                    Color.White,
                    0f,
                    healthFirst.Origin,
                    1.0f,
                    SpriteEffects.None,
                    0);
                spriteBatch.Draw(
                    healthSecond.Texture,
                    healthSecond.Position,
                    healthSecond.SourceRect,
                    Color.White,
                    0f,
                    healthSecond.Origin,
                    1.0f,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                spriteBatch.Draw(
                backgroundTexture2D,
                new Rectangle(
                    0,
                    0,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height
                    ),
                Color.White);
            }
            spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
