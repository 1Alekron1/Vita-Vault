using Microsoft.Xna.Framework;

namespace Vita_Vault.Core;

internal static class RectangleHelper
{
    private static int _horizontalOffset = 10;
    private static int _VerticalOffset = 15;

    public static bool IsOnTopOf(this Rectangle r1, Rectangle r2) 
    {
        return (r1.Bottom >= r2.Top && r1.Top <= r2.Top && (r1.Right >= r2.Left && r1.Left <= r2.Right) &&
                r1.Bottom - r2.Top <= _VerticalOffset && ((r1.Right - r2.Left >= _horizontalOffset && r1.Right <= r2.Right) ||
                                                          (r2.Right - r1.Left >= _horizontalOffset && r1.Left >= r2.Left)));
    }

    public static bool IsOnBottomOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Top < r2.Bottom && r1.Bottom > r2.Bottom && (r1.Right >= r2.Left && r1.Left <= r2.Right) &&
                r2.Bottom - r1.Top <= _VerticalOffset && ((r1.Right - r2.Left >= _horizontalOffset && r1.Right <= r2.Right) ||
                                                          (r2.Right - r1.Left >= _horizontalOffset && r1.Left >= r2.Left)));
    }
    public static bool IsOnLeftOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Right > r2.Left && r1.Left < r2.Left && r1.Bottom > r2.Top && r1.Top < r2.Bottom &&
                r1.Bottom - r2.Top > _VerticalOffset);
    }

    public static bool IsRightOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Right > r2.Right && r1.Left < r2.Right && r1.Bottom > r2.Top && r1.Top < r2.Bottom &&
                r1.Bottom - r2.Top > _VerticalOffset);
    }
}