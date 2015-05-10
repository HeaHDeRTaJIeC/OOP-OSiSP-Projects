using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MortalKombatXI
{
    class AnimateSprite
    {
        ConcurrentDictionary<string, Texture2D> spriteTexture;
        private Texture2D currentTexture;
        private delegate void Action(GameTime gameTime);
        private Action prevAction;
        float timer;
        private readonly float interval = GameConstants.AnimationInterval;
        private bool animateInProgress;
        private bool startMove;
        private bool currentMove;
        private bool isBlock = false;
        private bool isSitDown = false;
        int currentFrame;
        readonly int spriteWidth;
        readonly int spriteHeight;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;
        KeyboardState currentKbState;
        public GameMoves moves;


        public AnimateSprite(ConcurrentDictionary<string, Texture2D> texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
	        this.Texture = texture;
	        this.currentFrame = currentFrame;
	        this.spriteWidth = spriteWidth;
	        this.spriteHeight = spriteHeight;
        }

        public Texture2D CurrentTexture()
        {
            return currentTexture;
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            currentKbState = Keyboard.GetState();
            SourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            
            if (!animateInProgress)
            {
                currentMove = false;

                if (!isBlock)
                {
                    //Sit
                    if (currentKbState.IsKeyDown(moves.MoveDown))
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
                    if (currentKbState.IsKeyDown(moves.MoveUp))
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
                    if (currentKbState.IsKeyDown(moves.Block))
                    {
                        if (!isBlock)
                        {
                            currentFrame = 0;
                            isBlock = true;
                        }
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
                    if (currentKbState.IsKeyDown(moves.HandHitLeft) 
                        || currentKbState.IsKeyDown(moves.HandHitRight))
                    {
                        prevAction = AnimateHandSitHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateHandSitHit(gameTime);
                    }

                    //Uppercode hit
                    if (currentKbState.IsKeyDown(moves.UppercodeHit))
                    {
                        prevAction = AnimateUppercodeSitHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateUppercodeSitHit(gameTime);
                    }

                    if (!isBlock)
                        AnimateSit(gameTime);
                }
                else
                {
                    //Make block
                    if (currentKbState.IsKeyDown(moves.Block))
                    {
                        if (!isBlock)
                            currentFrame = 0;
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
                    //Ground hit
                    if (currentKbState.IsKeyDown(moves.LegGroundHit))   
                    {
                        prevAction = AnimateGroundHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateGroundHit(gameTime);
                    }

                    //Air hit
                    if (currentKbState.IsKeyDown(moves.LegAirHit))
                    {
                        prevAction = AnimateAirHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateAirHit(gameTime);
                    }

                    //Win move
                    if (currentKbState.IsKeyDown(moves.WinPose))
                    {
                        prevAction = AnimateWin;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateWin(gameTime);
                    }

                    //Move left
                    if (currentKbState.IsKeyDown(moves.MoveLeft))
                    {
                        if (!startMove)
                        {
                            currentFrame = 0;
                            startMove = true;
                        }
                        currentMove = true;
                        AnimateMoveLeft(gameTime);
                    }

                    //Move right
                    if (currentKbState.IsKeyDown(moves.MoveRight))
                    {
                        if (!startMove)
                        {
                            currentFrame = 8;
                            startMove = true;
                        }
                        currentMove = true;
                        AnimateMoveRight(gameTime);
                    }

                    //Left hand straight hit
                    if (currentKbState.IsKeyDown(moves.HandHitLeft))
                    {
                        prevAction = AnimateRightHandHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateRightHandHit(gameTime);
                    }

                    //Right hand straight hit
                    if (currentKbState.IsKeyDown(moves.HandHitRight))
                    {
                        prevAction = AnimateLeftHandHit;
                        animateInProgress = true;
                        currentFrame = 0;
                        AnimateLeftHandHit(gameTime);
                    }

                    //Uppercode hit
                    if (currentKbState.IsKeyDown(moves.UppercodeHit))
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
        }

        public void AnimateStay(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionStay"];
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
            currentTexture = Texture["ScorpionSit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentFrame = 0;

            if (timer > interval)
            {
                timer = 0f;
            }
        }

        public void AnimateWin(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionWin"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 6)
                {
                    animateInProgress = false;
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateMoveRight(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionMove"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                position.X += GameConstants.Step;

                if (currentFrame > 8)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateMoveLeft(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionMove"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame--;
                position.X -= GameConstants.Step;

                if (currentFrame < 0)
                {
                    currentFrame = 8;
                }
                timer = 0f;
            }
        }

        public void AnimateMakeBlock(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionBlock"];
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

        public void AnimateRemoveBlock(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionBlock"];
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

        public void AnimateMakeDownBlock(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionDownBlock"];
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

        public void AnimateRemoveDownBlock(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionDownBlock"];
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

        public void AnimateSitDown(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionSitDown"];
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
            currentTexture = Texture["ScorpionSitDown"];
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
                    isSitDown = false;
                }
                timer = 0f;
            }
        }

        public void AnimateRightHandHit(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionRightHandHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
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
            currentTexture = Texture["ScorpionLeftHandHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
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
            currentTexture = Texture["ScorpionUppercodeStayHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
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
            currentTexture = Texture["ScorpionUppercodeSitHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 5)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public void AnimateHandSitHit(GameTime gameTime)
        {
            currentTexture = Texture["ScorpionHandSitHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
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
            currentTexture = Texture["ScorpionGroundHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
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
            currentTexture = Texture["ScorpionAirHit"];
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 7)
                {
                    currentFrame = 0;
                    animateInProgress = false;
                }
                timer = 0f;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public ConcurrentDictionary<string, Texture2D> Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }


    }
}
