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
    private Texture2D[] btns;
    private Rectangle[] btnsRect;
    private MouseState ms;
    private Rectangle msRect;
    private int maxBtns = 2;
    private int previousBtn = -1;
    private SoundEffect tick;

    internal override void LoadContent(ContentManager Content)
    {
        _solid = Content.Load<Texture2D>("solid");
        tick =  Content.Load<SoundEffect>("tick");
        const int incrementValue = 80;
        btns = new Texture2D[maxBtns];
        btnsRect = new Rectangle[maxBtns];
        for (int i = 0; i < maxBtns + 1; i += 2)
        {
            var index = Math.Max(0, i - 1);
            btns[index] = Content.Load<Texture2D>($"btn{i + 1}");
            btnsRect[index] = new Rectangle(Data.ScreenWidth / 2 - btns[index].Width / 5,
                Data.ScreenHeight / 2 + incrementValue * i - 50, 2 * btns[index].Width / 5,
                2 * btns[index].Height / 5);
        }
    }

    internal override void Update(GameTime gameTime)
    {
        InputManager.Update();
        ms = Mouse.GetState();
        msRect = new Rectangle(ms.X, ms.Y, 1, 1);

        if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnsRect[0]))
            Data.CurrentState = Data.Scenes.Game;
        else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnsRect[1]))
            Data.CurrentState = Data.Scenes.Menu;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_solid, new Vector2(0, 0), Color.White * 0.5f);
        var intersectsAny = false;
        for (int i = 0; i < maxBtns; i++)
        {
            spriteBatch.Draw(btns[i], btnsRect[i], Color.White);
            if (msRect.Intersects(btnsRect[i]))
            {
                intersectsAny = true;
                spriteBatch.Draw(btns[i], btnsRect[i], Color.Gray);
                if (previousBtn != i)
                {
                    tick.Play();
                    previousBtn = i;
                }
            }
        }

        if (!intersectsAny) previousBtn = -1;
        spriteBatch.End();
    }
}