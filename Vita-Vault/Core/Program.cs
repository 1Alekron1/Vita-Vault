﻿namespace Vita_Vault.Core
{
    public static class Program
    {
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}