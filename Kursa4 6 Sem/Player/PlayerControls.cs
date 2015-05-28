using Microsoft.Xna.Framework.Input;

namespace MortalKombatXI
{
    class PlayerControls
    {
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys Block { get; set; }
        public Keys HandHitLeft { get; set; }
        public Keys HandHitRight { get; set; }
        public Keys UppercodeHit { get; set; }
        public Keys LegHit { get; set; }

        public void GetFirstPlayerKeyboard()
        {
            MoveDown = Keys.S;
            MoveUp = Keys.W;
            MoveLeft = Keys.A;
            MoveRight = Keys.D;
            Block = Keys.Q;
            HandHitLeft = Keys.F;
            HandHitRight = Keys.G;
            LegHit = Keys.T;
            UppercodeHit = Keys.R;
        }

        public void GetSecondPlayerKeyboard()
        {
            MoveDown = Keys.Down;
            MoveUp = Keys.Up;
            MoveLeft = Keys.Left;
            MoveRight = Keys.Right;
            Block = Keys.I;
            HandHitLeft = Keys.K;
            HandHitRight = Keys.L;
            LegHit = Keys.P;
            UppercodeHit = Keys.O;
        }
    }
}
