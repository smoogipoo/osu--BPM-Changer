using FsBeatmapProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    class JunUtils
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable {
            return val.CompareTo(max) > 0 ? max : val.CompareTo(min) < 0 ? min : val;
        }
        public static decimal Quantize(decimal value, decimal step) {
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
    }
}
