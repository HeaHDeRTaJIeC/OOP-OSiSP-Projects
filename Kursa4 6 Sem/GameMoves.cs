using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MortalKombatXI
{
    class GameMoves
    {
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys Block { get; set; }
        public Keys HandHitLeft { get; set; }
        public Keys HandHitRight { get; set; }
        public Keys UppercodeHit { get; set; }
        public Keys LegGroundHit { get; set; }
        public Keys LegAirHit { get; set; }
        public Keys WinPose { get; set; }
    }
}
