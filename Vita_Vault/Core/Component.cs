using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vita_Vault.Core;

internal abstract class Component
{
    internal abstract void LoadContent(ContentManager content);
    internal abstract void Update(GameTime gameTime);
    internal abstract void Draw(SpriteBatch spriteBatch);
}