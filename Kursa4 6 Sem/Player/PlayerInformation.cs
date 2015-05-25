namespace MortalKombatXI
{
    class PlayerInformation
    {
        public int Position { get; set; }
        public int Range { get; private set; }
        public int Damage { get; private set; }
        public bool IsHited;
        private int health;
        public int Health 
        {
            get { return health; }
            set { health = value < 0 ? 0 : value; }
        }
        private PlayerState currentState;
        public PlayerState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                switch (value)
                {
                    case PlayerState.Stay:
                        Range = 0;
                        Damage = 0;
                        break;
                    case PlayerState.StayAndBlock:
                        Range = 0;
                        Damage = 0;
                        break;
                    case PlayerState.Sit:
                        Range = 0;
                        Damage = 0;
                        break;
                    case PlayerState.SitAndBlock:
                        Range = 0;
                        Damage = 0;
                        break;
                    case PlayerState.Move:
                        Range = 0;
                        Damage = 0;
                        break;
                    case PlayerState.StayAndHandHit:
                        Range = 40;
                        Damage = 1;
                        break;
                    case PlayerState.StayAndLegHit:
                        Range = 60;
                        Damage = 2;
                        break;
                    case PlayerState.StayAndUppercodeHit:
                        Range = 20;
                        Damage = 1;
                        break;
                    case PlayerState.SitAndHandHit:
                        Range = 40;
                        Damage = 1;
                        break;
                    case PlayerState.SitAndLegHit:
                        Range = 60;
                        Damage = 2;
                        break;
                    case PlayerState.SitAndUppercodeHit:
                        Range = 40;
                        Damage = 2;
                        break;
                }
            }
        }
    }
}
