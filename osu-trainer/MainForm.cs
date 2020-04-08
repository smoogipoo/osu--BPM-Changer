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
        IOsuMemoryReader osu;

        // Color Theme
        Color formBg = Color.FromArgb(38, 35, 53);
        Color textBoxBg = Color.FromArgb(23, 16, 25);
        Color textBoxFg = Color.FromArgb(224, 224, 224);
        Color accentSalmon = Color.FromArgb(249, 126, 114);
        Color accentYellow = Color.FromArgb(254, 222, 93);
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
        List<Label> dumbLabels;
        List<TextBox> diffDisplays;
        List<OptionSlider> diffSliders;

        // Single Object Instances
        SoundPlayer sound = new SoundPlayer();
        BeatmapEditor beatmapEditor;

        // other
        private bool scaleARPreviousState;
        private string previousBeatmapRead;
        private bool diffCalcReady = true;
        private bool busyUpdatingBeatmap = false;
        private int beatmapFindFailCounter = 0;

        public MainForm()
        {
            InitializeComponent();

            nobg = Image.FromFile(nobgpng);

            // init control lists
            dumbLabels = new List<Label>
            {
                hplabel, cslabel, arlabel, odlabel,
                label2, label4, label5
            };
            diffDisplays = new List<TextBox>
            {
                HPDisplay, CSDisplay, ARDisplay, ODDisplay
            };


            // Init object instances
            osu = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");
            beatmapEditor = new BeatmapEditor(this);

            // Add event handlers
            beatmapEditor.StateChanged     += UpdateDumbLabels;
            beatmapEditor.StateChanged     += UpdateGenerateButton;
            beatmapEditor.BeatmapModified  += UpdateSongDisplay;
            beatmapEditor.BeatmapModified  += UpdateBpmDisplay;
            beatmapEditor.BeatmapModified  += UpdateHpCsArOdDisplay;
            beatmapEditor.BeatmapModified  += UpdateDifficultyDisplay;
            beatmapEditor.ControlsModified += UpdateLockButtons;
            beatmapEditor.ControlsModified += UpdateScaleButtons;

            beatmapEditor.SetState(EditorState.NOT_READY);
            beatmapEditor.NotReadyReason = BadBeatmapReason.NO_BEATMAP_LOADED;

            beatmapEditor.ForceUpdate();

            BeatmapUpdateTimer.Start();
        }

        void UpdateSongDisplay(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    switch (editor.NotReadyReason)
                    {
                        case BadBeatmapReason.NO_BEATMAP_LOADED:
                            SongLabel.Text = "no beatmap";
                            SongLabel.ForeColor = labelDisabledColor;
                            StaticGif.Visible = true;
                            DiffLabel.Visible = false;
                            break;
                        case BadBeatmapReason.ERROR_LOADING_BEATMAP:
                            SongLabel.Text = "Unable to read .osu file";
                            SongLabel.ForeColor = labelDisabledColor;
                            StaticGif.Visible = true;
                            DiffLabel.Visible = false;
                            break;
                        case BadBeatmapReason.DIFF_NOT_OSUSTD:
                            break;
                        default:
                            break;
                    }
                    break;
                case EditorState.READY:
                case EditorState.LOADING_BEATMAP:
                case EditorState.GENERATING_BEATMAP:
                    SongLabel.ForeColor = accentYellow;
                    StaticGif.Visible = false;
                    DiffLabel.Visible = true;

                    string artist = beatmapEditor.OriginalBeatmap.Artist;
                    string title = beatmapEditor.OriginalBeatmap.Title;
                    string diff = beatmapEditor.OriginalBeatmap.Version;
                    SongLabel.Text = TruncateLabelText($"{artist} - {title}", SongLabel);
                    DiffLabel.Text = TruncateLabelText(diff, DiffLabel);
                    break;
            }
        }

        void UpdateDumbLabels(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            Color labelColor = accentSalmon;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    labelColor = labelDisabledColor;
                    break;
                case EditorState.READY:
                case EditorState.LOADING_BEATMAP:
                case EditorState.GENERATING_BEATMAP:
                    labelColor = accentSalmon;
                    break;
            }
            foreach (var label in dumbLabels)
                label.ForeColor = labelColor;
        }

        void UpdateBpmDisplay(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                case EditorState.LOADING_BEATMAP:
                    //var originalBpms = GetBpmList(originalBeatmap).Select((bpm) => (int)bpm).ToList();
                    //var newBpms = GetBpmList(newBeatmap).Select((bpm) => (int)(bpm)).ToList();

                    //NewBpmTextBox.ForeColor = (bpmMultiplier > 1) ? accentRed : (bpmMultiplier < 1) ? easierColor : textBoxFg;

                    //OriginalBpmTextBox.Text = GetDominantBpm(originalBeatmap).ToString("0");
                    //NewBpmTextBox.Text = GetDominantBpm(newBeatmap).ToString("0");
                    //if (GetBpmList(originalBeatmap).Distinct().ToList().Count > 1)
                    //{
                    //    var oldbpms = GetBpmList(originalBeatmap);
                    //    float oldmin = oldbpms.Min();
                    //    float oldmax = oldbpms.Max();
                    //    OriginalBpmTextBox.Text += $" ({oldmin.ToString("0")} - {oldmax.ToString("0")})";

                    //    var newbpms = GetBpmList(newBeatmap);
                    //    float newmin = newbpms.Min();
                    //    float newmax = newbpms.Max();
                    //    NewBpmTextBox.Text += $" ({newmin.ToString("0")} - {newmax.ToString("0")})";
                    //}
                    break;
                case EditorState.GENERATING_BEATMAP:
                    break;
            }
        }


        void UpdateLockButtons(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                    break;
                case EditorState.LOADING_BEATMAP:
                    break;
                case EditorState.GENERATING_BEATMAP:
                    break;
            }
        }

        void UpdateScaleButtons(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                    break;
                case EditorState.LOADING_BEATMAP:
                    break;
                case EditorState.GENERATING_BEATMAP:
                    break;
            }
        }

        void UpdateHpCsArOdDisplay(object sender, EventArgs e)
        {
            bool enabled = (beatmapEditor.GetState() == EditorState.READY || beatmapEditor.GetState() == EditorState.LOADING_BEATMAP);
            foreach (var textbox in diffDisplays)
            {
                textbox.Enabled = enabled ? true : false;
                textbox.BackColor = enabled ? textBoxBg : SystemColors.ControlDark;
                textbox.ForeColor = textBoxFg;
                textbox.Font = new Font(ARDisplay.Font, FontStyle.Regular);
            }
            if (!enabled)
                return;

            // HP
            float newHP      = beatmapEditor.NewBeatmap.HPDrainRate;
            float originalHP = beatmapEditor.OriginalBeatmap.HPDrainRate;
            HPDisplay.Text = newHP.ToString();
            HPSlider.Value = (decimal)newHP;
            if (newHP > originalHP)
            {
                HPDisplay.ForeColor = accentRed;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Bold);
            }
            else if (newHP < originalHP)
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
            float newCS      = beatmapEditor.NewBeatmap.CircleSize;
            float originalCS = beatmapEditor.OriginalBeatmap.CircleSize;
            CSDisplay.Text = newCS.ToString();
            CSSlider.Value = (decimal)newCS;
            if (newCS > originalCS)
            {
                CSDisplay.ForeColor = accentRed;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Bold);
            }
            else if (newCS < originalCS)
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
            float newAR      = beatmapEditor.NewBeatmap.ApproachRate;
            ARDisplay.Text = newAR.ToString();
            ARSlider.Value = (decimal)newAR;
            if (newAR > beatmapEditor.GetScaledAR())
            {
                ARDisplay.ForeColor = accentRed;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else if (newAR < beatmapEditor.GetScaledAR())
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
            float newOD      = beatmapEditor.NewBeatmap.OverallDifficulty;
            float originalOD = beatmapEditor.OriginalBeatmap.OverallDifficulty;
            ODDisplay.Text = newOD.ToString();
            ODSlider.Value = (decimal)newOD;
            if (newOD > originalOD)
            {
                ODDisplay.ForeColor = accentRed;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else if (newOD < originalOD)
            {
                ODDisplay.ForeColor = easierColor;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else
            {
                ODDisplay.ForeColor = textBoxFg;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Regular);
            }
        }

        void UpdateDifficultyDisplay(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                    break;
                case EditorState.LOADING_BEATMAP:
                    break;
                case EditorState.GENERATING_BEATMAP:
                    break;
            }
        }


        void UpdateGenerateButton(object sender, EventArgs e)
        {
            BeatmapEditor editor = (BeatmapEditor)sender;
            switch (editor.GetState())
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                    break;
                case EditorState.LOADING_BEATMAP:
                    break;
                case EditorState.GENERATING_BEATMAP:
                    break;
            }
        }
        


        private void GenerateMapButton_Click(object sender, EventArgs e)
        {
            beatmapEditor.GenerateBeatmap();
        }

        public void PlayDoneSound()
        {
            sound.SoundLocation = matchConfirmWav;
            sound.Play();
        }



        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            // set new multiplier
            beatmapEditor.SetBpmMultiplier((float)BpmMultiplierUpDown.Value);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && GenerateMapButton.Enabled)
                GenerateMapButton_Click(null, null);
        }

        private void LoadBeatmap(string beatmapPath)
        {
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
            //if (originalBeatmap == null || beatmapFilename != Path.GetFileName(originalBeatmap.Filename))
            //    LoadBeatmap(absoluteFilename);
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


        #region Control State and Appearance
        private void DisableFormControls()
        {
            // top
            DisableSongDisplay();

            // labels
            foreach (var label in dumbLabels)
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
            foreach (var label in dumbLabels)
                label.ForeColor = accentSalmon;

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
            SongLabel.ForeColor = accentYellow;
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
            if (!beatmapEditor.NewMapIsDifferent())
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

            //// Star Rating
            //if ((bpmChanged || csChanged || newBeatmapLoaded) && diffCalcReady)
            //{
            //    diffCalcReady = false;
            //    DiffCalcCooldown.Start();
            //    float stars, aimStars, speedStars = -1.0f;
            //    try
            //    {
            //        (stars, aimStars, speedStars) = await Task.Run(() => DifficultyCalculator.CalculateStarRating(newBeatmap));
            //    }
            //    catch (NullReferenceException e) {
            //        // just do nothing, wait for next chance to recalculate difficulty
            //        Console.WriteLine(e);
            //        Console.WriteLine("lol asdfasdf;lkjasdf");
            //        return;
            //    }
            //    if (stars < 0)
            //        return;

            //    int aimPercent = (int)(100.0f * aimStars / (aimStars + speedStars));
            //    int speedPercent = 100 - aimPercent;
            //    StarLabel.Text = $"{stars:0.00} ☆";
            //    AimLabel.Text = $"aim: {aimPercent}%";
            //    SpeedLabel.Text = $"spd: {speedPercent}%";
            //    AimSpeedBar.LeftPercent = aimPercent;
            //}

            //UpdateBpmDisplay();

            //// Generate Map Button
            //if (beatmapEditor.NewMapIsDifferent())
            //    EnableGenerateMapButton();
            //else
            //    DisableGenerateMapButton();

        }
        private void UpdateBpmDisplay()
        {
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
            beatmapEditor.SetHP((float)HPSlider.Value);
        }

        private void CSSlider_ValueChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetCS((float)CSSlider.Value);
        }

        private void ARSlider_ValueChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetAR((float)ARSlider.Value);
        }

        private void ODSlider_ValueChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetCS((float)ODSlider.Value);
        }
        private void HPLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetHPLock(HPLockCheck.Checked);
        }
        private void CSLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetCSLock(CSLockCheck.Checked);
        }
        private void ARLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetARLock(ARLockCheck.Checked);
        }
        private void ODLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetODLock(ODLockCheck.Checked);
        }
        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e)
        {
            beatmapEditor.SetScaleAR(ScaleARCheck.Checked);
        }

        private void DiffCalcCooldown_Tick(object sender, EventArgs e)
        {
            diffCalcReady = true;
            DiffCalcCooldown.Stop();
        }
        #endregion

    }
}
