namespace Vita_Vault.Core;

public static class Data
{
    public static int ScreenWidth { get; set; } = 1600;
    public static int ScreenHeight { get; set; } = 900;
    public static bool Exit { get; set; } = false;
    public static bool IsNewGame = false;
    public static bool EscPressed = false;

    public enum Scenes
    {
        Menu,
        Game,
        Pause
    }

    public static Scenes CurrentState { get; set; } = Scenes.Menu;
}