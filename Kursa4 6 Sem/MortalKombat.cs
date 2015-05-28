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
        private GamePauseScreen gamePauseScreen;
        private GameRestartScreen gameRestartScreen;
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
            graphics.IsFullScreen = true;
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

            var scorpionDictionary = DictionaryLoader.GetDictionary(GameSettings.firstName, Content);
            var subZeroDictionary = DictionaryLoader.GetDictionary(GameSettings.secondName, Content);
            var firstHealth = new HealthBar(GameSettings.HealthBar, Content.Load<Texture2D>("DamageBar"));
            var secondHealth = new HealthBar(new Vector2(width - GameSettings.HealthBar.X, GameSettings.HealthBar.Y), Content.Load<Texture2D>("DamageBar"));
            var firstMoves = new PlayerControls();
            firstMoves.GetFirstPlayerKeyboard();
            var secondMoves = new PlayerControls();
            secondMoves.GetSecondPlayerKeyboard();
            
            //first player
            var firstSprite = new SpriteAnimation(GameSettings.SpriteWidth, GameSettings.SpriteHeigth, false)
            {
                SpriteTexture = scorpionDictionary,
                Moves = firstMoves,
                Position = new Vector2(GameSettings.FirstPosition.X - 100, GameSettings.FirstPosition.Y)
            };

            //second player
            var secondSprite = new SpriteAnimation(GameSettings.SpriteWidth, GameSettings.SpriteHeigth, true)
            {
                SpriteTexture = subZeroDictionary,
                Moves = secondMoves,
                Position = new Vector2(GameSettings.FirstPosition.X + 100, GameSettings.FirstPosition.Y)
            };

            //game screen
            var arenaRect = new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            var arenaTexture = Content.Load<Texture2D>("Arenas//TheDeadPoolArena");
            gameScreen = new GameScreen(this, Content.Load<SpriteFont>("Pristina"), Content.Load<SpriteFont>("Courier New"), width, arenaTexture, arenaRect)
            {
                FirstSprite = firstSprite,
                FirstHealthBar = firstHealth,

                SecondSprite = secondSprite,
                SecondHealthBar = secondHealth,
            };

            //menu screen
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
                MenuItemName = "New game",
                MenuItemOrigin = font.MeasureString("New game") / 2
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

            //help screen
            helpScreen = new HelpScreen(this, texture, rect, Content.Load<SpriteFont>("Courier new"), Content.Load<SpriteFont>("Viner Hand ITC"), firstMoves, secondMoves);

            //game pause screen
            font = Content.Load<SpriteFont>("Viner Hand ITC");
            var resumeItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 300),
                MenuItemName = "Resume",
                MenuItemOrigin = font.MeasureString("Resume") / 2
            };
            exitItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 400),
                MenuItemName = "Exit",
                MenuItemOrigin = font.MeasureString("Exit") / 2
            };
            menuItems = new MenuItem[]
            {
                resumeItem,
                exitItem
            };
            gamePauseScreen = new GamePauseScreen(this, menuItems);

            //game restart screen
            font = Content.Load<SpriteFont>("Viner Hand ITC");
            var restartItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 300),
                MenuItemName = "New game",
                MenuItemOrigin = font.MeasureString("New game") / 2
            };
            exitItem = new MenuItem
            {
                MenuFont = font,
                MenuItemPosition = new Vector2(width / 2, 400),
                MenuItemName = "Exit",
                MenuItemOrigin = font.MeasureString("Exit") / 2
            };
            menuItems = new MenuItem[]
            {
                restartItem,
                exitItem
            };
            gameRestartScreen = new GameRestartScreen(this, menuItems);

            Components.Add(menuScreen);
            Components.Add(gameScreen);
            Components.Add(helpScreen);
            Components.Add(gamePauseScreen);
            Components.Add(gameRestartScreen);
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
            if (interval >= 5)
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
                                GameState.StartNewGame();
                                gameScreen.Show();
                                break;
                            case 2:
                                menuScreen.Hide();
                                helpScreen.Show();
                                break;
                            case 3:
                                Exit();
                                break;
                        }
                    }
                }
                if (gamePauseScreen.Enabled)
                {
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        currentMenuItem--;
                        if (currentMenuItem < 1)
                        {
                            currentMenuItem = 2;
                        }
                        gamePauseScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Down))
                    {
                        currentMenuItem++;
                        if (currentMenuItem > 2)
                        {
                            currentMenuItem = 1;
                        }
                        gamePauseScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        switch (currentMenuItem)
                        {
                            case 1:
                                GameState.IsPaused = false;
                                gamePauseScreen.Hide();
                                break;
                            case 2:
                                GameState.IsPaused = false;
                                gamePauseScreen.Hide();
                                gameScreen.Hide();
                                menuScreen.Show();
                                currentMenuItem = 1;
                                break;
                        }
                    }
                }
                if (gameRestartScreen.Enabled)
                {
                    GameState.ShowMenu = false;
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        currentMenuItem--;
                        if (currentMenuItem < 1)
                        {
                            currentMenuItem = 2;
                        }
                        gameRestartScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Down))
                    {
                        currentMenuItem++;
                        if (currentMenuItem > 2)
                        {
                            currentMenuItem = 1;
                        }
                        gameRestartScreen.GetKey(currentMenuItem);
                    }
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        switch (currentMenuItem)
                        {
                            case 1:
                                GameState.IsPaused = false;
                                gameRestartScreen.Hide();
                                GameState.StartNewGame();
                                break;
                            case 2:
                                GameState.IsPaused = false;
                                gameRestartScreen.Hide();
                                gameScreen.Hide();
                                menuScreen.Show();
                                currentMenuItem = 1;
                                break;
                        }
                    }
                }
            }
            if (helpScreen.Enabled)
            {
                if (kbState.IsKeyDown(Keys.Escape))
                {
                    helpScreen.Hide();
                    menuScreen.Show();
                }
            }
            if (gameScreen.Enabled)
            {
                if (kbState.IsKeyDown(Keys.Escape))
                {
                    GameState.IsPaused = true;
                    gamePauseScreen.Show();
                    currentMenuItem = 1;
                }
                if (GameState.ShowMenu)
                {
                    GameState.IsPaused = true;
                    gameRestartScreen.Show();
                    currentMenuItem = 1;
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
