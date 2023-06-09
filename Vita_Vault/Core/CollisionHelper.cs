﻿using System;
using Microsoft.Xna.Framework;
using Vita_Vault.Models;

namespace Vita_Vault.Core;

internal static class CollisionHelper
{
    internal static bool CanMoveHere(float x, float y, int width, int height, Map map)
    {
        return !IsSolid(x, y, map) && !IsSolid(x + width, y + height, map) && !IsSolid(x + width, y, map) &&
               !IsSolid(x, y + height, map);
    }

    private static bool IsSolid(float x, float y, Map map)
    {
        if (x < 0 || x >= map.MapSize.X) return true;
        if (y < 0 || y >= map.MapSize.Y) return true;

        var column = Math.Floor(x / map.TileSize.X);
        var row = Math.Floor(y / map.TileSize.Y);
        var index = row * map.MapSize.X / map.TileSize.X + column;
        var value = map.CurrentMap.Layers[0].data[(int)index];
        return value != 0;
    }

    internal static float GetPosNextToWall(Rectangle hitBox, float xSpeed, Map map)
    {
        var currentTile = (int)(hitBox.X / map.TileSize.X);
        if (xSpeed <= 0) return currentTile * map.TileSize.X;
        var tileXPos = currentTile * map.TileSize.X;
        var xOffset = (int)(map.TileSize.X - hitBox.Width);
        return tileXPos + xOffset - 1;

    }

    internal static float GetPosNextToRoofOrFloor(Rectangle hitBox, float airSpeed, Map map)
    {
        var currentTile = (int)((hitBox.Y + hitBox.Height) / map.TileSize.Y);
        if (airSpeed <= 0) return (int)(hitBox.Y / map.TileSize.Y) * map.TileSize.Y;
        var tileYPos = (currentTile) * map.TileSize.Y;
        var yOffset = (int)(map.TileSize.Y - hitBox.Height);
        return tileYPos + yOffset - 1;

    }

    internal static bool OnFloor(Rectangle hitBox, Map map)
    {
        return IsSolid(hitBox.X, hitBox.Y + hitBox.Height + 1, map) ||
               IsSolid(hitBox.X + hitBox.Width, hitBox.Y + hitBox.Height + 1, map);
    }
}