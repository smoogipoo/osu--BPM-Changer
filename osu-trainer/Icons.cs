using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FsBeatmapProcessor;

namespace osu_trainer
{
    public static class Icons
    {
        public static Image GenerateIcon(GameMode mode, Color color)
        {
            var r = color.R / (float)byte.MaxValue;
            var g = color.G / (float)byte.MaxValue;
            var b = color.B / (float)byte.MaxValue;

            float[][] colorMatrixElements = {
                new float[] {r,  0,  0,  0, 0},        // red scaling factor of 2
                new float[] {0,  g,  0,  0, 0},        // green scaling factor of 1
                new float[] {0,  0,  b,  0, 0},        // blue scaling factor of 1
                new float[] {0,  0,  0,  1, 0},        // alpha scaling factor of 1
                new float[] {0,  0,  0,  0, 1}       // three translations of 0.2
            };

            Bitmap baseIcon = null;
            switch (mode)
            {
                case GameMode.osu:          baseIcon = Properties.Resources.standard; break;
                case GameMode.Taiko:        baseIcon = Properties.Resources.taiko;    break;
                case GameMode.CatchtheBeat: baseIcon = Properties.Resources._catch;   break;
                case GameMode.Mania:        baseIcon = Properties.Resources.mania;    break;
            }

            var width = baseIcon.Width + 2;
            var height = baseIcon.Height + 2;
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix(colorMatrixElements);
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                graphics.DrawImage(
                    baseIcon,
                    new Rectangle(0, 0, width, height),
                    0, 0,
                    width,
                    height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return bitmap;
        }
    }
}