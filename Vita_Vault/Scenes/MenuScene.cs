using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Vita_Vault.Core;

namespace Vita_Vault.Scenes;

internal class MenuScene : Component
{
    private const int MaxBtns = 4;
    private Texture2D background;
    private Texture2D logo;
    private Texture2D[] btns = new Texture2D[MaxBtns];
    private Rectangle[] btnsRect = new Rectangle[MaxBtns];
    private Song mainTheme;
    private SoundEffect tick;
    private int previousBtn = -1;

    private MouseState ms;
    private Rectangle msRect;
    internal override void LoadContent(ContentManager content)
    {
        background = content.Load<Texture2D>("bg");
        logo = content.Load<Texture2D>("logo");
        mainTheme = content.Load<Song>("main");
        tick = content.Load<SoundEffect>("tick");
        MediaPlayer.Play(mainTheme);
        MediaPlayer.IsRepeating = true;
        
        const int incrementValue = 80;
        for (int i = 0; i < MaxBtns; i++)
        {
            btns[i] = content.Load<Texture2D>($"btn{i}");
            btnsRect[i] = new Rectangle(Data.ScreenWidth / 2 - btns[i].Width / 5, Data.ScreenHeight / 2 + incrementValue * i - 50, 2 * btns[i].Width / 5,
                2 * btns[i].Height / 5);
        }
    }

    internal override void Update(GameTime gameTime)
    {
        ms = Mouse.GetState();
        msRect = new Rectangle(ms.X, ms.Y, 1, 1);

        if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnsRect[0]))
            Data.CurrentState = Data.Scenes.Game;
        else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnsRect[3]))
            Data.Exit = true;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
        spriteBatch.Draw(logo, new Vector2(Data.ScreenWidth / 2 - logo.Width / 2, Data.ScreenHeight / 2 - 350), Color.White);
        for (int i = 0; i < MaxBtns; i++)
        {
            spriteBatch.Draw(btns[i], btnsRect[i], Color.White);
            if (msRect.Intersects(btnsRect[i]))
            {
                spriteBatch.Draw(btns[i], btnsRect[i], Color.Gray);
                if (previousBtn != i)
                {
                    tick.Play();
                    previousBtn = i;
                }
            }
        }
    }
}