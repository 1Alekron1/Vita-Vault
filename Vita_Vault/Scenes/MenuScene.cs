using System.IO;
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
    private int _maxBtns;
    private Texture2D _background;
    private Texture2D _logo;
    private Texture2D[] _btns;
    private Rectangle[] _btnsRect;
    private Song _mainTheme;
    private SoundEffect _tick;
    private int _previousBtn = -1;
    private bool _intersectsAny;
    private MouseState _ms;
    private Rectangle _msRect;

    public void CalculateButtons()
    {
        var path = Constants.SavePath;
        if (!File.Exists(path) || new FileInfo(path).Length == 0) Data.IsNewGame = true;
        else Data.IsNewGame = false;
        _maxBtns = Data.IsNewGame ? 3 : 4;
        _btns = new Texture2D[_maxBtns];
        _btnsRect = new Rectangle[_maxBtns];

        const int incrementValue = 80;
        for (var i = 0; i < _maxBtns; i++)
        {
            var index = _maxBtns == 3 && i >= 1 ? i + 1 : i;
            _btns[i] = Constants.Content.Load<Texture2D>($"btn{index}");
            _btnsRect[i] = new Rectangle(Data.ScreenWidth / 2 - _btns[i].Width / 5,
                Data.ScreenHeight / 2 + incrementValue * i - 50, 2 * _btns[i].Width / 5,
                2 * _btns[i].Height / 5);
        }
    }

    internal override void LoadContent(ContentManager content)
    {
        _background = content.Load<Texture2D>("bg");
        _logo = content.Load<Texture2D>("logo");
        _mainTheme = content.Load<Song>("main");
        _tick = content.Load<SoundEffect>("tick");
        MediaPlayer.Play(_mainTheme);
        MediaPlayer.IsRepeating = true;
        CalculateButtons();
    }

    internal override void Update(GameTime gameTime)
    {
        _ms = Mouse.GetState();
        _msRect = new Rectangle(_ms.X, _ms.Y, 1, 1);

        switch (_ms.LeftButton)
        {
            case ButtonState.Released:
                Data.MousePressed = false;
                break;
            case ButtonState.Pressed when _msRect.Intersects(_btnsRect[0]) && !Data.MousePressed:
                Data.CurrentState = Data.Scenes.Game;
                Data.PreviousState = Data.Scenes.Menu;
                Data.IsNewGameStart = true;
                break;
        }

        switch (_ms.LeftButton)
        {
            case ButtonState.Pressed when _msRect.Intersects(_btnsRect[1]) && !Data.MousePressed && _maxBtns == 4:
                Data.CurrentState = Data.Scenes.Game;
                Data.PreviousState = Data.Scenes.Menu;
                break;
            case ButtonState.Pressed when _msRect.Intersects(_btnsRect[_maxBtns - 1]) && !Data.MousePressed:
                Data.Exit = true;
                break;
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
        spriteBatch.Draw(_logo, new Vector2(Data.ScreenWidth / 2 - _logo.Width / 2, Data.ScreenHeight / 2 - 350),
            Color.White);
        _intersectsAny = false;
        for (var i = 0; i < _maxBtns; i++)
        {
            spriteBatch.Draw(_btns[i], _btnsRect[i], Color.White);
            if (!_msRect.Intersects(_btnsRect[i])) continue;
            _intersectsAny = true;
            spriteBatch.Draw(_btns[i], _btnsRect[i], Color.Gray);
            if (_previousBtn == i) continue;
            _tick.Play();
            _previousBtn = i;
        }

        if (!_intersectsAny) _previousBtn = -1;
        spriteBatch.End();
    }
}