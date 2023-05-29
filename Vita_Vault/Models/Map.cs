using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Map : Component
{
    private TiledMap _map;
    private TiledTileset _tileset;
    private Texture2D _tilesetTexture;
    public HashSet<Rectangle> CollisionArray;
    public Vector2 MapSize;
    public Vector2 TileSize;
    public TiledMap CurrentMap => _map;
    private int _tileWidth;
    private int _tileHeight;
    private int _tilesetTilesWide;
    public Vector2 LvlOffset;

    internal override void LoadContent(ContentManager Content)
    {
        _map = new TiledMap("C:\\Users\\644\\OneDrive\\Рабочий стол\\Vita_Vault\\Vita_Vault\\Content\\Map.tmx");
        _tileset = new TiledTileset("C:\\Users\\644\\OneDrive\\Рабочий стол\\Vita_Vault\\Vita_Vault\\Content\\mapTileset.tsx");
        _tilesetTexture = Content.Load<Texture2D>("Terrain");
        _tileWidth = _tileset.TileWidth;
        _tileHeight = _tileset.TileHeight;
        _tilesetTilesWide = _tileset.Columns;
        CollisionArray = new ();
        TileSize = new Vector2(_tileWidth, _tileHeight);
        MapSize = new Vector2(_map.Width, _map.Height) * TileSize;
    }

    internal override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        CollisionArray.Clear();
        for (var i = 0; i < _map.Layers[0].data.Length; i++)
        {
            int gid = _map.Layers[0].data[i];
            if (gid == 0) continue;
            int tileFrame = gid - 1;

            int column = tileFrame % _tilesetTilesWide;
            int row = (int)Math.Floor(tileFrame / (double)_tilesetTilesWide);

            float x = (i % _map.Width) * _map.TileWidth;
            float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

            Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);
            Rectangle pos = new Rectangle((int)(x - LvlOffset.X), (int)(y - LvlOffset.Y), _tileWidth, _tileHeight);
            CollisionArray.Add(pos);
            spriteBatch.Draw(_tilesetTexture, pos, tilesetRec,
                Color.White);
        }
    }
}