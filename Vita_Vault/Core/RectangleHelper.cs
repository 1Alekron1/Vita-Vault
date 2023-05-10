using Microsoft.Xna.Framework;

namespace Vita_Vault.Core;

internal static class RectangleHelper
{
    private static int _leftRightOffset = 10;

    public static bool IsOnTopOf(this Rectangle r1, Rectangle r2) 
    {
        return (r1.Bottom >= r2.Top && r1.Top <= r2.Top && r1.Right >= r2.Left && r1.Left <= r2.Right &&
                r1.Bottom - r2.Top <= 10 && ((r1.Right - r2.Left >= _leftRightOffset && r1.Right <= r2.Right) ||
                                             (r2.Right - r1.Left >= _leftRightOffset && r1.Left >= r2.Left)));
    }

    public static bool IsOnLeftOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Right > r2.Left && r1.Left < r2.Left && r1.Bottom > r2.Top && r1.Top < r2.Bottom &&
                r1.Bottom - r2.Top > 10);
    }

    public static bool IsRightOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Right > r2.Right && r1.Left < r2.Right && r1.Bottom > r2.Top && r1.Top < r2.Bottom &&
                r1.Bottom - r2.Top > 10);
    }
}