using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class PerlinNoise : Component
{
    public Texture2D[] noise;
    public GraphicsDevice _graphicsDevice;
    private double currentFrame;
    private Texture2D currentTexture;
    private Vector2 _position;
    public Vector2 LvlOffset;
    public bool IsDestroyed { get; private set; }

    public PerlinNoise(Vector2 position, Vector2 lvlOffset)
    {
        _position = position;
        LvlOffset = lvlOffset;
    }

    internal override void LoadContent(ContentManager Content)
    {
        noise = new Texture2D[20];
        for (int i = 5; i < 25; i++)
        {
            var temp = GeneratePerlinNoise(GenerateWhiteNoise(i * 10, i * 10), 5);
            noise[i - 5] = CreateTexture(_graphicsDevice, MapGradient(Color.Yellow, Color.DarkRed, temp), i - 5);
        }

        currentTexture = noise[0];
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        currentFrame += 7 * time;
        if (currentFrame > noise.Length)
        {
            IsDestroyed = true;
            return;
        }

        currentTexture = noise[(int)currentFrame];
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(currentTexture,
            _position - new Vector2(currentTexture.Width / 2, currentTexture.Height / 2) - LvlOffset, Color.White);
    }

    private float[,] GenerateWhiteNoise(int width, int height)
    {
        Random random = new Random();
        var noise = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noise[i, j] = (float)random.NextDouble() % 1;
            }
        }

        return noise;
    }

    private float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1;
    }

    private float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
    {
        var width = baseNoise.GetLength(0);
        var height = baseNoise.GetLength(1);

        var smoothNoise = new float[width, height];

        var samplePeriod = (int)Math.Pow(2, octave);
        float sampleFrequency = 1.0f / samplePeriod;

        for (int i = 0; i < width; i++)
        {
            //рассчитваем индексы горизонтальной выборки
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width;
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                //рассчитваем индексы вертикальной выборки
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height;
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                //смешиваем два угла
                float top = Interpolate(baseNoise[sample_i0, sample_j0],
                    baseNoise[sample_i1, sample_j0], horizontal_blend);

                //смешиваем два угла
                float bottom = Interpolate(baseNoise[sample_i0, sample_j1],
                    baseNoise[sample_i1, sample_j1], horizontal_blend);

                //финальное смешивание
                smoothNoise[i, j] = Interpolate(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }

    private float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
    {
        var width = baseNoise.GetLength(0);
        var height = baseNoise.GetLength(1);

        var smoothNoise = new float[octaveCount][,];

        float persistance = 0.5f;

        for (int i = 0; i < octaveCount; i++)
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);


        var perlinNoise = new float[width, height];
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        //смешиваем два шума 
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                }
            }
        }

        //нормализация
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                perlinNoise[i, j] /= totalAmplitude;
        }

        return perlinNoise;
    }

    private Color GetColor(Color gradientStart, Color gradientEnd, float t)
    {
        float u = 1 - t;

        Color color = new Color(
            (int)(gradientStart.R * u + gradientEnd.R * t),
            (int)(gradientStart.G * u + gradientEnd.G * t),
            (int)(gradientStart.B * u + gradientEnd.B * t), 255);

        return color;
    }

    private Color[,] MapGradient(Color gradientStart, Color gradientEnd, float[,] perlinNoise)
    {
        int width = perlinNoise.GetLength(0);
        int height = perlinNoise.GetLength(1);

        var image = new Color[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                image[i, j] = GetColor(gradientStart, gradientEnd, perlinNoise[i, j]);
            }
        }

        return image;
    }

    public Texture2D CreateTexture(GraphicsDevice device, Color[,] pixels, int frame)
    {
        var width = pixels.GetLength(0);
        var height = pixels.GetLength(1);
        var offsetX = width / 2;
        var offsetY = height / 2;
        Texture2D texture = new Texture2D(device, width, height);

        Color[] data = new Color[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tempX = offsetX - i;
                var tempY = offsetY - j;
                var tempColor = pixels[i, j] * ((noise.Length - frame) / (float)noise.Length);
                var tempMult = (tempX * tempX) + (tempY * tempY);
                if (tempMult >= offsetX * offsetY) tempColor *= 0;
                else if (tempMult >= (offsetX - 5) * (offsetY - 5)) tempColor *= 0.1f;
                else if (tempMult >= (offsetX - 10) * (offsetY - 10)) tempColor *= 0.4f;
                else if (tempMult >= (offsetX - 15) * (offsetY - 15)) tempColor *= 0.8f;
                data[width * i + j] = tempColor;
            }
        }

        texture.SetData(data);

        return texture;
    }
}