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
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace osu_trainer
{
    public partial class MainForm : Form
    {
        // File Resources
        string matchConfirmWav = "resources\\match-confirm.wav";

        // Beatmap
        string userOsuInstallPath = null;
        Beatmap pureBeatmap;
        Beatmap originalBeatmap;
        Beatmap newBeatmap;
        float bpmMultiplier = 1.0f;
        float lockedHP = 0f;
        float lockedCS = 0f;
        float lockedAR = 0f;
        float lockedOD = 0f;
        IOsuMemoryReader osu;

        // Color Theme
        Color formBg = Color.FromArgb(38, 35, 53);
        Color textBoxBg = Color.FromArgb(23, 16, 25);
        Color textBoxFg = Color.FromArgb(224, 224, 224);
        Color labelColor1 = Color.FromArgb(249, 126, 114);
        Color labelColor2 = Color.FromArgb(254, 222, 93);
        Color labelDisabledColor = Color.FromArgb(136, 134, 144);
        Color accentPink = Color.FromArgb(255, 126, 219);
        Color accentBlue = Color.FromArgb(46, 226, 250);
        Color accentOrange = Color.FromArgb(246, 122, 44);
        Color accentRed = Color.FromArgb(254, 68, 80);
        Color accentCyan = Color.FromArgb(46, 226, 250);
        Color easierColor = Color.FromArgb(114, 241, 184);

        // Common Control Lists
        List<Label> labels;
        List<TextBox> diffDisplays;
        List<OptionSlider> diffSliders;

        // Single Object Instances
        SoundPlayer sound = new SoundPlayer();

        // other
        private bool scaleARPreviousState;
        private string previousBeatmapRead;

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
                hplabel,
                cslabel,
                arlabel,
                odlabel,
                label2,
                label4,
                label5
            };
            diffDisplays = new List<TextBox>
            {
                HPDisplay,
                CSDisplay,
                ARDisplay,
                ODDisplay,
            };
            diffSliders = new List<OptionSlider>
            {
                HPSlider,
                CSSlider,
                ARSlider,
                ODSlider,
            };
        }

        private async void GenerateMapButton_Click(object sender, EventArgs e)
        {
            // pre
            DisableGenerateMapButton();
            var oldButtonText = GenerateMapButton.Text;
            GenerateMapButton.Text = "Working...";
            BeatmapUpdateTimer.Stop();

            // main phase
            ModifyBeatmapTiming(newBeatmap, bpmMultiplier);
            ModifyBeatmapMetadata(newBeatmap, bpmMultiplier);
            if (!File.Exists(GetBeatmapDirectoryName() + "\\" + newBeatmap.AudioFilename))
                await Task.Run(() => SongSpeedChanger.GenerateAudioFile(originalBeatmap, newBeatmap, bpmMultiplier));
            newBeatmap.Save();

            // post
            sound.SoundLocation = matchConfirmWav;
            sound.Play();

            BeatmapUpdateTimer.Start();

            EnableGenerateMapButton();
            GenerateMapButton.Text = oldButtonText;

            // reset diff name
            newBeatmap.Version = originalBeatmap.Version;
        }

        private void ModifyBeatmapTiming(Beatmap map, float multiplier)
        {
            if (multiplier == 1)
                return;

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
        // OUT: beatmap.Version
        // OUT: beatmap.Filename
        // OUT: beatmap.AudioFilename (if multiplier is not 1x)
        // OUT: beatmap.Tags
        private void ModifyBeatmapMetadata(Beatmap map, float multiplier)
        {
            if (multiplier == 1)
            {
                string ARODCS = "";
                if (newBeatmap.ApproachRate != originalBeatmap.ApproachRate)
                    ARODCS += $" AR{newBeatmap.ApproachRate}";
                if (newBeatmap.OverallDifficulty != originalBeatmap.OverallDifficulty)
                    ARODCS += $" OD{newBeatmap.OverallDifficulty}";
                if (newBeatmap.CircleSize != originalBeatmap.CircleSize)
                    ARODCS += $" CS{newBeatmap.CircleSize}";
                map.Version += ARODCS;
            }
            else
            {
                // If song has changed, no ARODCS in diff name
                var bpmsUnique = GetBpmList(map).Distinct().ToList();
                if (bpmsUnique.Count >= 2)
                    map.Version += $" x{multiplier}";
                else
                    map.Version += $" {(bpmsUnique[0]).ToString("0")}bpm";
                map.AudioFilename = map.AudioFilename.Substring(0, map.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + NormalizeText(map.Version) + ".mp3";
            }

            map.Filename = map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + NormalizeText(map.Artist) + " - " + NormalizeText(map.Title) + " (" + NormalizeText(map.Creator) + ")" + " [" + NormalizeText(map.Version) + "].osu";
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
            // set new multiplier
            bpmMultiplier = (float)BpmMultiplierUpDown.Value;

            // calculate new approach rate
            if (ScaleARCheck.Checked && !ARLockCheck.Checked)
            {
                newBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(originalBeatmap, bpmMultiplier);
            }
            // always need to update new bpm display
            BeatmapChanged();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }

        private bool LoadBeatmap(string beatmapPath)
        {
            // test if the beatmap is valid before committing to using it
            Beatmap test;
            try
            {
                test = new Beatmap(beatmapPath);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Bad .osu file format");
                originalBeatmap = null;
                newBeatmap = null;
                DisableFormControls();
                SongLabel.Text = $"Error reading .osu file";
                return false;
            }
            // Check if beatmap was loaded successfully
            if (test.Filename == null && test.Title == null)
                return false;

            // Check if this map was generated by osu-trainer
            if (test.Tags.Contains("osutrainer"))
            {
                string[] diffFiles = Directory.GetFiles(Path.GetDirectoryName(test.Filename), "*.osu");
                int candidateSimilarity = int.MaxValue;
                Beatmap candidate = null;
                foreach (string diff in diffFiles)
                {
                    Beatmap map = new Beatmap(diff);
                    if (map.Tags.Contains("osutrainer"))
                        continue;
                    // lower value => more similar
                    int similarity = Helper.LevenshteinDistance(test.Version, map.Version);
                    if (similarity < candidateSimilarity)
                    {
                        candidate = map;
                        candidateSimilarity = similarity;
                    }
                }
                // just assume this shit is the original beatmap
                if (candidate != null)
                    test = candidate;
            }

            // Commit to new beatmap
            pureBeatmap = null;
            originalBeatmap = new Beatmap(test.Filename);
            newBeatmap = new Beatmap(test.Filename);

            // Apply locked settings
            if (HPLockCheck.Checked)
                newBeatmap.HPDrainRate = lockedHP;
            if (CSLockCheck.Checked)
                newBeatmap.CircleSize = lockedCS;
            if (ODLockCheck.Checked)
                newBeatmap.OverallDifficulty = lockedOD;
            if (ARLockCheck.Checked)
                newBeatmap.ApproachRate = lockedAR;
            else if (ScaleARCheck.Checked)
                newBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(originalBeatmap, bpmMultiplier);

            // Update shit from top to bottom
            EnableFormControls();
            UpdateSongBg(newBeatmap);
            SongLabel.Text = $"{newBeatmap.Artist} - {newBeatmap.Title}";
            DiffLabel.Text = newBeatmap.Version;
            BeatmapChanged();

            return true;
        }

        private void UpdateSongBg(Beatmap map)
        {
            var imageEvent = map.Events.OfType<ContentEvent>().FirstOrDefault(e => e.Type == ContentType.Image);
            if (imageEvent == null)
            {
                // no background for this map
                Console.WriteLine("No image");
                return;
            }
                
            string imageAbsolutePath = Path.GetDirectoryName(map.Filename) + "\\" + imageEvent.Filename;
            if (!File.Exists(imageAbsolutePath))
            {
                // no background for this map
                Console.WriteLine("No image");
                return;
            }

            // crop height to aspect ratio
            Image bg = Image.FromFile(imageAbsolutePath);
            Bitmap bmpImage = new Bitmap(bg);
            float aspectRatio = BgPanel.Size.Width / BgPanel.Size.Height;
            int cropHeight = (int)(bmpImage.Width / aspectRatio);
            // pan down to center image
            int panDown = (bmpImage.Height - cropHeight) / 3;

            BgPanel.BackgroundImage = bmpImage.Clone(new Rectangle(0, panDown, bmpImage.Width, cropHeight), bmpImage.PixelFormat);
        }

        private List<float> GetBpmList(Beatmap map)
        {
            var bpms = map.TimingPoints.Where((tp) => !tp.InheritsBPM).Select((tp) => 60000 / tp.BpmDelay).ToList();
            var bpmsUnique = bpms.Distinct().ToList();
            if (bpmsUnique.Count == 1)
                return bpmsUnique;
            return bpms;
        }
        private string GetBeatmapDirectoryName()
        {
            return Path.GetDirectoryName(originalBeatmap.Filename);
        }

        #region Control State and Appearance
        private void DisableFormControls()
        {
            // top
            DisableSongDisplay();

            // labels
            foreach (var label in labels)
                label.ForeColor = labelDisabledColor;

            ScaleARCheck.Enabled = false;

            // updowns
            DisableDiffControls();
            DisableBpmUpDown();

            // generate map
            DisableGenerateMapButton();

        }
        private void EnableFormControls()
        {
            // top
            EnableSongDisplay();

            // labels
            foreach (var label in labels)
                label.ForeColor = labelColor1;

            ScaleARCheck.Enabled = true;

            // Diff Controls
            EnableDiffControls();

            // BPM Multiplier Up Down
            EnableBpmUpDown();

            // generate map
            EnableGenerateMapButton();
        }

        private void DisableSongDisplay()
        {
            SongLabel.Text = "no beatmap";
            SongLabel.ForeColor = labelDisabledColor;
            DiffLabel.Visible = false;
            StaticGif.Visible = true;
        }
        private void EnableSongDisplay()
        {
            SongLabel.ForeColor = labelColor2;
            DiffLabel.Visible = true;
            StaticGif.Visible = false;
        }

        private void EnableDiffControls()
        {
            foreach (var display in diffDisplays)
            {
                EnableTextbox(display);
            }
            BeatmapChanged();
            foreach (var slider in diffSliders)
            {
                slider.Enabled = true;
            }
        }
        private void DisableDiffControls()
        {
            foreach (var display in diffDisplays)
            {
                DisableTextbox(display);
            }
            foreach (var slider in diffSliders)
            {
                slider.Enabled = false;
            }
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
            BpmMultiplierUpDown.ForeColor = textBoxFg;
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
        private void BeatmapChanged()
        {
            // HP
            HPDisplay.Text = newBeatmap.HPDrainRate.ToString();
            HPSlider.Value = (decimal)newBeatmap.HPDrainRate;
            if (newBeatmap.HPDrainRate > originalBeatmap.HPDrainRate)
            {
                HPDisplay.ForeColor = accentRed;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Bold);
            }
            else if (newBeatmap.HPDrainRate < originalBeatmap.HPDrainRate)
            {
                HPDisplay.ForeColor = easierColor;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Bold);
            }
            else
            {
                HPDisplay.ForeColor = textBoxFg;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Regular);
            }

            // CS
            CSDisplay.Text = newBeatmap.CircleSize.ToString();
            CSSlider.Value = (decimal)newBeatmap.CircleSize;
            if (newBeatmap.CircleSize > originalBeatmap.CircleSize)
            {
                CSDisplay.ForeColor = accentRed;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Bold);
            }
            else if (newBeatmap.CircleSize < originalBeatmap.CircleSize)
            {
                CSDisplay.ForeColor = easierColor;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Bold);
            }
            else
            {
                CSDisplay.ForeColor = textBoxFg;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Regular);
            }

            // AR
            ARDisplay.Text = newBeatmap.ApproachRate.ToString();
            ARSlider.Value = (decimal)newBeatmap.ApproachRate;
            if (newBeatmap.ApproachRate > DifficultyCalculator.CalculateMultipliedAR(originalBeatmap, bpmMultiplier))
            {
                ARDisplay.ForeColor = accentRed;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else if (newBeatmap.ApproachRate < DifficultyCalculator.CalculateMultipliedAR(originalBeatmap, bpmMultiplier))
            {
                ARDisplay.ForeColor = easierColor;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else
            {
                ARDisplay.ForeColor = textBoxFg;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);
            }
            
            // OD
            ODDisplay.Text = newBeatmap.OverallDifficulty.ToString();
            ODSlider.Value = (decimal)newBeatmap.OverallDifficulty;
            if (newBeatmap.OverallDifficulty > originalBeatmap.OverallDifficulty)
            {
                ODDisplay.ForeColor = accentRed;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else if (newBeatmap.OverallDifficulty < originalBeatmap.OverallDifficulty)
            {
                ODDisplay.ForeColor = easierColor;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else
            {
                ODDisplay.ForeColor = textBoxFg;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Regular);
            }

            UpdateBpmDisplay();
        }
        private void UpdateBpmDisplay()
        {
            var originalBpms = GetBpmList(originalBeatmap).Select((bpm) => (int)bpm).ToList();
            var newBpms = GetBpmList(newBeatmap).Select((bpm) => (int)(bpm * bpmMultiplier)).ToList();

            if (new HashSet<int>(originalBpms).Count == 1)
            {
                OriginalBpmTextBox.Text = originalBpms[0].ToString();
                NewBpmTextBox.Text = newBpms[0].ToString();
                return;
            }

            OriginalBpmTextBox.Text = string.Join(" ... ", originalBpms);
            NewBpmTextBox.Text = string.Join(" ... ", newBpms);
        }
        private void DisableTextbox(TextBox textbox)
        {
            textbox.Enabled = false;
            textbox.BackColor = SystemColors.ControlDark;
            textbox.ForeColor = textBoxFg;
            textbox.Font = new Font(ARDisplay.Font, FontStyle.Regular);
        }
        private void EnableTextbox(TextBox textbox)
        {
            textbox.Enabled = true;
            textbox.BackColor = textBoxBg;
            textbox.ForeColor = textBoxFg;
            textbox.Font = new Font(ARDisplay.Font, FontStyle.Regular);
        }
        private void EnableTextCheckbox(CheckBox cb)
        {
            cb.BackColor = Color.FromArgb(98, 96, 104);
            cb.FlatAppearance.BorderColor = accentCyan;
            cb.ForeColor = accentCyan;
        }
        private void DisableTextCheckbox(CheckBox cb)
        {
            cb.BackColor = Color.FromArgb(38, 35, 53);
            cb.FlatAppearance.BorderColor = Color.FromArgb(98, 96, 104);
            cb.ForeColor = Color.FromArgb(98, 96, 104);
        }

        private void HPLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (originalBeatmap == null && newBeatmap == null)
                return;
            if (HPLockCheck.Checked)
            {
                EnableTextCheckbox(HPLockCheck);
                lockedHP = (float)HPSlider.Value;
            }
            else
                DisableTextCheckbox(HPLockCheck);
        }
        #endregion

        #region Event Handlers
        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Try to get osu install folder
            if (userOsuInstallPath == null)
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
                userOsuInstallPath = Path.GetDirectoryName(osuExePath);
            }

            // Read memory for current map
            string beatmapFolder = osu.GetMapFolderName();
            string beatmapFilename = osu.GetOsuFileName();
            string absoluteFilename = userOsuInstallPath + "\\Songs\\" + beatmapFolder + "\\" + beatmapFilename;

            // Invalid read
            if (beatmapFilename == "")
                return;

            // Beatmap name hasn't changed since last read
            if (previousBeatmapRead != null && previousBeatmapRead == beatmapFilename)
                return;
            previousBeatmapRead = beatmapFilename;

            // No beatmap currently loaded
            if (originalBeatmap == null)
            {
                LoadBeatmap(absoluteFilename);
                return;
            }

            // Beatmap Changed
            if (beatmapFilename != Path.GetFileName(originalBeatmap.Filename))
                LoadBeatmap(absoluteFilename);
        }

        private void HPSlider_ValueChanged(object sender, EventArgs e)
        {
            newBeatmap.HPDrainRate = (float)HPSlider.Value;
            BeatmapChanged();
            if (HPLockCheck.Checked)
                lockedHP = (float)HPSlider.Value;
        }

        private void CSSlider_ValueChanged(object sender, EventArgs e)
        {
            newBeatmap.CircleSize = (float)CSSlider.Value;
            BeatmapChanged();
            if (CSLockCheck.Checked)
                lockedCS = (float)CSSlider.Value;
        }

        private void ARSlider_ValueChanged(object sender, EventArgs e)
        {
            // disable ar scaling
            if (!ARLockCheck.Checked)
            {
                ScaleARCheck.Checked = false;
            }

            newBeatmap.ApproachRate = (float)ARSlider.Value;
            BeatmapChanged();
            if (ARLockCheck.Checked)
                lockedAR = (float)ARSlider.Value;
        }

        private void ODSlider_ValueChanged(object sender, EventArgs e)
        {
            newBeatmap.OverallDifficulty = (float)ODSlider.Value;
            BeatmapChanged();
            if (ODLockCheck.Checked)
                lockedOD = (float)ODSlider.Value;
        }
        private void CSLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (originalBeatmap == null && newBeatmap == null)
                return;
            if (CSLockCheck.Checked)
            {
                EnableTextCheckbox(CSLockCheck);
                lockedCS = (float)CSSlider.Value;
            }
            else
                DisableTextCheckbox(CSLockCheck);
        }
        private void ARLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (originalBeatmap == null && newBeatmap == null)
                return;
            if (ARLockCheck.Checked)
            {
                // save scale ar state
                scaleARPreviousState = ScaleARCheck.Checked;
                ScaleARCheck.Checked = false;
                EnableTextCheckbox(ARLockCheck);
                lockedAR = (float)ARSlider.Value;
            }
            else
            {
                // restore scale ar state
                ScaleARCheck.Checked = scaleARPreviousState;
                DisableTextCheckbox(ARLockCheck);
            }
        }
        private void ODLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (originalBeatmap == null && newBeatmap == null)
                return;
            if (ODLockCheck.Checked)
            {
                EnableTextCheckbox(ODLockCheck);
                lockedOD = (float)ODSlider.Value;
            }
            else
                DisableTextCheckbox(ODLockCheck);
        }
        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ARLockCheck.Checked)
            {
                // negate input
                ScaleARCheck.Checked = false;
                return;
            }
            if (ScaleARCheck.Checked)
            {
                newBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(originalBeatmap, bpmMultiplier);
                BeatmapChanged();
                ScaleARCheck.ForeColor = accentCyan;
            }
            else
            {
                ScaleARCheck.ForeColor = SystemColors.ControlDark;
            }
        }
        #endregion
    }
}
