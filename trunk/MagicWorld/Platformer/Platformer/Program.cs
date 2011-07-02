using System;

namespace MagicWorld
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MagicWorldGame game = new MagicWorldGame())
            {
                game.Run();
            }
        }
    }
}

