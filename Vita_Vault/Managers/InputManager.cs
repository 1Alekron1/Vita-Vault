using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Core;

namespace Vita_Vault.Managers;

internal class InputManager
{
      private static Vector2 _direction;
      public static bool Jump { get; private set; }
      public static Vector2 Direction => _direction;
      internal static void Update()
      {
            var keyboardState = Keyboard.GetState();
            _direction = Vector2.Zero;
            Jump = false;

            if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;
            if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
            if (keyboardState.IsKeyDown(Keys.Up)) Jump = true;
            
            if (_direction != Vector2.Zero) _direction.Normalize();
      }
}