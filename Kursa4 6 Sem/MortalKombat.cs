using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MortalKombatXI.Classes;
using MortalKombatXI.Screens;


namespace MortalKombatXI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MortalKombat : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MenuScreen menuScreen;
        private GameScreen gameScreen;
        private HelpScreen helpScreen;
        private int currentMenuItem;
        private int interval;

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
            currentMenuItem = 1;
            interval = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            var scorpionDictionary = DictionaryLoader.GetDictionary("Scorpion", Content);
            var subZeroDictionary = DictionaryLoader.GetDictionary("SubZero", Content);
            var firstHealth = new HealthBar(new Vector2(400, 100), Content.Load<Texture2D>("DamageBar"));
            var secondHealth = new HealthBar(new Vector2(width - 400, 100), Content.Load<Texture2D>("DamageBar"));
            var firstMoves = new PlayerControls();
            firstMoves.GetFirstPlayerKeyboard();
            var secondMoves = new PlayerControls();
            secondMoves.GetSecondPlayerKeyboard();
            
            //first player
            var firstSprite = new SpriteAnimation(GameSettings.SpriteWidth, GameSettings.SpriteHeigth, false)
            {
                SpriteTexture = scorpionDictionary,
                moves = firstMoves,
                Position = new Vector2(GameSettings.FirstPosition.X - 100, GameSettings.FirstPosition.Y)
            };

            //second player
            var secondSprite = new SpriteAnimation(GameSettings.SpriteWidth, GameSettings.SpriteHeigth, true)
            {
                SpriteTexture = subZeroDictionary,
                moves = secondMoves,
                Position = new Vector2(GameSettings.FirstPosition.X + 100, GameSettings.FirstPosition.Y)
            };

            //game screen
            var arenaRect = new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            var arenaTexture = Content.Load<Texture2D>("Arenas//TheDeadPoolArena");
            gameScreen = new GameScreen(this, width, arenaTexture, arenaRect)
            {
                FirstSprite = firstSprite,
                FirstHealthBar = firstHealth,

                SecondSprite = secondSprite,
                SecondHealthBar = secondHealth,
            };

            var textures = new Texture2D[]
            {
                null,
                null,
                null,
                Content.Load<Texture2D>("BackgroundMenu"),
                null

            };
            var rectangles = new Rectangle[]
            {
                new Rectangle(), 
                new Rectangle(), 
                new Rectangle(), 
                new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height),
                new Rectangle(), 
            };
            var font = Content.Load<SpriteFont>("Narkisim");
            var gameNameItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width/2, 100),
                MenuItemName = "Mortal Kombat",
                MenuItemOrigin = font.MeasureString("Mortal Kombat")/2
            };
            font = Content.Load<SpriteFont>("Viner Hand ITC");
            var startItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 300),
                MenuItemName = "Start game",
                MenuItemOrigin = font.MeasureString("Start game") / 2
            };
            var helpItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 400),
                MenuItemName = "Help",
                MenuItemOrigin = font.MeasureString("Help") / 2
            };
            var exitItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 500),
                MenuItemName = "Exit",
                MenuItemOrigin = font.MeasureString("Exit") / 2
            };
            var menuItems = new MenuItem[]
            {
                startItem,
                helpItem,
                exitItem,
                gameNameItem
            };
            var texture = Content.Load<Texture2D>("BackgroundMenu");
            var rect = new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            menuScreen = new MenuScreen(this, menuItems, texture, rect);

            Components.Add(menuScreen);
            Components.Add(gameScreen);
            menuScreen.Show();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        //Обработка нажатий на клавиши
        void KeyboardHandle()
        {
            KeyboardState kbState = Keyboard.GetState();
            interval++;
            if (interval >= 10)
            {
                interval = 0;
                if (menuScreen.Enabled)
                {
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        currentMenuItem--;
                        if (currentMenuItem < 1)
                        {
                            currentMenuItem = 3;
                        }
                        menuScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Down))
                    {
                        currentMenuItem++;
                        if (currentMenuItem > 3)
                        {
                            currentMenuItem = 1;                    
                        }
                        menuScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        switch (currentMenuItem)
                        {
                            case 1:
                                menuScreen.Hide();
                                gameScreen.Show();
                                break;
                            /*case 2:
                                menuScreen.Hide();
                                helpScreen.Show();
                                break;*/
                            case 3:
                                Exit();
                                break;
                        }
                    }
                }
            }
            /*if (helpScreen.Enabled)
            {
                if (kbState.IsKeyDown(Keys.Escape))
                {
                    helpScreen.Hide();
                    menuScreen.Show();
                }
            }*/
            if (gameScreen.Enabled)
            {
                if (kbState.IsKeyDown(Keys.Escape))
                {
                    gameScreen.Hide();
                    menuScreen.Show();
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardHandle();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
