using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;
using Vita_Vault.Scenes;

namespace Vita_Vault.Managers;

internal class GameStateManager : Component
{
    private MenuScene _ms;
    private GameScene _gs;
    private PauseScene _ps;

    internal override void LoadContent(ContentManager content)
    {
        _ms = new MenuScene();
        _ps = new PauseScene();
        _ms.LoadContent(content);
        _ps.LoadContent(content);
    }

    internal override void Update(GameTime gameTime)
    {
        switch (Data.CurrentState)
        {
            case Data.Scenes.Menu:
                _ms.Update(gameTime);
                break;
            case Data.Scenes.Game:
                _gs.Update(gameTime);
                break;
            case Data.Scenes.Pause:
                _ps.Update(gameTime);
                break;
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        switch (Data.CurrentState)
        {
            case Data.Scenes.Menu:
                if (Data.PreviousState == Data.Scenes.Pause) _ms.CalculateButtons();
                _ms.Draw(spriteBatch);
                break;
            case Data.Scenes.Game:
                if (Data.PreviousState == Data.Scenes.Menu) CreateLevel();
                _gs.Draw(spriteBatch);
                break;
            case Data.Scenes.Pause:
                _gs.Draw(spriteBatch);
                _ps.Draw(spriteBatch);
                break;
        }
    }

    private void CreateLevel()
    {
        if (Data.IsNewGameStart) File.WriteAllText(Constants.SavePath,string.Empty);
        _gs = new GameScene();
        _gs.LoadContent(Constants.Content);
        Data.PreviousState = Data.Scenes.Game;
        Data.IsNewGameStart = false;
    }
}