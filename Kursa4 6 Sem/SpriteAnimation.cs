using System.Collections.Generic;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MortalKombatXI
{
    class SpriteAnimation
    {
        private int currentFrame;
        public int moveDelta { get; private set; }
        public Texture2D CurrentTexture { get; private set; }
        private delegate void Action(GameTime gameTime);
        private Action prevAction;
        float timer;
        private readonly float interval = GameSettings.AnimationInterval;
        private bool animateInProgress;
        private bool startMove;
        private bool currentMove;
        private bool isBlock;
        private bool isSitDown;
        private readonly bool invertMove; 
        readonly int spriteWidth;
        readonly int spriteHeight;
        KeyboardState currentKbState;
        public PlayerControls Moves { get; set; }
        public PlayerInformation Information { get; set; }
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (invertMove)
                    Information.Position = (int) value.X + 80;
                else
                    Information.Position = (int) value.X;
            }
        }
        public Vector2 Origin { get; set; }
        public Dictionary<string, Texture2D> SpriteTexture { get; set; }
        public Rectangle SourceRect { get; set; }


        public SpriteAnimation(int spriteWidth, int spriteHeight, bool invertMove)
        {
	        this.spriteWidth = spriteWidth;
	        this.spriteHeight = spriteHeight;
            this.invertMove = invertMove;
            Information = new PlayerInformation
            {
                CurrentState = new PlayerState(),
                Health = GameSettings.MaxHealth
            };
        }

        public PlayerState HandleSpriteMovement(GameTime gameTime)
        {
            moveDelta = 0;
            currentKbState = Keyboard.GetState();
            if (GameState.ShowRound || GameState.ShowWinner || GameState.IsEnd)
                currentKbState = new KeyboardState(Keys.None);
            SourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            if (!animateInProgress)
            {
                currentMove = false;

                if (!isBlock)
                {
                    //Sit
                    if (currentKbState.IsKeyDown(Moves.MoveDown))
                    {
                        if (!isSitDown)
                        {
                            currentFrame = 0;
                            prevAction = AnimateSitDown;
                            animateInProgress = true;
                            AnimateSitDown(gameTime);
                        }
                    }
                    //Stay
                    if (currentKbState.IsKeyDown(Moves.MoveUp))
                    {
                        if (isSitDown)
                        {
                            currentFrame = 2;
                            prevAction = AnimateStandUp;
                            animateInProgress = true;
                            AnimateStandUp(gameTime);
                        }
                    }
                }

                if (isSitDown)
                {
                    //Make block (sit)
                    if (currentKbState.IsKeyDown(Moves.Block))
                    {
                        currentFrame = isBlock ? 1 : 0;
                        isBlock = true;
                        AnimateMakeDownBlock(gameTime);
                    }
                    //Remove block (sit)
                    else if (isBlock)
                    {
                        prevAction = AnimateRemoveDownBlock;
                        animateInProgress = true;
                        currentFrame = 1;
                        AnimateRemoveDownBlock(gameTime);
                    }

                    //Hand hit
                    if (currentKbState.IsKeyDown(Moves.HandHitLeft) 
                        || currentKbState.IsKeyDown(Moves.HandHitRight))
                    {
                        prevAction = AnimateHandSitHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateHandSitHit(gameTime);
                    }

                    //Uppercode hit
                    if (currentKbState.IsKeyDown(Moves.UppercodeHit))
                    {
                        prevAction = AnimateUppercodeSitHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateUppercodeSitHit(gameTime);
                    }

                    //Ground hit
                    if (currentKbState.IsKeyDown(Moves.LegHit))
                    {
                        prevAction = AnimateGroundHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateGroundHit(gameTime);
                    }

                    if (!isBlock)
                        AnimateSit(gameTime);
                }
                else
                {
                    //Make block
                    if (currentKbState.IsKeyDown(Moves.Block))
                    {
                        currentFrame = isBlock ? 1 : 0;
                        isBlock = true;
                        
                        AnimateMakeBlock(gameTime);
                    }
                    //Remove block
                    else if (isBlock)
                    {
                        prevAction = AnimateRemoveBlock;
                        animateInProgress = true;
                        currentFrame = 1;
                        AnimateRemoveBlock(gameTime);
                    }
                }

                

                if (!isBlock && !isSitDown)
                {
                    //Air hit
                    if (currentKbState.IsKeyDown(Moves.LegHit))
                    {
                        prevAction = AnimateAirHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateAirHit(gameTime);
                    }

                    //Win move
                    if (currentKbState.IsKeyDown(Moves.WinPose))
                    {
                        prevAction = AnimateWin;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateWin(gameTime);
                    }

                    //Move left
                    if (currentKbState.IsKeyDown(Moves.MoveLeft))
                    {
                        if (!invertMove)
                        {
                            if (!startMove)
                            {
                                currentFrame = 0;
                                startMove = true;
                            }
                            currentMove = true;
                            AnimateMoveLeft(gameTime);
                        }
                        else
                        {
                            if (!startMove)
                            {
                                currentFrame = 8;
                                startMove = true;
                            }
                            currentMove = true;
                            AnimateMoveRight(gameTime);
                        }
                    }

                    //Move right
                    if (currentKbState.IsKeyDown(Moves.MoveRight))
                    {
                        if (!invertMove)
                        {
                            if (!startMove)
                            {
                                currentFrame = 8;
                                startMove = true;
                            }
                            currentMove = true;
                            AnimateMoveRight(gameTime);
                        }
                        else
                        {
                            if (!startMove)
                            {
                                currentFrame = 0;
                                startMove = true;
                            }
                            currentMove = true;
                            AnimateMoveLeft(gameTime);
                        }
                    }

                    //Left hand straight hit
                    if (currentKbState.IsKeyDown(Moves.HandHitLeft))
                    {
                        prevAction = AnimateRightHandHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateRightHandHit(gameTime);
                    }

                    //Right hand straight hit
                    if (currentKbState.IsKeyDown(Moves.HandHitRight))
                    {
                        prevAction = AnimateLeftHandHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateLeftHandHit(gameTime);
                    }

                    //Uppercode hit
                    if (currentKbState.IsKeyDown(Moves.UppercodeHit))
                    {
                        prevAction = AnimateUppercodeStayHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateUppercodeStayHit(gameTime);
                    }



                    if (!currentMove)
                    {
                        if (startMove)
                        {
                            startMove = false;
                            currentFrame = 0;
                        }
                        AnimateStay(gameTime);
                    }
                }
            }
            else
            {
                prevAction(gameTime);
            }
            

            Origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            return Information.CurrentState;
        }

        public void Win(GameTime gameTime)
        {
            prevAction = AnimateWin;
            animateInProgress = true;
        }

        public void Stop()
        {
            animateInProgress = false;
        }

        public void AnimateStay(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Stay;
            CurrentTexture = SpriteTexture["Stay"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 6)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateSit(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Sit;
            CurrentTexture = SpriteTexture["Sit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 0;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateWin(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Stay;
            CurrentTexture = SpriteTexture["Win"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 6)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateMoveRight(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Move;
            CurrentTexture = SpriteTexture["Move"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (!invertMove)
                {
                    moveDelta = GameSettings.Step;
                }
                else
                {
                    moveDelta = -GameSettings.Step;
                }

                if (currentFrame > 8)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateMoveLeft(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Move;
            CurrentTexture = SpriteTexture["Move"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame--;
                if (!invertMove)
                {
                    moveDelta = -GameSettings.Step;
                }
                else
                {
                    moveDelta = GameSettings.Step;
                }

                if (currentFrame < 0)
                {
                    currentFrame = 8;
                }
                timer = 0f;
            }
        }

        public void AnimateMakeBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.StayAndBlock;
            CurrentTexture = SpriteTexture["Block"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame < 1)
                {
                    currentFrame++;
                }
                timer = 0f;
            }
        }

        public void AnimateHitBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.StayAndBlock;
            CurrentTexture = SpriteTexture["Block"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 2;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateRemoveBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Stay;
            CurrentTexture = SpriteTexture["Block"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame > 0)
                {
                    currentFrame--;
                }
                else
                {
                    animateInProgress = false;
                    isBlock = false;
                }
                timer = 0f;
            }
        }

        public void AnimateHit(GameTime gameTime)
        {
            animateInProgress = false;
            Information.CurrentState = PlayerState.Stay;
            CurrentTexture = SpriteTexture["Stay"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 0;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateMakeDownBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.SitAndBlock;
            CurrentTexture = SpriteTexture["DownBlock"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame < 1)
                {
                    currentFrame++;
                }
                timer = 0f;
            }
        }

        public void AnimateHitDownBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.SitAndBlock;
            CurrentTexture = SpriteTexture["DownBlock"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 2;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateRemoveDownBlock(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Sit;
            CurrentTexture = SpriteTexture["DownBlock"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame > 0)
                {
                    currentFrame--;
                }
                else
                {
                    animateInProgress = false;
                    isBlock = false;
                }
                timer = 0f;
            }
        }

        public void AnimateHitDown(GameTime gameTime)
        {
            animateInProgress = false;
            Information.CurrentState = PlayerState.Sit;
            CurrentTexture = SpriteTexture["Sit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 0;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateSitDown(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Sit;
            CurrentTexture = SpriteTexture["SitDown"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame < 2)
                {
                    currentFrame++;
                }
                else
                {
                    animateInProgress = false;
                    isSitDown = true;
                }
                timer = 0f;
            }
        }

        public void AnimateStandUp(GameTime gameTime)
        {
            Information.CurrentState = PlayerState.Sit;
            CurrentTexture = SpriteTexture["SitDown"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (currentFrame > 0)
                {
                    currentFrame--;
                }
                else
                {
                    animateInProgress = false;
                    Information.CurrentState = PlayerState.Stay;
                    isSitDown = false;
                }
                timer = 0f;
            }
        }

        public void AnimateRightHandHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["RightHandHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 2)
                {
                    Information.CurrentState = PlayerState.StayAndHandHit;
                }
                else
                {
                    Information.CurrentState = PlayerState.Stay;
                }
                if (currentFrame > 4)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateLeftHandHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["LeftHandHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 2)
                {
                    Information.CurrentState = PlayerState.StayAndHandHit;
                }
                else
                {
                    Information.CurrentState = PlayerState.Stay;
                }
                if (currentFrame > 4)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateUppercodeStayHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["UppercodeStayHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 2)
                    Information.CurrentState = PlayerState.StayAndUppercodeHit;
                else
                    Information.CurrentState = PlayerState.Stay;
                if (currentFrame > 3)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateUppercodeSitHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["UppercodeSitHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 2)
                    Information.CurrentState = PlayerState.SitAndUppercodeHit;
                else
                    if (currentFrame > 4)
                        Information.CurrentState = PlayerState.Stay;
                    else
                        Information.CurrentState = PlayerState.Sit;

                if (currentFrame > 5)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                    isSitDown = false;
                }
                timer = 0f;
            }
        }

        public void AnimateHandSitHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["HandSitHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 2)
                {
                    Information.CurrentState = PlayerState.SitAndHandHit;
                }
                else
                {
                    Information.CurrentState = PlayerState.Sit;
                }
                if (currentFrame > 4)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateGroundHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["GroundHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 4)
                    Information.CurrentState = PlayerState.SitAndLegHit;
                else
                    Information.CurrentState = PlayerState.Sit;
                if (currentFrame > 7)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateAirHit(GameTime gameTime)
        {
            CurrentTexture = SpriteTexture["AirHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == 4)
                    Information.CurrentState = PlayerState.StayAndLegHit;
                else
                    Information.CurrentState = PlayerState.Stay;
                if (currentFrame > 7)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void Routine()
        {
            isSitDown = false;
            isBlock = false;
            animateInProgress = false;
        }
    }
}
