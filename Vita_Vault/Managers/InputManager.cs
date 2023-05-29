using Microsoft.Xna.Framework.Input;
using Vita_Vault.Models;

namespace Vita_Vault.Managers;

internal class InputManager
{
      internal static void Update(Player player)
      {
            var keyboardState = Keyboard.GetState();
            player.right = false;
            player.left = false;
            player.jump = false;
            if (keyboardState.IsKeyDown(Keys.D)) player.right = true;
            if (keyboardState.IsKeyDown(Keys.A)) player.left = true;
            if (keyboardState.IsKeyDown(Keys.W)) player.jump = true;
      }
}