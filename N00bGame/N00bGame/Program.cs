using System;

namespace N00bGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (N00bGame game = new N00bGame())
            {
                game.Run();
            }
        }
    }
#endif
}

