using FsBeatmapProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    internal class JunUtils
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable
        {
            return val.CompareTo(max) > 0 ? max : val.CompareTo(min) < 0 ? min : val;
        }

        public static decimal Quantize(decimal value, decimal step)
        {
            value += step / 2; // make function symmetrical
            int steps = (int)(value / step);
            return steps * step;
        }

        public static string FullPathFromSongsFolder(string path) => Path.Combine(Properties.Settings.Default.SongsFolder, path);

        public static string NormalizeText(string str)
        {
            return str.Replace("\"", "").Replace("*", "").Replace("\\", "").Replace("/", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }

        public static string GetTempFilename(string ext)
        {
            return Path.GetTempPath() + Guid.NewGuid() + '.' + ext;
        }

        public static string GetBeatmapDirectoryName(Beatmap map)
        {
            return Path.GetDirectoryName(map.Filename);
        }

        public static GraphicsPath RoundedRect(RectangleF bounds, int radius)
        {
            var diameter = radius * 2;
            var size = new Size(diameter, diameter);
            var arc = new RectangleF(bounds.Location, size);
            var path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}