﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;
using Vita_Vault.Managers;
using Vita_Vault.Models;

namespace Vita_Vault.Scenes;

internal class GameScene : Component
{
    private Map _map;
    private Player _player;
    private Shooting _shooting;
    private Texture2D _background;
    private Texture2D _bigClouds;

    private float _xLvlOffset;
    private float _yLvlOffset;
    private float _leftBorder;
    private float _rightBorder;
    private float _bottomBorder;
    private float _upBorder;
    private float _lvlTilesWide;
    private float _lvlTilesHigh;
    private float _maxTilesOffsetX;
    private float _maxTilesOffsetY;
    private float _maxLvlOffsetX;
    private float _maxLvlOffsetY;

    internal override void LoadContent(ContentManager content)
    {
        GenerateNoise();
        _map = new Map();
        _map.LoadContent(content);
        _player = new Player();
        _player.LoadContent(content);
        _shooting = new Shooting();
        _shooting.LoadContent(content);
        _background = content.Load<Texture2D>("gamebg");
        _bigClouds = content.Load<Texture2D>("clouds");
        _leftBorder = (int)(0.2 * Data.ScreenWidth);
        _rightBorder = (int)(0.8 * Data.ScreenWidth);
        _bottomBorder = (int)(0.6 * Data.ScreenHeight);
        _upBorder = (int)(0.2 * Data.ScreenHeight);
        _lvlTilesWide = _map.CurrentMap.Width;
        _lvlTilesHigh = _map.CurrentMap.Height;
        _maxTilesOffsetX = _lvlTilesWide - Data.ScreenWidth / _map.TileSize.X;
        _maxTilesOffsetY = _lvlTilesHigh - Data.ScreenHeight / _map.TileSize.Y;
        _maxLvlOffsetX = _maxTilesOffsetX * _map.TileSize.X;
        _maxLvlOffsetY = _maxTilesOffsetY * _map.TileSize.Y;
    }

    private void GenerateNoise()
    {
        var noise = new Texture2D[20];
        for (int i = 5; i < 25; i++)
        {
            var temp = PerlinNoiseGenerator.GeneratePerlinNoise(PerlinNoiseGenerator.GenerateWhiteNoise(i * 10, i * 10),
                5);
            noise[i - 5] = PerlinNoiseGenerator.CreateTexture(Constants.GraphicsDevice,
                PerlinNoiseGenerator.MapGradient(Color.Orange, Color.DarkRed, temp), i - 5);
        }

        Constants.NoiseFrames = noise;
    }

    internal override void Update(GameTime gameTime)
    {
        InputManager.Update(_player, _shooting);
        _player.SetMap(_map);
        _shooting.SetMap(_map);
        _shooting.SetPosition(_player.Position);
        _player.Update(gameTime);
        _map.UpdateLoading(_player);
        _shooting.Update(gameTime);
        CheckCloseToBorder();
    }

    private void CheckCloseToBorder()
    {
        var playerX = _player.Position.X;
        var playerY = _player.Position.Y;
        var diffX = playerX - _xLvlOffset;
        var diffY = playerY - _yLvlOffset;
        if (diffX > _rightBorder) _xLvlOffset += diffX - _rightBorder;
        else if (diffX < _leftBorder) _xLvlOffset += diffX - _leftBorder;
        if (diffY > _bottomBorder) _yLvlOffset += diffY - _bottomBorder;
        else if (diffY < _upBorder) _yLvlOffset += diffY - _upBorder;

        if (_xLvlOffset > _maxLvlOffsetX) _xLvlOffset = _maxLvlOffsetX;
        else if (_xLvlOffset < 0) _xLvlOffset = 0;
        if (_yLvlOffset > _maxLvlOffsetY) _yLvlOffset = _maxLvlOffsetY;
        else if (_xLvlOffset < 0) _yLvlOffset = 0;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        Constants.GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin();
        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
        DrawClouds(spriteBatch);
        _map.LvlOffset = new Vector2(_xLvlOffset, _yLvlOffset);
        _player.LvlOffset = new Vector2(_xLvlOffset, _yLvlOffset);
        _shooting.LvlOffset = new Vector2(_xLvlOffset, _yLvlOffset);
        _map.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        _shooting.Draw(spriteBatch);
        spriteBatch.End();
    }

    private void DrawClouds(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 4; i++)
            spriteBatch.Draw(_bigClouds, new Vector2(i * _bigClouds.Width - (int)(_xLvlOffset * 0.3), 380),
                Color.White);
    }
}