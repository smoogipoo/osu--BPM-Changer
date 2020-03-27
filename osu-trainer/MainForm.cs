using BMAPI.v1;
using BMAPI.v1.Events;
using BMAPI.v1.HitObjects;
using OsuMemoryDataProvider;
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
        string UserOsuInstallPath = "C:\\Program Files\\osu!";
        Beatmap OriginalBeatmap;
        Beatmap NewBeatmap;
        float bpmMultiplier = 1.0f;
        private IOsuMemoryReader osu;

        public MainForm()
        {
            InitializeComponent();
            OsuFolderTextBox.Text = UserOsuInstallPath;
            BeatmapUpdateTimer.Start();

            osu = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");
        }

        private void SelectMapButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Beatmap";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                LoadBeatmap(ofd.FileName);
            }
        }

        private async void GenerateMapButton_Click(object sender, EventArgs e)
        {
            // pre
            GenerateMapButton.Enabled = false;
            GenerateMapButton.Text = "Working...";
            var timerWasEnabled = BeatmapUpdateTimer.Enabled;
            BeatmapUpdateTimer.Stop();

            ModifyBeatmapSpeed(NewBeatmap, bpmMultiplier);
            await Task.Run(() => GenerateMap(NewBeatmap, bpmMultiplier));

            // post
            if (timerWasEnabled)
                BeatmapUpdateTimer.Start();
            GenerateMapButton.Text = "PRESS SPACEBAR TO CREATE SHITMAP";
            GenerateMapButton.Enabled = true;

            // reset diff name
            NewBeatmap.Version = OriginalBeatmap.Version;
        }


        private void ModifyBeatmapSpeed(Beatmap map, float multiplier)
        {
            if (multiplier == 1)
                return;

            // OUT: map.Version (diff name)
            var bpmsUnique = GetBpmList(map).Distinct().ToList();
            if (bpmsUnique.Count >= 2)
                map.Version += $" x{multiplier}";
            else
                map.Version += $" {(bpmsUnique[0] * multiplier).ToString("0")}bpm";

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
            if (multiplier == 1)
            {

                string ARODCS = "";
                if (NewBeatmap.ApproachRate != OriginalBeatmap.ApproachRate)
                    ARODCS += $" AR{NewBeatmap.ApproachRate}";
                if (NewBeatmap.OverallDifficulty != OriginalBeatmap.OverallDifficulty)
                    ARODCS += $" OD{NewBeatmap.OverallDifficulty}";
                if (NewBeatmap.CircleSize != OriginalBeatmap.CircleSize)
                    ARODCS += $" CS{NewBeatmap.CircleSize}";
                map.Version += ARODCS;
                map.Filename = map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + NormalizeText(map.Artist) + " - " + NormalizeText(map.Title) + " (" + NormalizeText(map.Creator) + ")" + " [" + NormalizeText(map.Version) + "].osu";
                map.Save(map.Filename);
                return;
            }

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
            setMultiplier((float)BpmMultiplierUpDown.Value);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }

        private void OsuFolderTextBox_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new OpenFileDialog())
            {
                folderDialog.Title = "Select your osu! installation folder";
                folderDialog.ValidateNames = false;
                folderDialog.CheckFileExists = false;
                folderDialog.CheckPathExists = true;
                folderDialog.FileName = "Select Folder";
                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                UserOsuInstallPath = Path.GetDirectoryName(folderDialog.FileName);
                OsuFolderTextBox.BackColor = SystemColors.Control;
                OsuFolderTextBox.Text = UserOsuInstallPath;
            }
        }

        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            string beatmapFolder = osu.GetMapFolderName();
            string beatmapFilename = osu.GetOsuFileName();
            string absoluteFilename = UserOsuInstallPath + "\\Songs\\" + beatmapFolder + "\\" + beatmapFilename;

            if (OriginalBeatmap == null)
            {
                LoadBeatmap(absoluteFilename);
                return;
            }

            // Beatmap Changed
            if (beatmapFilename != Path.GetFileName(OriginalBeatmap.Filename))
                LoadBeatmap(absoluteFilename);
        }

        private void LoadBeatmap(string beatmapPath)
        {
            try
            {
                OriginalBeatmap = new Beatmap(beatmapPath);
                NewBeatmap = new Beatmap(beatmapPath);
            }
            catch (FormatException e)
            {
                CurrentSongText.Text = Path.GetFileName(beatmapPath) + " (error)";
                CurrentSongText.BackColor = Color.Red;
                return;
            }
            // Song Display
            CurrentSongText.Text = Path.GetFileName(OriginalBeatmap.Filename);
            CurrentSongText.BackColor = Color.WhiteSmoke;

            // Update BPM Display
            var originalBpms = GetBpmList(OriginalBeatmap).Select((bpm) => (int)bpm).ToList();
            OriginalBpmTextBox.Text = string.Join(" → ", originalBpms);
            var newBpms = GetBpmList(NewBeatmap).Select((bpm) => (int)(bpm * bpmMultiplier)).ToList();
            NewBpmTextBox.Text = string.Join(" → ", newBpms);

            // Scale AR and Update AR Display
            NewBeatmap.ApproachRate = DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier);
            ARUpDown.Value = (decimal)NewBeatmap.ApproachRate;
            ARUpDown.BackColor = SystemColors.Window;


            // Generate Button Ready
            GenerateMapButton.Text = "PRESS SPACEBAR TO CREATE SHITMAP";
            GenerateMapButton.Enabled = true;
        }

        private void AutoDetectMapCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoDetectMapCheckbox.Checked)
                BeatmapUpdateTimer.Start();
            else
                BeatmapUpdateTimer.Stop();
        }

        private void setMultiplier(float mult)
        {
            bpmMultiplier = mult;
            BpmMultiplierUpDown.Value = (decimal)mult;

            // Update BPM Display
            var newBpms = GetBpmList(NewBeatmap).Select((bpm) => (int)(bpm * bpmMultiplier)).ToList();
            NewBpmTextBox.Text = string.Join(" → ", newBpms);

            // Scale AR and Update AR Display
            NewBeatmap.ApproachRate = DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier);
            ARUpDown.Value = (decimal)NewBeatmap.ApproachRate;
            ARUpDown.BackColor = SystemColors.Window;
        }

        private List<float> GetBpmList(Beatmap map)
        {
            var bpms = map.TimingPoints.Where((tp) => !tp.InheritsBPM).Select((tp) => 60000 / tp.BpmDelay).ToList();
            var bpmsUnique = bpms.Distinct().ToList();
            if (bpmsUnique.Count == 1)
                return bpmsUnique;
            return bpms;
        }

        private void ARUpDown_ValueChanged(object sender, EventArgs e)
        {
            NewBeatmap.ApproachRate = (float)ARUpDown.Value;
            if (NewBeatmap.ApproachRate > DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier))
                ARUpDown.BackColor = Color.FromArgb(255, 220, 220);
            else if (NewBeatmap.ApproachRate < DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier))
                ARUpDown.BackColor = Color.LightCyan;
            else
                ARUpDown.BackColor = SystemColors.Window;
        }
    }
}
