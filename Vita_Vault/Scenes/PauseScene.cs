using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Core;
using Vita_Vault.Managers;

namespace Vita_Vault.Scenes;

internal class PauseScene : Component
{
    private Texture2D _solid;
    private Texture2D _solid1;
    private Texture2D[] _btns;
    private Rectangle[] _btnsRect;
    private MouseState _ms;
    private Rectangle _msRect;
    private const int MaxBtns = 2;
    private int _previousBtn = -1;
    private SoundEffect _tick;

    internal override void LoadContent(ContentManager content)
    {
        _solid = content.Load<Texture2D>("solid");
        _solid1 = content.Load<Texture2D>("solid1");
        _tick = content.Load<SoundEffect>("tick");
        const int incrementValue = 80;
        _btns = new Texture2D[MaxBtns];
        _btnsRect = new Rectangle[MaxBtns];
        for (var i = 0; i < MaxBtns + 1; i += 2)
        {
            var index = Math.Max(0, i - 1);
            _btns[index] = content.Load<Texture2D>($"btn{i + 1}");
            _btnsRect[index] = new Rectangle(Data.ScreenWidth / 2 - _btns[index].Width / 5,
                Data.ScreenHeight / 2 + incrementValue * i - 50, 2 * _btns[index].Width / 5,
                2 * _btns[index].Height / 5);
        }
    }

    internal override void Update(GameTime gameTime)
    {
        InputManager.Update();
        _ms = Mouse.GetState();
        _msRect = new Rectangle(_ms.X, _ms.Y, 1, 1);
        Data.PreviousState = Data.Scenes.Pause;
        switch (_ms.LeftButton)
        {
            case ButtonState.Pressed when _msRect.Intersects(_btnsRect[0]):
                Data.CurrentState = Data.Scenes.Game;
                break;
            case ButtonState.Pressed when _msRect.Intersects(_btnsRect[1]):
                Data.CurrentState = Data.Scenes.Menu;
                Data.MousePressed = true;
                break;
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_solid, new Vector2(0, 0), Color.White * 0.5f);
        spriteBatch.Draw(_solid1, new Rectangle(Data.ScreenWidth / 2 - 250, Data.ScreenHeight / 2 - 300, 500, 600),
            Color.Black);
        var intersectsAny = false;
        for (var i = 0; i < MaxBtns; i++)
        {
            spriteBatch.Draw(_btns[i], _btnsRect[i], Color.White);
            if (!_msRect.Intersects(_btnsRect[i])) continue;
            intersectsAny = true;
            spriteBatch.Draw(_btns[i], _btnsRect[i], Color.Gray);
            if (_previousBtn == i) continue;
            _tick.Play();
            _previousBtn = i;
        }

        if (!intersectsAny) _previousBtn = -1;
        spriteBatch.End();
    }
}