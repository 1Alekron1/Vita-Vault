using Microsoft.Xna.Framework;

namespace Vita_Vault.Core;

internal static class RectangleHelper
{
    public static bool IsOnTopOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Bottom >= r2.Top && r1.Top <= r2.Bottom && r1.Right >= r2.Left && r1.Left <= r2.Right);
    }
}