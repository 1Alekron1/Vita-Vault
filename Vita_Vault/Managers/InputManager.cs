using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Core;
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
            if (keyboardState.IsKeyDown(Keys.Right)) player.right = true;
            if (keyboardState.IsKeyDown(Keys.Left)) player.left = true;
            if (keyboardState.IsKeyDown(Keys.Up)) player.jump = true;
      }
}