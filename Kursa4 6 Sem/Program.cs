namespace MortalKombatXI
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MortalKombat game = new MortalKombat())
            {
                game.Run();
            }
        }
    }
#endif
}

