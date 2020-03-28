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
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace osu_trainer
{
    public partial class MainForm : Form
    {
        string UserOsuInstallPath = null;
        Beatmap OriginalBeatmap;
        Beatmap NewBeatmap;
        float bpmMultiplier = 1.0f;
        IOsuMemoryReader osu;

        // Color Theme
        Color formBg = Color.FromArgb(38, 35, 53);
        Color textBoxBg = Color.FromArgb(23, 16, 25);
        Color textBoxFg = Color.FromArgb(224, 224, 224);
        Color label1Color = Color.FromArgb(249, 126, 114);
        Color labelDisabledColor = Color.FromArgb(136, 134, 144);
        Color accentPink = Color.FromArgb(255, 126, 219);
        Color accentBlue = Color.FromArgb(46, 226, 250);
        Color accentOrange = Color.FromArgb(246, 122, 44);

        // Common Control Lists
        List<Label> labels;
        List<NumericUpDown> updowns;
        List<TextBox> textboxes;

        public MainForm()
        {
            InitializeComponent();
            InitializeControlLists();

            // Init osu memory reader
            osu = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");

            // Controls will be enabled when a beatmap is loaded in
            DisableFormControls();
            BeatmapUpdateTimer.Start();

        }
        private void InitializeControlLists()
        {
            labels = new List<Label>
            {
                label2,
                label4,
                label5,
                label6
            };
            updowns = new List<NumericUpDown>
            {
                ARUpDown,
                BpmMultiplierUpDown
            };
            textboxes = new List<TextBox>
            {
                OriginalBpmTextBox,
                NewBpmTextBox
            };
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
            DisableGenerateMapButton();
            var oldButtonText = GenerateMapButton.Text;
            GenerateMapButton.Text = "Working...";
            BeatmapUpdateTimer.Stop();

            // main phase
            ModifyBeatmapTiming(NewBeatmap, bpmMultiplier);
            ModifyBeatmapMetadata(NewBeatmap, bpmMultiplier);
            await Task.Run(() => SongSpeedChanger.GenerateMap(NewBeatmap, bpmMultiplier));

            // post
            if (AutoDetectMapCheckbox.Checked)
                BeatmapUpdateTimer.Start();

            EnableGenerateMapButton();
            GenerateMapButton.Text = oldButtonText;

            // reset diff name
            NewBeatmap.Version = OriginalBeatmap.Version;
        }

        private void ModifyBeatmapTiming(Beatmap map, float multiplier)
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
        private void ModifyBeatmapMetadata(Beatmap map, float multiplier)
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
        }

        private static string getTempFilename(string ext)
        {
            return Path.GetTempPath() + Guid.NewGuid() + '.' + ext;
        }
        public static string NormalizeText(string str)
        {
            return str.Replace("\"", "").Replace("*", "").Replace("\\", "").Replace("/", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }

        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Value changed");
            setMultiplier((float)BpmMultiplierUpDown.Value);
            FormatAR();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }

        private bool LoadBeatmap(string beatmapPath)
        {
            try
            {
                OriginalBeatmap = new Beatmap(beatmapPath);
                NewBeatmap = new Beatmap(beatmapPath);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Bad .osu file format");
                return false;
            }
            // Check if beatmap was loaded successfully
            if (NewBeatmap.Filename == null && NewBeatmap.Title == null)
            {
                return false;
            }
            // Song Display
            SongLabel.Text = $"{NewBeatmap.Artist} - {NewBeatmap.Title}";
            DiffLabel.Text = NewBeatmap.Version;
            UpdateSongBg(NewBeatmap);

            // Update BPM Display
            UpdateBpmDisplay();

            // Scale Approach Rate
            NewBeatmap.ApproachRate = DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier);

            EnableFormControls();

            // Update Approach Rate Display
            ARUpDown.Value = (decimal)NewBeatmap.ApproachRate;
            FormatAR();

            return true;
        }

        private void UpdateSongBg(Beatmap map)
        {
            var imageEvent = map.Events.OfType<ContentEvent>().FirstOrDefault(e => e.Type == ContentType.Image);
            string imageAbsolutePath = Path.GetDirectoryName(map.Filename) + "\\" + imageEvent.Filename;
            if (File.Exists(imageAbsolutePath))
            {
                // crop height to aspect ratio
                Image bg = Image.FromFile(imageAbsolutePath);
                Bitmap bmpImage = new Bitmap(bg);
                float aspectRatio = BgPanel.Size.Width / BgPanel.Size.Height;
                int cropHeight = (int)(bmpImage.Width / aspectRatio);

                // pan down to center image
                int panDown = (bmpImage.Height - cropHeight) / 2;

                BgPanel.BackgroundImage = bmpImage.Clone(new Rectangle(0, panDown, bmpImage.Width, cropHeight), bmpImage.PixelFormat);
            }
            else
            {
                // no background for this map
                Console.WriteLine($"No image");
            }
        }

        private void AutoDetectMapCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoDetectMapCheckbox.Checked)
            {
                DisableSelectMapButton();
                BeatmapUpdateTimer.Start();
            }
            else
            {
                EnableSelectMapButton();
                BeatmapUpdateTimer.Stop();
            }
        }

        private void setMultiplier(float mult)
        {
            bpmMultiplier = mult;
            BpmMultiplierUpDown.Value = (decimal)mult;

            // Update BPM Display
            UpdateBpmDisplay();

            // Scale AR and Update AR Display
            NewBeatmap.ApproachRate = DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier);
            ARUpDown.Value = (decimal)NewBeatmap.ApproachRate;
            ARUpDown.BackColor = textBoxBg;
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
            FormatAR();
        }

        #region Control State and Appearance
        private void DisableFormControls()
        {
            SelectMapButton.Enabled = false;
            DisableARUpDown();
            BpmMultiplierUpDown.Enabled = false;
            GenerateMapButton.Enabled = false;
            foreach (var label in labels)
                label.ForeColor = labelDisabledColor;
            DisableARUpDown();
            DisableBpmUpDown();

            DisableSelectMapButton();
        }
        private void EnableFormControls()
        {
            EnableARUpDown();
            BpmMultiplierUpDown.Enabled = true;
            EnableGenerateMapButton();
            foreach (var label in labels)
                label.ForeColor = label1Color;
            EnableARUpDown();
            EnableBpmUpDown();

            if (AutoDetectMapCheckbox.Checked == false)
                EnableSelectMapButton();
        }
        private void EnableARUpDown()
        {
            ARUpDown.Enabled = true;
            ARUpDown.BackColor = textBoxBg;
            FormatAR();
        }
        private void DisableARUpDown()
        {
            ARUpDown.Enabled = false;
            ARUpDown.Font = new Font(ARUpDown.Font, FontStyle.Regular);
            ARUpDown.ForeColor = Color.FromArgb(224, 224, 224);
            ARUpDown.BackColor = SystemColors.ControlDark;
        }
        private void EnableBpmUpDown()
        {
            BpmMultiplierUpDown.Enabled = true;
            BpmMultiplierUpDown.BackColor = textBoxBg;
        }
        private void DisableBpmUpDown()
        {
            BpmMultiplierUpDown.Enabled = false;
            BpmMultiplierUpDown.Font = new Font(BpmMultiplierUpDown.Font, FontStyle.Regular);
            BpmMultiplierUpDown.ForeColor = Color.FromArgb(224, 224, 224);
            BpmMultiplierUpDown.BackColor = SystemColors.ControlDark;
        }
        private void EnableGenerateMapButton()
        {
            GenerateMapButton.Enabled = true;
            GenerateMapButton.ForeColor = Color.White;
            GenerateMapButton.BackColor = accentPink;
            GenerateMapButton.Font = new Font(GenerateMapButton.Font, FontStyle.Bold);
        }
        private void DisableGenerateMapButton()
        {
            GenerateMapButton.ForeColor = Color.DimGray;
            GenerateMapButton.BackColor = SystemColors.ControlLight;
            GenerateMapButton.Font = new Font(GenerateMapButton.Font, FontStyle.Regular);
            GenerateMapButton.Enabled = false;
        }
        private void FormatAR()
        {
            if (NewBeatmap.ApproachRate > DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier))
            {
                ARUpDown.ForeColor = Color.FromArgb(254, 68, 80);
                ARUpDown.Font = new Font(ARUpDown.Font, FontStyle.Bold);
            }
            else if (NewBeatmap.ApproachRate < DifficultyCalculator.CalculateNewAR(OriginalBeatmap, bpmMultiplier))
            {
                ARUpDown.ForeColor = Color.FromArgb(114, 241, 184);
                ARUpDown.Font = new Font(ARUpDown.Font, FontStyle.Bold);
            }
            else
            {
                ARUpDown.ForeColor = Color.FromArgb(224, 224, 224);
                ARUpDown.Font = new Font(ARUpDown.Font, FontStyle.Regular);
            }
        }
        private void EnableSelectMapButton()
        {
            SelectMapButton.BackColor = accentOrange;
            SelectMapButton.ForeColor = Color.White;
            SelectMapButton.Font = new Font(SelectMapButton.Font, FontStyle.Bold);
            SelectMapButton.Enabled = true;
        }
        private void DisableSelectMapButton()
        {
            SelectMapButton.BackColor = SystemColors.ControlLight;
            SelectMapButton.ForeColor = Color.DimGray;
            SelectMapButton.Font = new Font(SelectMapButton.Font, FontStyle.Regular);
            SelectMapButton.Enabled = false;
        }
        private void UpdateBpmDisplay()
        {
            var originalBpms = GetBpmList(OriginalBeatmap).Select((bpm) => (int)bpm).ToList();
            var newBpms = GetBpmList(NewBeatmap).Select((bpm) => (int)(bpm * bpmMultiplier)).ToList();

            if (new HashSet<int>(originalBpms).Count == 1)
            {
                OriginalBpmTextBox.Text = originalBpms[0].ToString();
                NewBpmTextBox.Text = newBpms[0].ToString();
                return;
            }

            OriginalBpmTextBox.Text = string.Join(" ... ", originalBpms);
            NewBpmTextBox.Text = string.Join(" ... ", newBpms);
        }
        #endregion
        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Try to get osu install folder
            if (UserOsuInstallPath == null)
            {
                // check if osu process is running
                var processes = Process.GetProcessesByName("osu!");
                if (processes.Length == 0)
                    return;

                // check if osu exe exists
                var osuExePath = processes[0].MainModule.FileName;
                if (!File.Exists(osuExePath))
                    return;

                // set path
                UserOsuInstallPath = Path.GetDirectoryName(osuExePath);
            }


            string beatmapFolder = osu.GetMapFolderName();
            string beatmapFilename = osu.GetOsuFileName();
            string absoluteFilename = UserOsuInstallPath + "\\Songs\\" + beatmapFolder + "\\" + beatmapFilename;
            if (beatmapFilename == "")
                return;

            if (OriginalBeatmap == null)
            {
                LoadBeatmap(absoluteFilename);
                return;
            }

            // Beatmap Changed
            if (beatmapFilename != Path.GetFileName(OriginalBeatmap.Filename))
                LoadBeatmap(absoluteFilename);
        }
    }
}
