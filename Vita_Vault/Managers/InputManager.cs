﻿using Microsoft.Xna.Framework.Input;
using Vita_Vault.Core;
using Vita_Vault.Models;

namespace Vita_Vault.Managers;

internal class InputManager
{
      internal static void Update(Player player, Shooting shooting)
      {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            player.right = false;
            player.left = false;
            player.jump = false;
            if (keyboardState.IsKeyUp(Keys.Escape)) Data.EscPressed = false;
            if (keyboardState.IsKeyDown(Keys.Escape) && !Data.EscPressed)
            {
                  Data.CurrentState = Data.Scenes.Pause;
                  Data.EscPressed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D)) player.right = true;
            if (keyboardState.IsKeyDown(Keys.A)) player.left = true;
            if (keyboardState.IsKeyDown(Keys.W)) player.jump = true;
            if (mouseState.LeftButton == ButtonState.Pressed)
                  shooting.Shoot(player.Position, mouseState.Position.ToVector2());
            if (mouseState.LeftButton == ButtonState.Released) shooting.Stop();
      }

      internal static void Update()
      {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyUp(Keys.Escape)) Data.EscPressed = false;
            if (keyboardState.IsKeyDown(Keys.Escape) && !Data.EscPressed)
            {
                  Data.CurrentState = Data.Scenes.Game;
                  Data.EscPressed = true;
            }
      }
}