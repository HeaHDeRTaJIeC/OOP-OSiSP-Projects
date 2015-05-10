using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MortalKombatXI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        Texture2D backgroundTexture2D;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimateSprite sprite;
        AnimateSprite sprite1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var dictionaryTexture2Ds = new ConcurrentDictionary<string, Texture2D>();
            dictionaryTexture2Ds.GetOrAdd("ScorpionStay", Content.Load<Texture2D>("Moves//ScorpionStay"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionStay", Content.Load<Texture2D>("Moves//ScorpionStay"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionWin", Content.Load<Texture2D>("Moves//ScorpionWin"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionMove", Content.Load<Texture2D>("Moves//ScorpionMove"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionBlock", Content.Load<Texture2D>("Moves//ScorpionBlock"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionRightHandHit", Content.Load<Texture2D>("Moves//ScorpionRightHandHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionLeftHandHit", Content.Load<Texture2D>("Moves//ScorpionLeftHandHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionSitDown", Content.Load<Texture2D>("Moves//ScorpionSitDown"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionGroundHit", Content.Load<Texture2D>("Moves//ScorpionGroundHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionDownBlock", Content.Load<Texture2D>("Moves//ScorpionDownBlock"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionAirHit", Content.Load<Texture2D>("Moves//ScorpionAirHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionUppercodeStayHit", Content.Load<Texture2D>("Moves//ScorpionUppercodeStayHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionUppercodeSitHit", Content.Load<Texture2D>("Moves//ScorpionUppercodeSitHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionHandSitHit", Content.Load<Texture2D>("Moves//ScorpionHandSitHit"));
            dictionaryTexture2Ds.GetOrAdd("ScorpionSit", Content.Load<Texture2D>("Moves//ScorpionSit"));
            
            sprite = new AnimateSprite(dictionaryTexture2Ds, 0, GameConstants.SpriteWidth, GameConstants.SpriteHeigth);
            sprite.moves = new GameMoves();
            sprite.moves.MoveDown = Keys.Down;
            sprite.moves.MoveUp = Keys.Up;
            sprite.moves.MoveLeft = Keys.Left;
            sprite.moves.MoveRight = Keys.Right;
            sprite.moves.Block = Keys.Q;
            sprite.moves.HandHitLeft = Keys.G;
            sprite.moves.HandHitRight = Keys.F;
            sprite.moves.LegAirHit = Keys.T;
            sprite.moves.LegGroundHit = Keys.Y;
            sprite.moves.UppercodeHit = Keys.R;
            sprite.moves.WinPose = Keys.Space;
            sprite.Position = new Vector2(GameConstants.FirstPosition.X, GameConstants.FirstPosition.Y);

            sprite1 = new AnimateSprite(dictionaryTexture2Ds, 0, GameConstants.SpriteWidth, GameConstants.SpriteHeigth);
            sprite1.moves = new GameMoves();
            sprite1.moves.MoveDown = Keys.S;
            sprite1.moves.MoveUp = Keys.W;
            sprite1.moves.MoveLeft = Keys.A;
            sprite1.moves.MoveRight = Keys.D;
            sprite1.moves.Block = Keys.B;
            sprite1.moves.HandHitLeft = Keys.L;
            sprite1.moves.HandHitRight = Keys.K;
            sprite1.moves.LegAirHit = Keys.O;
            sprite1.moves.LegGroundHit = Keys.P;
            sprite1.moves.UppercodeHit = Keys.U;
            sprite1.moves.WinPose = Keys.Space;
            sprite1.Position = new Vector2(GameConstants.FirstPosition.X + 200, GameConstants.FirstPosition.Y);

            // TODO: use this.Content to load your game content here
            backgroundTexture2D = Content.Load<Texture2D>("BackgroundMenu");
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
            sprite.HandleSpriteMovement(gameTime);
            sprite1.HandleSpriteMovement(gameTime);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(
                backgroundTexture2D,
                new Rectangle(
                    0,
                    0,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                    graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height
                    ),
                Color.White);
            spriteBatch.Draw(
                sprite.CurrentTexture(), 
                sprite.Position,
                sprite.SourceRect, 
                Color.White, 
                0f, 
                sprite.Origin, 
                1.0f, 
                SpriteEffects.None, 
                0);
            spriteBatch.Draw(
                sprite1.CurrentTexture(),
                sprite1.Position,
                sprite1.SourceRect,
                Color.White,
                0f,
                sprite1.Origin,
                1.0f,
                SpriteEffects.None,
                0);
            spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
