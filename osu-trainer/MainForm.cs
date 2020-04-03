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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace osu_trainer
{
    public partial class MainForm : Form
    {
        // File Resources
        string matchConfirmWav = "resources\\match-confirm.wav";
        string nobgpng = "resources\\nobg.png";
        static Image nobg;

        // Beatmap
        string userSongsFolder = null;
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
        Color accentPink = Color.FromArgb(243, 114, 185);
        Color accentBlue = Color.FromArgb(46, 226, 250);
        Color accentOrange = Color.FromArgb(246, 122, 44);
        Color accentRed = Color.FromArgb(254, 68, 80);
        Color accentCyan = Color.FromArgb(46, 226, 250);
        Color easierColor = Color.FromArgb(114, 241, 184);
        Color aimColor = Color.FromArgb(255, 133, 201);
        Color speedColor = Color.FromArgb(114, 241, 184);
        Color starsColor = Color.FromArgb(225, 132, 65);

        // Common Control Lists
        List<Label> labels;
        List<TextBox> diffDisplays;
        List<OptionSlider> diffSliders;

        // Single Object Instances
        SoundPlayer sound = new SoundPlayer();

        // other
        private bool scaleARPreviousState;
        private string previousBeatmapRead;
        private bool diffCalcReady = true;
        private bool busyUpdatingBeatmap = false;
        private int beatmapFindFailCounter = 0;


        public MainForm()
        {
            InitializeComponent();
            InitializeControlLists();

            nobg = Image.FromFile(nobgpng);

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
            ModifyBeatmapTiming(originalBeatmap, newBeatmap, bpmMultiplier);
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

        // it is safe to call this function repeatedly
        private void ModifyBeatmapTiming(Beatmap originalMap, Beatmap newMap, float multiplier)
        {
            if (multiplier == 1)
                return;

            // Want to divide timestamps since high multiplier => shorter time
            // OUT: tp.BpmDelay          for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            for (int i = 0; i < originalMap.TimingPoints.Count; i++)
            {
                var originalTimingPoint = originalMap.TimingPoints[i];
                var newTimingPoint = newMap.TimingPoints[i];
                if (originalTimingPoint.InheritsBPM == false)
                {
                    float oldBpm = 60000 / originalTimingPoint.BpmDelay;
                    float newBpm = oldBpm * multiplier;
                    float newDelay = 60000 / newBpm;
                    newTimingPoint.BpmDelay = newDelay;
                    newTimingPoint.Time = (int)(originalTimingPoint.Time / multiplier);
                }
                else
                {
                    newTimingPoint.Time = (int)(originalTimingPoint.Time / multiplier);
                }
            }
            //foreach (TimingPoint timingPoint in newMap.TimingPoints)
            //{
            //    if (timingPoint.InheritsBPM == false)
            //    {
            //        float oldBpm = 60000 / timingPoint.BpmDelay;
            //        float newBpm = oldBpm * multiplier;
            //        float newDelay = 60000 / newBpm;
            //        timingPoint.BpmDelay = newDelay;
            //        timingPoint.Time = (int)(timingPoint.Time / multiplier);
            //    }
            //    else
            //    {
            //        timingPoint.Time = (int)(timingPoint.Time / multiplier);
            //    }
            //}

            // OUT: event.StartTime      for each event in beatmap
            // OUT: event.EndTime        for each break event in beatmap
            for (int i = 0; i < originalMap.Events.Count; i++)
            {
                var originalEvent = originalMap.Events[i];
                var newEvent = newMap.Events[i];
                newEvent.StartTime = (int)(originalEvent.StartTime / multiplier);
                if (originalEvent.GetType() == typeof(BreakEvent))
                    ((BreakEvent)newEvent).EndTime = (int)(((BreakEvent)originalEvent).EndTime / multiplier);
            }
            //foreach (EventBase e in newMap.Events)
            //{
            //    e.StartTime = (int)(e.StartTime / multiplier);
            //    if (e.GetType() == typeof(BreakEvent))
            //        ((BreakEvent)e).EndTime = (int)(((BreakEvent)e).EndTime / multiplier);
            //}

            // OUT: hitobject.StartTime         for each hit object in beatmap
            // OUT: hitobject.EndTime           for each spinner in beatmap
            for (int i = 0; i < originalMap.HitObjects.Count; i++)
            {
                var originalObject = originalMap.HitObjects[i];
                var newObject = newMap.HitObjects[i];
                newObject.StartTime = (int)(originalObject.StartTime / multiplier);
                if (originalObject.GetType() == typeof(SpinnerObject))
                    ((SpinnerObject)newObject).EndTime = (int)(((SpinnerObject)originalObject).EndTime / multiplier);
            }
            //foreach (CircleObject hitobject in newMap.HitObjects)
            //{
            //    hitobject.StartTime = (int)(hitobject.StartTime / multiplier);
            //    if (hitobject.GetType() == typeof(SpinnerObject))
            //        ((SpinnerObject)hitobject).EndTime = (int)(((SpinnerObject)hitobject).EndTime / multiplier);
            //}
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
            // apply changes to objects
            ModifyBeatmapTiming(originalBeatmap, newBeatmap, bpmMultiplier);
            // always need to update new bpm display
            BeatmapChanged(bpmChanged: true);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }

        private void LoadBeatmap(string beatmapPath)
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
                return;
            }
            // Check if beatmap was loaded successfully
            if (test.Filename == null && test.Title == null)
            {
                return;
            }

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
            ModifyBeatmapTiming(originalBeatmap, newBeatmap, bpmMultiplier); // for diffcalc

            // Check if game mode is osu! standard
            if (newBeatmap.Mode != GameMode.osu)
            {
                // Partially enable controls
                DisableFormControls();
                StaticGif.Visible = false;
                UpdateSongBg(newBeatmap);
                SongLabel.Visible = true;
                SongLabel.ForeColor = labelColor2;
                SongLabel.Text = TruncateLabelText($"{newBeatmap.Artist} - {newBeatmap.Title}", SongLabel);
                DiffLabel.Visible = true;
                DiffLabel.ForeColor = accentRed;
                DiffLabel.Text = $"not osu!std";
                return;
            }

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

            // Update
            EnableFormControls();
            BeatmapChanged(newBeatmapLoaded: true);
            UpdateSongBg(newBeatmap);
            SongLabel.Text = TruncateLabelText($"{newBeatmap.Artist} - {newBeatmap.Title}", SongLabel);
            DiffLabel.Text = TruncateLabelText(newBeatmap.Version, DiffLabel);
            GetDominantBpm(newBeatmap);
            return;
        }
        private string TruncateLabelText(string txt, Label label)
        {
            if (TextRenderer.MeasureText(txt, label.Font).Width > label.Width)
            {
                string truncated = txt;
                while (TextRenderer.MeasureText(truncated, label.Font).Width > label.Width - 10)
                    truncated = truncated.Substring(0, truncated.Length-1);
                truncated += "...";
                return truncated;
            }
            return txt;
        }

        // Detection of user Songs folder also happens here
        private void DetectCurrentBeatmap()
        {
            // Try to get osu install folder
            if (userSongsFolder == null)
            {
                if (Directory.Exists(Properties.Settings.Default.SongsFolder))
                    userSongsFolder = Properties.Settings.Default.SongsFolder;
            }
            if (userSongsFolder == null)
            {
                // check if osu!.exe is running
                var processes = Process.GetProcessesByName("osu!");
                if (processes.Length == 0)
                    return;

                // set path
                var osuExePath = processes[0].MainModule.FileName;
                userSongsFolder = Path.GetDirectoryName(osuExePath) + "\\Songs";
                Properties.Settings.Default.SongsFolder = userSongsFolder;
                Properties.Settings.Default.Save();
            }

            // Read memory for current map
            string beatmapFolder = osu.GetMapFolderName();
            string beatmapFilename = osu.GetOsuFileName();

            // Read unsuccessful (osu might not be running)
            if (beatmapFilename == "")
                return;

            // Beatmap name hasn't changed since last read
            if (previousBeatmapRead != null && previousBeatmapRead == beatmapFilename)
                return;
            previousBeatmapRead = beatmapFilename;

            // Try to locate the beatmap
            string absoluteFilename = userSongsFolder + "\\" + beatmapFolder + "\\" + beatmapFilename;
            if (!File.Exists(absoluteFilename))
            {
                Console.WriteLine(beatmapFindFailCounter + 1);
                if (++beatmapFindFailCounter == 10)
                {
                    string msg = "Automatic beatmap detection failed 10 times in a row. ";
                    msg += "Your songs folder is probably somewhere else. ";
                    msg += "Please manually select your Songs folder in the next window.";
                    MessageBox.Show(msg, "Having trouble finding your beatmaps...", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    using (var folderDialog = new OpenFileDialog())
                    {
                        folderDialog.Title = "Select your osu! installation folder";
                        folderDialog.ValidateNames = false;
                        folderDialog.CheckFileExists = false;
                        folderDialog.CheckPathExists = true;
                        folderDialog.FileName = "Select Folder";
                        if (folderDialog.ShowDialog() != DialogResult.OK)
                            return;

                        userSongsFolder = Path.GetDirectoryName(folderDialog.FileName);
                        Properties.Settings.Default.SongsFolder = userSongsFolder;
                        Properties.Settings.Default.Save();
                        // try again
                        absoluteFilename = userSongsFolder + "\\" + beatmapFolder + "\\" + beatmapFilename;
                        if (!File.Exists(absoluteFilename))
                            return;
                    }
                }
                return;
            }
            beatmapFindFailCounter = 0;

            // No beatmap currently loaded, or beatmap changed
            if (originalBeatmap == null || beatmapFilename != Path.GetFileName(originalBeatmap.Filename))
                LoadBeatmap(absoluteFilename);
        }

        private void UpdateSongBg(Beatmap map)
        {
            var imageEvent = map.Events.OfType<ContentEvent>().FirstOrDefault(e => e.Type == ContentType.Image);
            if (imageEvent == null)
            {
                // no background for this map
                BgPanel.BackgroundImage = CropAndPanToFit(nobg, BgPanel.Size.Width, BgPanel.Size.Height);
                return;
            }
                
            string imageAbsolutePath = Path.GetDirectoryName(map.Filename) + "\\" + imageEvent.Filename;
            if (!File.Exists(imageAbsolutePath))
            {
                // no background for this map
                BgPanel.BackgroundImage = CropAndPanToFit(nobg, BgPanel.Size.Width, BgPanel.Size.Height);
                return;
            }

            // crop height to aspect ratio
            Image bg = Image.FromFile(imageAbsolutePath);
            BgPanel.BackgroundImage = CropAndPanToFit(bg, BgPanel.Size.Width, BgPanel.Size.Height);
        }
        private Bitmap CropAndPanToFit(Image img, int destinationWidth, int destinationHeight)
        {
            Bitmap bmpImage = new Bitmap(img);
            float aspectRatio = destinationWidth / destinationHeight;
            int cropHeight = (int)(bmpImage.Width / aspectRatio);
            // pan down to center image
            int panDown = (bmpImage.Height - cropHeight) / 3;
            // crop and pan via cloning
            return bmpImage.Clone(new Rectangle(0, panDown, bmpImage.Width, cropHeight), bmpImage.PixelFormat);
        }

        private float GetDominantBpm(Beatmap map)
        {
            var bpms = GetBpmList(map);
            if (bpms.Count == 1)
                return bpms[0];
            // store bpm => prominence (as in, how long that bpm is active in the map)
            float previousTime = 0;
            float previousBpm = 0;
            var bpmTimingPoints = map.TimingPoints.Where(tp => !tp.InheritsBPM).ToList();
            var bpmProminenceValues = new Dictionary<float, float>();

            for (int i = 0; i < bpmTimingPoints.Count; i++)
            {
                var tp = bpmTimingPoints[i];
                var currentBpm = 60000 / tp.BpmDelay;
                var currentTime = tp.Time;
                // case: first timing point
                if (i == 0)
                {
                    previousBpm = currentBpm;
                    previousTime = currentTime;
                }
                // case: middle timing point
                else if (i < bpmTimingPoints.Count - 1)
                {
                    if (!bpmProminenceValues.ContainsKey(previousBpm))
                        bpmProminenceValues.Add(previousBpm, 0);
                    float duration = currentTime - previousTime;
                    bpmProminenceValues[previousBpm] += duration;

                    previousBpm = currentBpm;
                    previousTime = currentTime;
                }
                // case: last timing point
                else if (i == bpmTimingPoints.Count - 1)
                {
                    if (!bpmProminenceValues.ContainsKey(previousBpm))
                        bpmProminenceValues.Add(previousBpm, 0);
                    float duration = currentTime - previousTime;
                    bpmProminenceValues[previousBpm] += duration;

                    // jump ahead in time to last hit object in map
                    if (!bpmProminenceValues.ContainsKey(currentBpm))
                        bpmProminenceValues.Add(currentBpm, 0);
                    float finalTime = map.HitObjects.Last().StartTime;
                    bpmProminenceValues[currentBpm] += finalTime - currentTime;
                }
            }
            var lines = bpmProminenceValues.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            Console.WriteLine(string.Join(Environment.NewLine, lines));

            float candidateBpm = 0;
            float maxProminence = float.MinValue;
            foreach (KeyValuePair<float, float> entry in bpmProminenceValues)
            {
                if (entry.Value > maxProminence)
                {
                    candidateBpm = entry.Key;
                    maxProminence = entry.Value;
                }
            }
            return candidateBpm;
        }

        private List<float> GetBpmList(Beatmap map)
        {
            if (map == null)
                return new List<float> { 0.0f };
            var bpms = map.TimingPoints.Where((tp) => !tp.InheritsBPM).Select((tp) => 60000 / tp.BpmDelay).ToList();
            var bpmsUnique = bpms.Distinct().ToList();
            if (bpmsUnique.Count == 1)
                return bpmsUnique;
            return bpms;
        }

        private bool NewMapIsDifferent()
        {
            return (
                newBeatmap.HPDrainRate != originalBeatmap.HPDrainRate ||
                newBeatmap.CircleSize != originalBeatmap.CircleSize ||
                newBeatmap.ApproachRate != originalBeatmap.ApproachRate ||
                newBeatmap.OverallDifficulty != originalBeatmap.OverallDifficulty ||
                bpmMultiplier != 1.0f
                );
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

            // updowns
            DisableDiffControls();
            DisableBpmUpDown();

            // Star Rating Display
            StarLabel.ForeColor = labelDisabledColor;
            StarLabel.Text = "☆";
            AimLabel.ForeColor = labelDisabledColor;
            AimLabel.Text = "";
            SpeedLabel.ForeColor = labelDisabledColor;
            SpeedLabel.Text = "";
            AimSpeedBar.LeftColour = labelDisabledColor;
            AimSpeedBar.RightColour = labelDisabledColor;

            // BPM Display
            OriginalBpmTextBox.ForeColor = textBoxFg;
            NewBpmTextBox.ForeColor = textBoxFg;

            // generate map
            DisableGenerateMapButton();

        }
        private void EnableFormControls()
        {
            // Song Display
            EnableSongDisplay();

            // Labels
            foreach (var label in labels)
                label.ForeColor = labelColor1;

            // Diff Controls
            EnableDiffControls();

            // BPM Multiplier Up Down
            EnableBpmUpDown();

            // Star Rating Display
            StarLabel.ForeColor = starsColor;
            AimLabel.ForeColor = aimColor;
            SpeedLabel.ForeColor = speedColor;
            AimSpeedBar.LeftColour = aimColor;
            AimSpeedBar.RightColour = speedColor;

            // BPM Display
            OriginalBpmTextBox.ForeColor = textBoxFg;
            NewBpmTextBox.ForeColor = textBoxFg;

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
            if (!NewMapIsDifferent())
                return;
            GenerateMapButton.Enabled = true;
            GenerateMapButton.ForeColor = Color.White;
            GenerateMapButton.BackColor = accentPink;
        }
        private void DisableGenerateMapButton()
        {
            GenerateMapButton.ForeColor = Color.DimGray;
            GenerateMapButton.BackColor = SystemColors.ControlLight;
            GenerateMapButton.Enabled = false;
        }

        // making this async seems to be causing a lot of headaches...
        private async void BeatmapChanged(bool bpmChanged = false, bool csChanged = false, bool newBeatmapLoaded = false)
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

            // Star Rating
            if ((bpmChanged || csChanged || newBeatmapLoaded) && diffCalcReady)
            {
                diffCalcReady = false;
                DiffCalcCooldown.Start();
                float stars, aimStars, speedStars = -1.0f;
                try
                {
                    (stars, aimStars, speedStars) = await Task.Run(() => DifficultyCalculator.CalculateStarRating(newBeatmap));
                }
                catch (NullReferenceException e) {
                    // just do nothing, wait for next chance to recalculate difficulty
                    Console.WriteLine(e);
                    Console.WriteLine("lol asdfasdf;lkjasdf");
                    return;
                }
                if (stars < 0)
                    return;

                int aimPercent = (int)(100.0f * aimStars / (aimStars + speedStars));
                int speedPercent = 100 - aimPercent;
                StarLabel.Text = $"{stars:0.00} ☆";
                AimLabel.Text = $"aim: {aimPercent}%";
                SpeedLabel.Text = $"spd: {speedPercent}%";
                AimSpeedBar.LeftPercent = aimPercent;
            }

            UpdateBpmDisplay();

            // Generate Map Button
            if (NewMapIsDifferent())
                EnableGenerateMapButton();
            else
                DisableGenerateMapButton();

        }
        private void UpdateBpmDisplay()
        {
            var originalBpms = GetBpmList(originalBeatmap).Select((bpm) => (int)bpm).ToList();
            var newBpms = GetBpmList(newBeatmap).Select((bpm) => (int)(bpm)).ToList();

            NewBpmTextBox.ForeColor = (bpmMultiplier > 1) ? accentRed : (bpmMultiplier < 1) ? easierColor : textBoxFg;

            OriginalBpmTextBox.Text = GetDominantBpm(originalBeatmap).ToString("0");
            NewBpmTextBox.Text = GetDominantBpm(newBeatmap).ToString("0");
            if (GetBpmList(originalBeatmap).Distinct().ToList().Count > 1)
            {
                var oldbpms = GetBpmList(originalBeatmap);
                float oldmin = oldbpms.Min();
                float oldmax = oldbpms.Max();
                OriginalBpmTextBox.Text += $" ({oldmin.ToString("0")} - {oldmax.ToString("0")})";

                var newbpms = GetBpmList(newBeatmap);
                float newmin = newbpms.Min();
                float newmax = newbpms.Max();
                NewBpmTextBox.Text += $" ({newmin.ToString("0")} - {newmax.ToString("0")})";
            }
            return;
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
            cb.BackColor = Color.FromArgb(38, 35, 53);
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
            if (busyUpdatingBeatmap)
                return;
            busyUpdatingBeatmap = true;
            DetectCurrentBeatmap();
            busyUpdatingBeatmap = false;
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
            BeatmapChanged(csChanged: true);
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
                DisableTextCheckbox(ScaleARCheck);
                EnableTextCheckbox(ARLockCheck);
                lockedAR = (float)ARSlider.Value;
            }
            else
            {
                // restore scale ar state
                ScaleARCheck.Checked = scaleARPreviousState;
                if (ScaleARCheck.Checked)
                    EnableTextCheckbox(ScaleARCheck);
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
                EnableTextCheckbox(ScaleARCheck);
            }
            else
            {
                DisableTextCheckbox(ScaleARCheck);
            }
        }

        private void DiffCalcCooldown_Tick(object sender, EventArgs e)
        {
            diffCalcReady = true;
            DiffCalcCooldown.Stop();
        }
        #endregion

    }
}
