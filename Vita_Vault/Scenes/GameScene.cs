using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;
using Vita_Vault.Models;

namespace Vita_Vault.Scenes;

internal class GameScene : Component
{
    private Map _map; 
    private Texture2D background;
    
    internal override void LoadContent(ContentManager Content)
    {
        _map = new Map();
        _map.LoadContent(Content);
    }

    internal override void Update(GameTime gameTime)
    {
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        _map.Draw(spriteBatch);
    }
}