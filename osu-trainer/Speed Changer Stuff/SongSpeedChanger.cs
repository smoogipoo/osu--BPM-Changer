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
        // TODO: do we need all these steps? can we do something to improve performance?
        // TODO: do not save beatmap here, just generate .mp3. Assign new mp3 name outside of this function
        public static void GenerateMap(Beatmap map, double multiplier)
        {
            if (multiplier == 1)
                return;
            //temp1: Audio copy
            //temp2: Decoded wav
            //temp3: stretched file
            //temp4: Encoded mp3
            string temp1 = getTempFilename("mp3");
            string temp2 = getTempFilename("wav");
            string temp3 = getTempFilename("wav");
            string temp4 = getTempFilename("mp3");

            // TODO: try catch
            CopyFile(map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + map.AudioFilename, temp1);

            map.AudioFilename = map.AudioFilename.Substring(0, map.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + NormalizeText(map.Version) + ".mp3";

            // lame.exe
            Process lame1 = new Process();
            lame1.StartInfo.FileName = "Speed Changer Stuff\\lame.exe";
            lame1.StartInfo.Arguments = string.Format("--decode \"{0}\" \"{1}\"", temp1, temp2);
            lame1.StartInfo.UseShellExecute = false;
            lame1.StartInfo.CreateNoWindow = true;
            lame1.Start();
            lame1.WaitForExit();

            // soundstretch.exe
            Process soundstretch = new Process();
            soundstretch.StartInfo.FileName = "Speed Changer Stuff\\soundstretch.exe";
            soundstretch.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" -tempo={2}", temp2, temp3, (multiplier - 1) * 100);
            soundstretch.StartInfo.UseShellExecute = false;
            soundstretch.StartInfo.CreateNoWindow = true;
            soundstretch.Start();
            soundstretch.WaitForExit();

            // lame.exe again
            Process lame2 = new Process();
            lame2.StartInfo.FileName = "Speed Changer Stuff\\lame.exe";
            lame2.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", temp3, temp4);
            lame2.StartInfo.UseShellExecute = false;
            lame2.StartInfo.CreateNoWindow = true;
            lame2.Start();
            lame2.WaitForExit();

            CopyFile(temp4, map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture)) + "\\" + map.AudioFilename);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Saving beatmap...");
            map.Filename = map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + NormalizeText(map.Artist) + " - " + NormalizeText(map.Title) + " (" + NormalizeText(map.Creator) + ")" + " [" + NormalizeText(map.Version) + "].osu";
            map.Save(map.Filename);

            Console.WriteLine("Cleaning up...");
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
