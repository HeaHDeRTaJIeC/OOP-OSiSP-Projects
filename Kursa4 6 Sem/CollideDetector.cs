using System;
using Microsoft.Xna.Framework;

namespace MortalKombatXI
{
    class CollideDetector
    {
        private static readonly String[,] HitActions =
        {
            {null, null, null, null, null, "AnimateHit"     , "AnimateHit"      , "AnimateHit"      , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, "AnimateHitBlock", "AnimateHitBlock" , "AnimateHitBlock" , "AnimateHit"          , "AnimateHit"          , "AnimateHitBlock"},
            {null, null, null, null, null, null             , null              , null              , "AnimateHitDown"      , "AnimateHitDown"      , "AnimateHitDown"},
            {null, null, null, null, null, null             , null              , null              , "AnimateHitDownBlock" , "AnimateHitDownBlock" , "AnimateHitDownBlock"},
            {null, null, null, null, null, "AnimateHit"     , "AnimateHit"      , "AnimateHit"      , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, "AnimateHit"     , "AnimateHit"      , "AnimateHit"      , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, "AnimateHit"     , "AnimateHit"      , "AnimateHit"      , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, "AnimateHit"     , "AnimateHit"      , "AnimateHit"      , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, null             , null              , null              , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, null             , null              , null              , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},
            {null, null, null, null, null, null             , null              , null              , "AnimateHit"          , "AnimateHit"          , "AnimateHit"},

        };

        public static void RepairMoveCollision(SpriteAnimation first, SpriteAnimation second, int width)
        {
            if (first.Information.CurrentState == PlayerState.Move)
            {
                if (first.moveDelta < 0)
                {
                    if (first.Information.Position > 100)
                        first.Position = new Vector2(first.Position.X + first.moveDelta, first.Position.Y);
                }
                else
                {
                    if (second.Information.Position - first.Information.Position - first.moveDelta > 0)
                        first.Position = new Vector2(first.Position.X + first.moveDelta, first.Position.Y);
                }
            }
            if (second.Information.CurrentState == PlayerState.Move)
            {
                if (second.moveDelta > 0)
                {
                    if (second.Information.Position < width - 20)
                        second.Position = new Vector2(second.Position.X + second.moveDelta, second.Position.Y);
                }
                else
                {
                        if (second.Information.Position - first.Information.Position + second.moveDelta > 0)
                    second.Position = new Vector2(second.Position.X + second.moveDelta, second.Position.Y);                    
                }
            }

        }

        public static void HitCollision(SpriteAnimation first, SpriteAnimation second, GameTime gameTime)
        {
            String method = HitActions[(int)first.Information.CurrentState, (int)second.Information.CurrentState];
            if (method != null)
            {
                if (second.Information.Position - second.Information.Range - first.Information.Position < 15)
                {
                    first.GetType().GetMethod(method).Invoke(first, new object[] {gameTime});
                    if (method != "AnimateHitBlock" && method != "AnimateHitDownBlock")
                        if (!first.Information.IsHited)
                        {
                            first.Information.Health -= second.Information.Damage;
                            first.Information.IsHited = true;
                        }
                }
            }
            else
                first.Information.IsHited = false;
            method = HitActions[(int) second.Information.CurrentState, (int) first.Information.CurrentState];
            if (method != null)
            {
                if (second.Information.Position - first.Information.Position - first.Information.Range < 15)
                {
                    second.GetType().GetMethod(method).Invoke(second, new object[] {gameTime});
                    if (method != "AnimateHitBlock" && method != "AnimateHitDownBlock")
                        if (!second.Information.IsHited)
                        {
                            second.Information.Health -= first.Information.Damage;
                            second.Information.IsHited = true;
                        }
                }
            }
            else
                second.Information.IsHited = false;
        }
    }
}
