using BMAPI.v1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    class SongSpeedChanger
    {
        // (in: originalMap.AudioFilename) => (out: newMap.AudioFilename)
        public static void GenerateAudioFile(Beatmap originalMap, Beatmap newMap, double multiplier)
        {
            if (multiplier == 1)
                throw new ArgumentException("Don't call this function if multiplier is 1.0x");


            var watch = new Stopwatch();
            watch.Start();

            //temp1: Audio copy
            //temp2: Decoded wav
            //temp3: stretched file
            //temp4: Encoded mp3
            string temp1 = getTempFilename("mp3");
            string temp2 = getTempFilename("wav");
            string temp3 = getTempFilename("wav");
            string temp4 = getTempFilename("mp3");

            // TODO: try catch
            CopyFile(originalMap.Filename.Substring(0, originalMap.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + originalMap.AudioFilename, temp1);

            // lame.exe
            Process lame1 = new Process();
            lame1.StartInfo.FileName = "Speed Changer Stuff\\lame.exe";
            lame1.StartInfo.Arguments = string.Format("-q 9 --priority 4 --decode \"{0}\" \"{1}\"", temp1, temp2);
            lame1.StartInfo.UseShellExecute = false;
            lame1.StartInfo.CreateNoWindow = true;
            lame1.Start();
            lame1.WaitForExit();

            // soundstretch.exe
            Process soundstretch = new Process();
            soundstretch.StartInfo.FileName = "Speed Changer Stuff\\soundstretch.exe";
            soundstretch.StartInfo.Arguments = string.Format($"\"{temp2}\" \"{temp3}\" -quick -naa -tempo={(multiplier - 1) * 100}");
            soundstretch.StartInfo.UseShellExecute = false;
            soundstretch.StartInfo.CreateNoWindow = true;
            soundstretch.Start();
            soundstretch.WaitForExit();

            // lame.exe again
            Process lame2 = new Process();
            lame2.StartInfo.FileName = "Speed Changer Stuff\\lame.exe";
            lame2.StartInfo.Arguments = string.Format("-q 9 --priority 4 \"{0}\" \"{1}\"", temp3, temp4);
            lame2.StartInfo.UseShellExecute = false;
            lame2.StartInfo.CreateNoWindow = true;
            lame2.Start();
            lame2.WaitForExit();

            CopyFile(temp4, Path.GetDirectoryName(newMap.Filename) + "\\" + newMap.AudioFilename);

            watch.Stop();
            Console.WriteLine($"Time elapsed: {watch.ElapsedMilliseconds}");

            // Clean up
            File.Delete(temp1);
            File.Delete(temp2);
            File.Delete(temp3);
            File.Delete(temp4);
        }

        public static void CopyFile(string src, string dst)
        {
            using (FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream dstStream = new FileStream(dst, FileMode.Create))
            {
                srcStream.CopyTo(dstStream);
            }
        }
        public static string NormalizeText(string str)
        {
            return str.Replace("\"", "").Replace("*", "").Replace("\\", "").Replace("/", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }
        private static string getTempFilename(string ext)
        {
            return Path.GetTempPath() + Guid.NewGuid() + '.' + ext;
        }
    }
}
