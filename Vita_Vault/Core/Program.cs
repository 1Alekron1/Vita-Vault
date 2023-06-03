namespace Vita_Vault.Core
{
    internal static class Program
    {
        internal static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}