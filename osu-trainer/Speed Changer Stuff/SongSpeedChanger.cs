using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace osu_trainer
{
    internal class SongSpeedChanger
    {
        public static void GenerateAudioFile(string inFile, string outFile, decimal multiplier, BackgroundWorker worker, bool changePitch = false)
        {
            if (multiplier == 1)
                throw new ArgumentException("Don't call this function if multiplier is 1.0x");

            string temp1 = JunUtils.GetTempFilename("mp3"); // audio copy
            string temp2 = JunUtils.GetTempFilename("wav"); // decoded wav
            string temp3 = JunUtils.GetTempFilename("wav"); // stretched file
            string temp4 = JunUtils.GetTempFilename("mp3"); // encoded mp3

            // TODO: try catch
            File.Copy(inFile, temp1);

            worker.ReportProgress(17);

            // mp3 => wav
            Process lame1 = new Process();
            lame1.StartInfo.FileName = Path.Combine("Speed Changer Stuff", "lame.exe");
            lame1.StartInfo.Arguments = $"-q 9 --priority 4 --decode \"{temp1}\" \"{temp2}\"";
            lame1.StartInfo.UseShellExecute = false;
            lame1.StartInfo.CreateNoWindow = true;
            lame1.Start();
            lame1.WaitForExit();

            worker.ReportProgress(33);

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

            worker.ReportProgress(50);

            // wav => mp3
            Process lame2 = new Process();
            lame2.StartInfo.FileName = Path.Combine("Speed Changer Stuff", "lame.exe");
            lame2.StartInfo.Arguments = $"-q 9 --priority 4 \"{temp3}\" \"{temp4}\"";
            lame2.StartInfo.UseShellExecute = false;
            lame2.StartInfo.CreateNoWindow = true;
            lame2.Start();
            lame2.WaitForExit();

            worker.ReportProgress(67);

            if (File.Exists(outFile))
                File.Delete(outFile);
            File.Copy(temp4, outFile);
            worker.ReportProgress(83);

            // Clean up
            File.Delete(temp1);
            File.Delete(temp2);
            File.Delete(temp3);
            File.Delete(temp4);
            worker.ReportProgress(100);
        }
    }
}