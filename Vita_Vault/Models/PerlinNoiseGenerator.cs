using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vita_Vault.Models;

internal static class PerlinNoiseGenerator
{
    private const int AmountOfFrames = 20;

    internal static float[,] GenerateWhiteNoise(int width, int height)
    {
        var random = new Random();
        var noise = new float[width, height];

        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
            noise[i, j] = (float)random.NextDouble() % 1;

        return noise;
    }

    internal static float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
    {
        var width = baseNoise.GetLength(0);
        var height = baseNoise.GetLength(1);

        var smoothNoise = new float[octaveCount][,];

        const float persistence = 0.5f;

        for (int i = 0; i < octaveCount; i++)
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);


        var perlinNoise = new float[width, height];
        var amplitude = 1.0f;
        var totalAmplitude = 0.0f;

        //смешиваем два шума 
        for (var octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistence;
            totalAmplitude += amplitude;

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
        }

        //нормализация
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
            perlinNoise[i, j] /= totalAmplitude;

        return perlinNoise;
    }


    internal static Color[,] MapGradient(Color gradientStart, Color gradientEnd, float[,] perlinNoise)
    {
        var width = perlinNoise.GetLength(0);
        var height = perlinNoise.GetLength(1);
        var image = new Color[width, height];

        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
            image[i, j] = GetColor(gradientStart, gradientEnd, perlinNoise[i, j]);

        return image;
    }

    internal static Texture2D CreateTexture(GraphicsDevice device, Color[,] pixels, int frame)
    {
        var width = pixels.GetLength(0);
        var height = pixels.GetLength(1);
        var offsetX = width / 2;
        var offsetY = height / 2;
        var texture = new Texture2D(device, width, height);

        var data = new Color[width * height];

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var tempX = offsetX - i;
                var tempY = offsetY - j;
                var tempColor = pixels[i, j] * ((AmountOfFrames - frame) / (float)AmountOfFrames);
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

    private static float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1;
    }

    private static float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
    {
        var width = baseNoise.GetLength(0);
        var height = baseNoise.GetLength(1);

        var smoothNoise = new float[width, height];

        var samplePeriod = (int)Math.Pow(2, octave);
        var sampleFrequency = 1.0f / samplePeriod;

        for (var i = 0; i < width; i++)
        {
            //рассчитваем индексы горизонтальной выборки
            var sampleI0 = (i / samplePeriod) * samplePeriod;
            var sampleI1 = (sampleI0 + samplePeriod) % width;
            var horizontalBlend = (i - sampleI0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                //рассчитваем индексы вертикальной выборки
                var sampleJ0 = (j / samplePeriod) * samplePeriod;
                var sampleJ1 = (sampleJ0 + samplePeriod) % height;
                var verticalBlend = (j - sampleJ0) * sampleFrequency;

                //смешиваем два угла
                var top = Interpolate(baseNoise[sampleI0, sampleJ0],
                    baseNoise[sampleI1, sampleJ0], horizontalBlend);

                //смешиваем два угла
                var bottom = Interpolate(baseNoise[sampleI0, sampleJ1],
                    baseNoise[sampleI1, sampleJ1], horizontalBlend);

                //финальное смешивание
                smoothNoise[i, j] = Interpolate(top, bottom, verticalBlend);
            }
        }

        return smoothNoise;
    }

    private static Color GetColor(Color gradientStart, Color gradientEnd, float t)
    {
        var u = 1 - t;

        var color = new Color(
            (int)(gradientStart.R * u + gradientEnd.R * t),
            (int)(gradientStart.G * u + gradientEnd.G * t),
            (int)(gradientStart.B * u + gradientEnd.B * t), 255);

        return color;
    }
}