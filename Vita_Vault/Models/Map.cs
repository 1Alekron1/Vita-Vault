using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Map : Component
{
    public Vector2 MapSize;
    public Vector2 TileSize;
    public Vector2 LvlOffset;
    public TiledMap CurrentMap => _map;
    private TiledMap _map;
    private TiledTileset _tileSet;
    private Texture2D _tileSetTexture;
    private int _tileWidth;
    private int _tileHeight;
    private int _tileSetTilesWide;
    private int _upOffset;
    private int _downOffset;

    internal override void LoadContent(ContentManager content)
    {
        _map = new TiledMap(Path.Combine(Constants.DirectoryPath, "Content\\Map.tmx"));
        _tileSet = new TiledTileset(Path.Combine(Constants.DirectoryPath, "Content\\mapTileset.tsx"));
        _tileSetTexture = content.Load<Texture2D>("Terrain");
        _tileWidth = _tileSet.TileWidth;
        _tileHeight = _tileSet.TileHeight;
        _tileSetTilesWide = _tileSet.Columns;
        TileSize = new Vector2(_tileWidth, _tileHeight);
        MapSize = new Vector2(_map.Width, _map.Height) * TileSize;
    }

    internal override void Update(GameTime gameTime)
    {
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        for (var i = _upOffset; i < _downOffset; i++)
        {
            var gid = _map.Layers[0].data[i];
            if (gid == 0) continue;
            var tileFrame = gid - 1;

            var column = tileFrame % _tileSetTilesWide;
            var row = (int)Math.Floor(tileFrame / (double)_tileSetTilesWide);

            float x = (i % _map.Width) * _map.TileWidth;
            var y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

            var tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);
            var pos = new Rectangle((int)(x - LvlOffset.X), (int)(y - LvlOffset.Y), _tileWidth, _tileHeight);
            spriteBatch.Draw(_tileSetTexture, pos, tilesetRec,
                Color.White);
        }
    }

    internal void UpdateLoading(Player player)
    {
        var column = Math.Floor(player.Position.X / TileSize.X);
        var row = Math.Floor(player.Position.Y / TileSize.Y);
        var index = row * MapSize.X / TileSize.X + column;
        _upOffset = (int)Math.Max(0, index - 1440);
        _downOffset = (int)Math.Min(14400, index + 1500);
    }
}