using BMAPI.v1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    class Helper
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable {
            return val.CompareTo(max) > 0 ? max : val.CompareTo(min) < 0 ? min : val;
        }
        public static decimal Quantize(decimal value, decimal step) {
            value += step / 2; // make function symmetrical
            int steps = (int)(value / step);
            return steps * step;
        }
        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

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
