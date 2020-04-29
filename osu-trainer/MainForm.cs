using FsBeatmapProcessor;
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
        string matchConfirmWav = Path.Combine("resources", "match-confirm.wav");
        string nobgpng = Path.Combine("resources", "nobg.png");
        static Image nobg;

        // Beatmap
        string userSongsFolder = null;
        IOsuMemoryReader osuReader;

        // Color Theme
        Color formBg = Color.FromArgb(38, 35, 53);
        Color textBoxBg = Color.FromArgb(23, 16, 25);
        Color textBoxFg = Color.FromArgb(224, 224, 224);
        Color readOnlyTextBoxBg = Color.FromArgb(36, 24, 38);
        Color readOnlyTextBoxFg = Color.Silver;
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
        readonly SoundPlayer sound = new SoundPlayer();
        readonly BeatmapEditor editor;

        // other
        private string previousBeatmapRead;
        private int beatmapFindFailCounter = 0;
        private bool? gameLoaded = null;

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

            // load user settings
            if (Directory.Exists(Properties.Settings.Default.SongsFolder))
                userSongsFolder = Properties.Settings.Default.SongsFolder;

            // Init object instances
            osuReader = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");
            editor = new BeatmapEditor(this);

            // Add event handlers (observers)
            editor.StateChanged     += ToggleDumbLabels;
            editor.StateChanged     += ToggleGenerateButton;
            editor.StateChanged     += ToggleHpCsArOdDisplay;
            editor.StateChanged     += ToggleDifficultyDisplay;
            editor.StateChanged     += ToggleBpmUpDown;
            editor.StateChanged     += ToggleBpmDisplay;
            editor.BeatmapSwitched  += UpdateSongDisplay;
            editor.BeatmapModified  += UpdateBpmDisplay;
            editor.BeatmapModified  += UpdateHpCsArOdDisplay;
            editor.BeatmapModified  += UpdateDifficultyDisplay;
            editor.BeatmapModified  += ToggleGenerateButton;
            editor.BeatmapModified  += UpdateBpmUpDown;
            editor.ControlsModified += UpdateLockButtons;
            editor.ControlsModified += UpdateScaleButtons;
            editor.ControlsModified += UpdateChanGePitchButton;

            // need controls to show up as initially disabled
            editor.ForceEventStateChanged();
            editor.ForceEventBeatmapSwitched();
            editor.ForceEventControlsModified();

            BeatmapUpdateTimer.Start();
            OsuRunningTimer.Start();
        }

        #region Callbacks for updating GUI controls
        private void UpdateBpmUpDown(object sender, EventArgs e)
        {
            BpmMultiplierUpDown.Value = (decimal)editor.BpmMultiplier;
        }
        private void UpdateSongDisplay(object sender, EventArgs e)
        {
            string artist, title, diff;
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
                        case BadBeatmapReason.EMPTY_MAP:
                            UpdateSongBg(editor.NewBeatmap);

                            SongLabel.ForeColor = accentYellow;
                            DiffLabel.ForeColor = accentRed;
                            StaticGif.Visible = false;
                            DiffLabel.Visible = true;

                            artist = editor.OriginalBeatmap.Artist;
                            title = editor.OriginalBeatmap.Title;
                            SongLabel.Text = TruncateLabelText($"{artist} - {title}", SongLabel);
                            DiffLabel.Text = $"empty map";
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

                    artist = editor.OriginalBeatmap.Artist;
                    title = editor.OriginalBeatmap.Title;
                    diff = editor.OriginalBeatmap.Version;
                    SongLabel.Text = TruncateLabelText($"{artist} - {title}", SongLabel);
                    DiffLabel.Text = TruncateLabelText(diff, DiffLabel);
                    break;
            }
        }
        private void UpdateSongBg(Beatmap map)
        {
            if (map.Background == "")
            {
                // no background for this map
                BgPanel.BackgroundImage = CropAndPanToFit(nobg, BgPanel.Size.Width, BgPanel.Size.Height);
                return;
            }
                
            string imageAbsolutePath = Path.Combine(Path.GetDirectoryName(map.Filename), map.Background);
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
        private void ToggleDumbLabels(object sender, EventArgs e)
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
        private void ToggleBpmDisplay(object sender, EventArgs e)
        {
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    OriginalBpmRangeTextBox.Visible = false;
                    NewBpmRangeTextBox.Visible = false;
                    NewBpmTextBox.BackColor = labelDisabledColor;
                    OriginalBpmTextBox.BackColor = labelDisabledColor;
                    break;
                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    OriginalBpmTextBox.BackColor    = readOnlyTextBoxBg;
                    NewBpmTextBox.BackColor         = textBoxBg;
                    break;
            }

        }
        private void UpdateBpmDisplay(object sender, EventArgs e)
        {
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    break;
                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    (decimal oldbpm, decimal oldmin, decimal oldmax) = editor.GetOriginalBpmData();
                    (decimal newbpm, decimal newmin, decimal newmax) = editor.GetNewBpmData();

                    // bpm
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
        private void UpdateLockButtons(object sender, EventArgs e)
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
        private void UpdateScaleButtons(object sender, EventArgs e)
        {
            ScaleARCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleAR ? accentBlue : Color.Gray;
            ScaleARCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleAR ? accentBlue : Color.Gray;
            ScaleODCheck.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleOD ? accentBlue : Color.Gray;
            ScaleODCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ScaleOD ? accentBlue : Color.Gray;
        }
        private void UpdateChanGePitchButton(object sender, EventArgs e)
        {
            changePitchButton.ForeColor                  = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ChangePitch ? accentBlue : Color.Gray;
            changePitchButton.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ChangePitch ? accentBlue : Color.Gray;
        }
        private void ToggleHpCsArOdDisplay(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            HPSlider.Enabled = enabled;
            HPDisplay.Enabled = enabled ? true : false;
            HPDisplay.BackColor = enabled ? readOnlyTextBoxBg : SystemColors.ControlDark;
            HPDisplay.ForeColor = readOnlyTextBoxFg;
            HPDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            CSSlider.Enabled = enabled;
            CSDisplay.Enabled = enabled ? true : false;
            CSDisplay.BackColor = enabled ? readOnlyTextBoxBg : SystemColors.ControlDark;
            CSDisplay.ForeColor = readOnlyTextBoxFg;
            CSDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            ARSlider.Enabled = enabled;
            ARDisplay.Enabled = enabled ? true : false;
            ARDisplay.BackColor = enabled ? readOnlyTextBoxBg : SystemColors.ControlDark;
            ARDisplay.ForeColor = readOnlyTextBoxFg;
            ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY);
            ODSlider.Enabled = enabled;
            ODDisplay.Enabled = enabled ? true : false;
            ODDisplay.BackColor = enabled ? readOnlyTextBoxBg : SystemColors.ControlDark;
            ODDisplay.ForeColor = readOnlyTextBoxFg;
            ODDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);
        }
        private void UpdateHpCsArOdDisplay(object sender, EventArgs e)
        {
            // HP
            decimal newHP      = editor.NewBeatmap.HPDrainRate;
            decimal originalHP = editor.OriginalBeatmap.HPDrainRate;
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
            decimal newCS      = editor.NewBeatmap.CircleSize;
            decimal originalCS = editor.OriginalBeatmap.CircleSize;
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
            decimal newAR    = editor.NewBeatmap.ApproachRate;
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
            decimal newOD      = editor.NewBeatmap.OverallDifficulty;
            decimal originalOD = editor.OriginalBeatmap.OverallDifficulty;
            ODDisplay.Text = newOD.ToString();
            ODSlider.Value = (decimal)newOD;
            if (newOD > editor.GetScaledOD())
            {
                ODDisplay.ForeColor = accentRed;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else if (newOD < editor.GetScaledOD())
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
        private void ToggleDifficultyDisplay(object sender, EventArgs e)
        {
            if (editor.State == EditorState.NOT_READY)
            {
                StarLabel.ForeColor     = labelDisabledColor;
                StarLabel.Text          = "☆";
                AimLabel.ForeColor      = labelDisabledColor;
                AimLabel.Text           = "";
                SpeedLabel.ForeColor    = labelDisabledColor;
                SpeedLabel.Text         = "";
                AimSpeedBar.LeftColour  = labelDisabledColor;
                AimSpeedBar.RightColour = labelDisabledColor;
            }
            else if (editor.State == EditorState.READY)
            {
                StarLabel.ForeColor     = starsColor;
                AimLabel.ForeColor      = aimColor;
                SpeedLabel.ForeColor    = speedColor;
                AimSpeedBar.LeftColour  = aimColor;
                AimSpeedBar.RightColour = speedColor;
            }
        }
        private void ToggleBpmUpDown(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            BpmMultiplierUpDown.Enabled   = enabled ? true : false;
            BpmMultiplierUpDown.BackColor = enabled ? textBoxBg : SystemColors.ControlDark;
        }
        private void UpdateDifficultyDisplay(object sender, EventArgs e)
        {
            StarLabel.Text = $"{editor.StarRating:0.00} ☆";
            AimLabel.Text = $"aim: {editor.AimRating:0.0}";
            SpeedLabel.Text = $"spd: {editor.SpeedRating:0.0}";
            if (editor.StarRating == 0)
            {
                StarLabel.ForeColor     = labelDisabledColor;
                AimLabel.ForeColor      = labelDisabledColor;
                SpeedLabel.ForeColor    = labelDisabledColor;
                AimSpeedBar.LeftColour  = labelDisabledColor;
                AimSpeedBar.RightColour = labelDisabledColor;
            }
            else
            {
                decimal aimPercent = 100 * editor.AimRating / (editor.AimRating + editor.SpeedRating);
                AimSpeedBar.LeftPercent = (int)aimPercent;
                StarLabel.ForeColor     = starsColor;
                AimLabel.ForeColor      = aimColor;
                SpeedLabel.ForeColor    = speedColor;
                AimSpeedBar.LeftColour  = aimColor;
                AimSpeedBar.RightColour = speedColor;
            }
        }
        private void ToggleGenerateButton(object sender, EventArgs e)
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
            GenerateMapButton.Text      = editor.State == EditorState.GENERATING_BEATMAP ? "Working..." : "3. Create Map";
        }
        #endregion



        #region User input event handlers
        private void HpSlider_ValueChanged(object sender, EventArgs e)            => editor.SetHP((decimal)HPSlider.Value);
        private void CsSlider_ValueChanged(object sender, EventArgs e)            => editor.SetCS((decimal)CSSlider.Value);
        private void ArSlider_ValueChanged(object sender, EventArgs e)            => editor.SetAR((decimal)ARSlider.Value);
        private void OdSlider_ValueChanged(object sender, EventArgs e)            => editor.SetOD((decimal)ODSlider.Value);
        private void HpLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetHPLock(HPLockCheck.Checked);
        private void CsLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetCSLock(CSLockCheck.Checked);
        private void ArLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetARLock(ARLockCheck.Checked);
        private void OdLockCheck_CheckedChanged(object sender, EventArgs e)       => editor.SetODLock(ODLockCheck.Checked);
        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e)      => editor.SetScaleAR(!editor.ScaleAR);
        private void ScaleODCheck_CheckedChanged(object sender, EventArgs e)      => editor.SetScaleOD(!editor.ScaleOD);
        private void ChangePitchButton_CheckedChanged(object sender, EventArgs e) => editor.ToggleChangePitchSetting();
        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e) => editor.SetBpmMultiplier((decimal)BpmMultiplierUpDown.Value);
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (BpmMultiplierUpDown.Focused || NewBpmTextBox.Focused)
                return;
            if (e.KeyCode == Keys.D1)
            {
                ResetButton_Click(null, null);
                ResetButton.Focus();
            }
            if (e.KeyCode == Keys.D2)
            {
                DeleteButton_Click(null, null);
                DeleteButton.Focus();
            }
            if (e.KeyCode == Keys.D3 && GenerateMapButton.Enabled)
            {
                GenerateMapButton_Click(null, null);
                GenerateMapButton.Focus();
            }
        }
        private void NewBpmTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void NewBpmTextBox_Submit()
        {
            int bpm;
            if (int.TryParse(NewBpmTextBox.Text, out bpm))
                editor.SetBpm(bpm);
        }
        private void NewBpmTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                NewBpmTextBox_Submit();
        }
        private void NewBpmTextBox_Leave(object sender, EventArgs e)
        {
            NewBpmTextBox_Submit();
        }
        private void ResetButton_Click(object sender, EventArgs e) => editor.ResetBeatmap();
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var mp3List = editor.GetUnusedMp3s();
            if (new DeleteMp3sForm(mp3List).ShowDialog() == DialogResult.OK)
            {
                mp3List
                    .Select(relativeMp3 => JunUtils.FullPathFromSongsFolder(relativeMp3))
                    .Select(absMp3 => new FileInfo(absMp3))
                    .ToList()
                    .ForEach(file => file.Delete());
                if (mp3List.Count > 0)
                    MessageBox.Show($"Deleted {mp3List.Count} file(s).", "Success");
                editor.CleanUpManifestFile();
            }
        }
        private void GenerateMapButton_Click(object sender, EventArgs e) => editor.GenerateBeatmap();
        private void Unfocus(object sender, EventArgs e) => ActiveControl = Panel3;
        #endregion



        #region Timer events
        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (gameLoaded == false || gameLoaded == null)
                return;


            // this can be cleaned up...
            // Read memory for current map
            string beatmapFolder = osuReader.GetMapFolderName();
            string beatmapFilename = osuReader.GetOsuFileName();

            // Read unsuccessful
            if (beatmapFilename == "")
                return;

            // Beatmap name hasn't changed since last read
            if (previousBeatmapRead != null && previousBeatmapRead == beatmapFilename)
                return;
            previousBeatmapRead = beatmapFilename;

            // Try to locate the beatmap
            string absoluteFilename = Path.Combine(userSongsFolder, beatmapFolder, beatmapFilename);
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
                        absoluteFilename = Path.Combine(userSongsFolder, beatmapFolder, beatmapFilename);
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
        private async void OsuRunningTimer_Tick(object sender, EventArgs e)
        {
            // check if osu!.exe is running
            var processes = Process.GetProcessesByName("osu!");
            if (processes.Length == 0)
                gameLoaded = false;
            else
            {
                if (gameLoaded == false)
                {
                    await Task.Run(() => Thread.Sleep(5000));
                    gameLoaded = true;
                }
                else if (gameLoaded == null)
                    gameLoaded = true;

                if (userSongsFolder == null)
                {
                    // Try to get osu songs folder
                    var osuExePath = processes[0].MainModule.FileName;
                    userSongsFolder = Path.Combine(Path.GetDirectoryName(osuExePath), "Songs");
                    Properties.Settings.Default.SongsFolder = userSongsFolder;
                    Properties.Settings.Default.Save();
                }
            }
        }
        #endregion Timer events



        #region Misc
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
        private Bitmap CropAndPanToFit(Image img, int destinationWidth, int destinationHeight)
        {
            Bitmap bmpImage = new Bitmap(img);
            decimal aspectRatio = destinationWidth / destinationHeight;
            int cropHeight = (int)(bmpImage.Width / aspectRatio);
            // pan down to center image
            int panDown = (bmpImage.Height - cropHeight) / 3;
            // crop and pan via cloning
            return bmpImage.Clone(new Rectangle(0, panDown, bmpImage.Width, cropHeight), bmpImage.PixelFormat);
        }
        #endregion Misc

        #region borderless window title bar
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) => Drag = false;
        private void closeButton_Click(object sender, EventArgs e) => Close();
        private void minimizeButton_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        #endregion

    }
}
