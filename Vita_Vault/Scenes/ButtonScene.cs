using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Vita_Vault.Core;

namespace Vita_Vault.Scenes;

internal class ButtonScene : Component
{
    private int maxBtns;
    private Texture2D logo;
    private Texture2D background;
    private Texture2D[] btns;
    private Rectangle[] btnsRect;
    private SoundEffect tick;
    private int previousBtn = -1;
    private bool intersectsAny;

    private MouseState ms;
    private Rectangle msRect;
    internal override void LoadContent(ContentManager Content)
    {
        tick = Content.Load<SoundEffect>("tick");
    }

    internal override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        DrawOther(spriteBatch);
        DrawButtons(spriteBatch);
        spriteBatch.End();
    }

    internal void DrawOther(SpriteBatch spriteBatch)
    {
    }

    internal void DrawButtons(SpriteBatch spriteBatch)
    {
        intersectsAny = false;
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
    }
}