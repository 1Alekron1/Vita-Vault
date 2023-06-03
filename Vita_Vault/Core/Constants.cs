using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vita_Vault.Core;

internal abstract class Constants
{
    public static Texture2D[] NoiseFrames;
    public static ContentManager Content { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static string DirectoryPath { get; set; }
    public static string SavePath { get; set; }
}