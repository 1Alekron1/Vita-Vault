namespace Vita_Vault.Core;

internal static class Data
{
    public static readonly int ScreenWidth = 1600;
    public static readonly int ScreenHeight = 900;
    public static bool Exit { get; set; }
    public static bool IsNewGame = false;
    public static bool IsNewGameStart = false;
    public static bool EscPressed = false;
    public static bool MousePressed = false;

    public enum Scenes
    {
        Menu,
        Game,
        Pause
    }

    public static Scenes CurrentState { get; set; } = Scenes.Menu;
    public static Scenes PreviousState { get; set; } = Scenes.Menu;
}