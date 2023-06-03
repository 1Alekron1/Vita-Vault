using System.IO;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Core;
using Vita_Vault.Models;

namespace Vita_Vault.Managers;

internal abstract class InputManager
{
    internal static void Update(Player player, Shooting shooting)
    {
        var keyboardState = Keyboard.GetState();
        var mouseState = Mouse.GetState();
        player.Right = false;
        player.Left = false;
        player.jump = false;
        if (keyboardState.IsKeyUp(Keys.Escape)) Data.EscPressed = false;

        if (keyboardState.IsKeyDown(Keys.Escape) && !Data.EscPressed)
        {
            Data.CurrentState = Data.Scenes.Pause;
            var path = Constants.SavePath;
            if (!File.Exists(path)) File.Create(path).Dispose();
            using (TextWriter tw = new StreamWriter(path))
                tw.WriteLine($"{player.LastStablePosition.X} {player.LastStablePosition.Y}");
            Data.EscPressed = true;
            shooting.IsIdle = false;
            return;
        }

        if (keyboardState.IsKeyDown(Keys.D)) player.Right = true;
        if (keyboardState.IsKeyDown(Keys.A)) player.Left = true;
        if (keyboardState.IsKeyDown(Keys.W)) player.jump = true;
        switch (mouseState.LeftButton)
        {
            case ButtonState.Pressed:
                shooting.Shoot(player.Position, mouseState.Position.ToVector2());
                break;
            case ButtonState.Released:
                shooting.Stop();
                break;
        }
    }

    internal static void Update()
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyUp(Keys.Escape)) Data.EscPressed = false;
        if (!keyboardState.IsKeyDown(Keys.Escape) || Data.EscPressed) return;
        Data.CurrentState = Data.Scenes.Game;
        Data.EscPressed = true;
    }
}