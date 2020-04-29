using FsBeatmapProcessor;
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
        public static void GenerateAudioFile(string inFile, string outFile, decimal multiplier, bool changePitch=false)
        {
            if (multiplier == 1)
                throw new ArgumentException("Don't call this function if multiplier is 1.0x");

            string temp1 = JunUtils.GetTempFilename("mp3"); // audio copy
            string temp2 = JunUtils.GetTempFilename("wav"); // decoded wav
            string temp3 = JunUtils.GetTempFilename("wav"); // stretched file
            string temp4 = JunUtils.GetTempFilename("mp3"); // encoded mp3

            // TODO: try catch
            CopyFile(inFile, temp1);

            // mp3 => wav
            Process lame1 = new Process();
            lame1.StartInfo.FileName = Path.Combine("Speed Changer Stuff", "lame.exe");
            lame1.StartInfo.Arguments = $"-q 9 --priority 4 --decode \"{temp1}\" \"{temp2}\"";
            lame1.StartInfo.UseShellExecute = false;
            lame1.StartInfo.CreateNoWindow = true;
            lame1.Start();
            lame1.WaitForExit();

            // stretch (or speed up) wav
            decimal cents = (decimal)(1200.0 * Math.Log((double)multiplier) / Math.Log(2));
            decimal semitones = cents / 100.0M;
            Process soundstretch = new Process();
            soundstretch.StartInfo.FileName = Path.Combine("Speed Changer Stuff", "soundstretch.exe");
            if (changePitch)
                soundstretch.StartInfo.Arguments = $"\"{temp2}\" \"{temp3}\" -quick -naa -tempo={(multiplier - 1) * 100} -pitch={semitones}";
            else
                soundstretch.StartInfo.Arguments = $"\"{temp2}\" \"{temp3}\" -quick -naa -tempo={(multiplier - 1) * 100}";
            soundstretch.StartInfo.UseShellExecute = false;
            soundstretch.StartInfo.CreateNoWindow = true;
            soundstretch.Start();
            soundstretch.WaitForExit();

            // wav => mp3
            Process lame2 = new Process();
            lame2.StartInfo.FileName = Path.Combine("Speed Changer Stuff", "lame.exe");
            lame2.StartInfo.Arguments = $"-q 9 --priority 4 \"{temp3}\" \"{temp4}\"";
            lame2.StartInfo.UseShellExecute = false;
            lame2.StartInfo.CreateNoWindow = true;
            lame2.Start();
            lame2.WaitForExit();

            CopyFile(temp4, outFile);

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
    }
}
