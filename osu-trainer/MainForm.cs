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
        IOsuMemoryReader osuReader;

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
        BeatmapEditor editor;

        // other
        private bool scaleARPreviousState;
        private string previousBeatmapRead;
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
            diffSliders = new List<OptionSlider>
            {
                HPSlider, CSSlider, ARSlider, ODSlider
            };


            // Init object instances
            osuReader = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");
            editor = new BeatmapEditor(this);

            // Add event handlers (observers)
            editor.StateChanged     += ToggleDumbLabels;
            editor.StateChanged     += ToggleGenerateButton;
            editor.StateChanged     += ToggleHpCsArOdDisplay;
            editor.StateChanged     += ToggleDifficultyDisplay;
            editor.StateChanged     += ToggleBpmUpDown;
            editor.BeatmapSwitched  += UpdateSongDisplay;
            editor.BeatmapModified  += UpdateBpmDisplay;
            editor.BeatmapModified  += UpdateHpCsArOdDisplay;
            editor.BeatmapModified  += UpdateDifficultyDisplay;
            editor.BeatmapModified  += ToggleGenerateButton;
            editor.BeatmapModified  += UpdateBpmUpDown;
            editor.ControlsModified += UpdateLockButtons;
            editor.ControlsModified += UpdateScaleButtons;

            // need controls to show up as disabled
            editor.ForceEventStateChanged();
            editor.ForceEventBeatmapSwitched();

            BeatmapUpdateTimer.Start();
        }

        private void UpdateBpmUpDown(object sender, EventArgs e)
        {
            BpmMultiplierUpDown.Value = (decimal)editor.BpmMultiplier;
        }

        void UpdateSongDisplay(object sender, EventArgs e)
        {
            switch (editor.State)
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
                case EditorState.GENERATING_BEATMAP:
                    UpdateSongBg(editor.NewBeatmap);

                    SongLabel.ForeColor = accentYellow;
                    DiffLabel.ForeColor = accentYellow;
                    StaticGif.Visible = false;
                    DiffLabel.Visible = true;

                    string artist = editor.OriginalBeatmap.Artist;
                    string title = editor.OriginalBeatmap.Title;
                    string diff = editor.OriginalBeatmap.Version;
                    SongLabel.Text = TruncateLabelText($"{artist} - {title}", SongLabel);
                    DiffLabel.Text = TruncateLabelText(diff, DiffLabel);
                    break;
            }
        }

        void ToggleDumbLabels(object sender, EventArgs e)
        {
            Color labelColor = accentSalmon;
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    labelColor = labelDisabledColor;
                    break;
                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    labelColor = accentSalmon;
                    break;
            }
            foreach (var label in dumbLabels)
                label.ForeColor = labelColor;
        }

        void UpdateBpmDisplay(object sender, EventArgs e)
        {
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    OriginalBpmTextBox.BackColor = labelDisabledColor;
                    OriginalBpmRangeTextBox.Visible = false;
                    NewBpmTextBox.BackColor = labelDisabledColor;
                    NewBpmRangeTextBox.Visible = false;
                    break;
                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    (float oldbpm, float oldmin, float oldmax) = editor.GetOriginalBpmData();
                    (float newbpm, float newmin, float newmax) = editor.GetNewBpmData();

                    // bpm
                    OriginalBpmTextBox.BackColor    = textBoxBg;
                    NewBpmTextBox.BackColor         = textBoxBg;
                    OriginalBpmTextBox.Text         = Math.Round(oldbpm).ToString("0");
                    NewBpmTextBox.Text              = Math.Round(newbpm).ToString("0");
                    if (newbpm > oldbpm)
                        NewBpmTextBox.ForeColor = accentRed;
                    else if (newbpm < oldbpm)
                        NewBpmTextBox.ForeColor = easierColor;
                    else
                        NewBpmTextBox.ForeColor = textBoxFg;

                    // bpm range
                    OriginalBpmRangeTextBox.Text    = $"({Math.Round(oldmin).ToString("0")} - {Math.Round(oldmax).ToString("0")})";
                    NewBpmRangeTextBox.Text         = $"({Math.Round(newmin).ToString("0")} - {Math.Round(newmax).ToString("0")})";
                    OriginalBpmRangeTextBox.Visible = (oldmin != oldmax);
                    NewBpmRangeTextBox.Visible      = (oldmin != oldmax);
                    break;
            }
        }


        void UpdateLockButtons(object sender, EventArgs e)
        {
            HPLockCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.HpIsLocked ? accentBlue : Color.Gray;
            HPLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.HpIsLocked ? accentBlue : Color.Gray;
            CSLockCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.CsIsLocked ? accentBlue : Color.Gray;
            CSLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.CsIsLocked ? accentBlue : Color.Gray;
            ARLockCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ArIsLocked ? accentBlue : Color.Gray;
            ARLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ArIsLocked ? accentBlue : Color.Gray;
            ODLockCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.OdIsLocked ? accentBlue : Color.Gray;
            ODLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.OdIsLocked ? accentBlue : Color.Gray;
        }

        void UpdateScaleButtons(object sender, EventArgs e)
        {
            ScaleARCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleAR ? accentBlue : Color.Gray;
            ScaleARCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleAR ? accentBlue : Color.Gray;
        }

        void ToggleHpCsArOdDisplay(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            foreach (var textbox in diffDisplays)
            {
                textbox.Enabled = enabled ? true : false;
                textbox.BackColor = enabled ? textBoxBg : SystemColors.ControlDark;
                textbox.ForeColor = textBoxFg;
                textbox.Font = new Font(ARDisplay.Font, FontStyle.Regular);
            }
            foreach (var slider in diffSliders)
            {
                slider.Enabled = enabled;
            }
        }
        void UpdateHpCsArOdDisplay(object sender, EventArgs e)
        {
            // HP
            float newHP      = editor.NewBeatmap.HPDrainRate;
            float originalHP = editor.OriginalBeatmap.HPDrainRate;
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
            float newCS      = editor.NewBeatmap.CircleSize;
            float originalCS = editor.OriginalBeatmap.CircleSize;
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
            float newAR      = editor.NewBeatmap.ApproachRate;
            ARDisplay.Text = newAR.ToString();
            ARSlider.Value = (decimal)newAR;
            if (newAR > editor.GetScaledAR())
            {
                ARDisplay.ForeColor = accentRed;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else if (newAR < editor.GetScaledAR())
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
            float newOD      = editor.NewBeatmap.OverallDifficulty;
            float originalOD = editor.OriginalBeatmap.OverallDifficulty;
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

        void ToggleDifficultyDisplay(object sender, EventArgs e)
        {
            if (editor.State == EditorState.NOT_READY)
            {
                StarLabel.ForeColor = labelDisabledColor;
                StarLabel.Text = "☆";
                AimLabel.ForeColor = labelDisabledColor;
                AimLabel.Text = "";
                SpeedLabel.ForeColor = labelDisabledColor;
                SpeedLabel.Text = "";
                AimSpeedBar.LeftColour = labelDisabledColor;
                AimSpeedBar.RightColour = labelDisabledColor;
            }
            else if (editor.State == EditorState.READY)
            {
                StarLabel.ForeColor = starsColor;
                AimLabel.ForeColor = aimColor;
                SpeedLabel.ForeColor = speedColor;
                AimSpeedBar.LeftColour = aimColor;
                AimSpeedBar.RightColour = speedColor;
            }
        }
        void ToggleBpmUpDown(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            BpmMultiplierUpDown.Enabled   = enabled ? true : false;
            BpmMultiplierUpDown.BackColor = enabled ? textBoxBg : SystemColors.ControlDark;
        }
        void UpdateDifficultyDisplay(object sender, EventArgs e)
        {
            StarLabel.Text = $"{editor.starRating:0.00} ☆";
            AimLabel.Text = $"aim: {editor.aimRating}";
            SpeedLabel.Text = $"spd: {editor.speedRating}";
            float aimPercent = 100 * editor.aimRating / (editor.aimRating + editor.speedRating);
            AimSpeedBar.LeftPercent = (int)aimPercent;
        }


        void ToggleGenerateButton(object sender, EventArgs e)
        {
            bool enabled = false;
            switch (editor.State)
            {
                case EditorState.READY:
                    enabled = editor.NewMapIsDifferent() ? true : false;
                    break;
                case EditorState.NOT_READY:
                case EditorState.GENERATING_BEATMAP:
                    enabled = false;
                    break;
            }
            GenerateMapButton.Enabled   = enabled ? true : false;
            GenerateMapButton.ForeColor = enabled ? Color.White : Color.DimGray;
            GenerateMapButton.BackColor = enabled ? accentPink : SystemColors.ControlLight;
            GenerateMapButton.Text      = editor.State == EditorState.GENERATING_BEATMAP ? "Working..." : "2. Create Map";
        }
        



        public void PlayDoneSound()
        {
            sound.SoundLocation = matchConfirmWav;
            sound.Play();
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

        #region Event Handlers
        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
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
            string beatmapFolder = osuReader.GetMapFolderName();
            string beatmapFilename = osuReader.GetOsuFileName();

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

            // signal the editor class to load this beatmap sometime in the future
            editor.RequestBeatmapLoad(absoluteFilename);
        }
        private void HPSlider_ValueChanged(object sender, EventArgs e)            => editor.SetHP((float)HPSlider.Value);
        private void CSSlider_ValueChanged(object sender, EventArgs e)            => editor.SetCS((float)CSSlider.Value);
        private void ARSlider_ValueChanged(object sender, EventArgs e)            => editor.SetAR((float)ARSlider.Value);
        private void ODSlider_ValueChanged(object sender, EventArgs e)            => editor.SetOD((float)ODSlider.Value);
        private void HPLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetHPLock(HPLockCheck.Checked);
        private void CSLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetCSLock(CSLockCheck.Checked);
        private void ARLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetARLock(ARLockCheck.Checked);
        private void ODLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetODLock(ODLockCheck.Checked);
        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e)      => editor.SetScaleAR(!editor.ScaleAR);
        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e) => editor.SetBpmMultiplier((float)BpmMultiplierUpDown.Value);
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                ResetButton_Click(null, null);
                ResetButton.Focus();
            }
            if (e.KeyCode == Keys.D2 && GenerateMapButton.Enabled)
            {
                GenerateMapButton_Click(null, null);
                GenerateMapButton.Focus();
            }
        }
        private void GenerateMapButton_Click(object sender, EventArgs e) => editor.GenerateBeatmap();
        private void ResetButton_Click(object sender, EventArgs e)       => editor.ResetBeatmap();
        #endregion
    }
}
