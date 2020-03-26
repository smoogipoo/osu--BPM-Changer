#define DEBUG
using BMAPI.v1;
using BMAPI.v1.Events;
using BMAPI.v1.HitObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace osu_trainer
{
    public partial class MainForm : Form
    {
        private Beatmap OriginalBeatmap;
        private Beatmap NewBeatmap;
        private float bpmMultiplier;
        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectMapButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Beatmap";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
#if !DEBUG
                    try
                    {
#endif
                        OriginalBeatmap = new Beatmap(ofd.FileName);
                        NewBeatmap = new Beatmap(ofd.FileName);
                        success = true;
#if !DEBUG
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("The beatmap could not be parsed. Please post the following error in the forums:\n" + ex);
                        Application.Exit();
                    }
#endif
                }
            }

            if (!success)
                return;

            CurrentSongText.Text = OriginalBeatmap.Filename;
            GenerateMapButton.Text = "PRESS SPACEBAR TO CREATE SHITMAP";
            GenerateMapButton.Enabled = true;
        }

        private async void GenerateMapButton_Click(object sender, EventArgs e)
        {
            GenerateMapButton.Enabled = false;
            GenerateMapButton.Text = "Working...";
            ModifyBeatmapSpeed(NewBeatmap, bpmMultiplier);
            await Task.Run(() => GenerateMap(NewBeatmap, bpmMultiplier));
            GenerateMapButton.Text = "PRESS SPACEBAR TO CREATE SHITMAP";
            GenerateMapButton.Enabled = true;

            // reset diff name
            NewBeatmap.Version = OriginalBeatmap.Version;
        }


        private void ModifyBeatmapSpeed(Beatmap map, float multiplier)
        {
            // OUT: map.Version (diff name)
            var bpms = map.TimingPoints.Where((tp) => !tp.InheritsBPM).Select((tp) => 60000 / tp.BpmDelay).ToList();
            var bpmsUnique = bpms.Distinct().ToList();
            if (bpmsUnique.Count >= 2)
                map.Version += $" x{multiplier}";
            else
                map.Version += $" {(bpmsUnique[0] * multiplier).ToString("0.0")}bpm";

            // Want to divide timestamps since high multiplier => shorter time
            // OUT: tp.BpmDelay          for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            foreach (TimingPoint timingPoint in map.TimingPoints)
            {
                if (timingPoint.InheritsBPM == false)
                {
                    float oldBpm = 60000 / timingPoint.BpmDelay;
                    float newBpm = oldBpm * multiplier;
                    float newDelay = 60000 / newBpm;
                    timingPoint.BpmDelay = newDelay;
                    timingPoint.Time = (int)(timingPoint.Time / multiplier);
                }
                else
                {
                    timingPoint.Time = (int)(timingPoint.Time / multiplier);
                }
            }

            // OUT: event.StartTime      for each event in beatmap
            // OUT: event.EndTime        for each break event in beatmap
            foreach (EventBase e in map.Events)
            {
                e.StartTime = (int)(e.StartTime / multiplier);
                if (e.GetType() == typeof(BreakEvent))
                    ((BreakEvent)e).EndTime = (int)(((BreakEvent)e).EndTime / multiplier);
            }

            // OUT: hitobject.StartTime         for each hit object in beatmap
            // OUT: hitobject.EndTime           for each spinner in beatmap
            foreach (CircleObject hitobject in map.HitObjects)
            {
                hitobject.StartTime = (int)(hitobject.StartTime / multiplier);
                if (hitobject.GetType() == typeof(SpinnerObject))
                    ((SpinnerObject)hitobject).EndTime = (int)(((SpinnerObject)hitobject).EndTime / multiplier);
            }
        }

        private void GenerateMap(Beatmap map, double multiplier)
        {

            // make this map searchable in the in-game menus
            map.Tags.Add("osutrainer");
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
            lame1.StartInfo.FileName = "lame.exe";
            lame1.StartInfo.Arguments = string.Format("--decode \"{0}\" \"{1}\"", temp1, temp2);
            lame1.StartInfo.UseShellExecute = false;
            lame1.StartInfo.CreateNoWindow = true;
            lame1.Start();
            lame1.WaitForExit();

            // soundstretch.exe
            Process soundstretch = new Process();
            soundstretch.StartInfo.FileName = "soundstretch.exe";
            soundstretch.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" -tempo={2}", temp2, temp3, (multiplier - 1) * 100);
            soundstretch.StartInfo.UseShellExecute = false;
            soundstretch.StartInfo.CreateNoWindow = true;
            soundstretch.Start();
            soundstretch.WaitForExit();

            // lame.exe again
            Process lame2 = new Process();
            lame2.StartInfo.FileName = "lame.exe";
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
        private static string getTempFilename(string ext)
        {
            return Path.GetTempPath() + Guid.NewGuid() + '.' + ext;
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

        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            bpmMultiplier = (float) BpmMultiplierUpDown.Value;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }
    }
}
