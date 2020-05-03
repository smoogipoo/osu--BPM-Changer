using FsBeatmapProcessor;
using osu_trainer.Controls;
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
        private string nobgpng = Path.Combine("resources", "nobg.png");

        private static Image nobg;

        // Beatmap
        private string userSongsFolder = null;

        private IOsuMemoryReader osuReader;

        // Common Control Lists
        private List<Label> dumbLabels;

        private List<TextBox> diffDisplays;
        private List<OptionSlider> diffSliders;

        // Single Object Instances
        private readonly SoundPlayer sound = new SoundPlayer();

        private readonly BeatmapEditor editor;

        // other
        private string previousBeatmapRead;

        private int beatmapFindFailCounter = 0;
        private bool? gameLoaded = null;

        public MainForm()
        {
            InitializeComponent();
            this.Font = new Font(Program.FontCollection.Families[0], this.Font.Size, FontStyle.Bold, this.Font.Unit);
            DeleteButton.Color = Colors.AccentRed;
            ResetButton.Color = Colors.AccentSalmon;
            ThemeControls(this.Controls);

            nobg = Image.FromFile(nobgpng);

            // init control lists
            dumbLabels = new List<Label>
            {
                hplabel, cslabel, arlabel, odlabel,
                BpmMultiplierLabel, OriginalBpmLabel, NewBpmLabel
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
            editor.StateChanged += ToggleDumbLabels;
            editor.StateChanged += ToggleGenerateButton;
            editor.StateChanged += ToggleHpCsArOdDisplay;
            editor.StateChanged += ToggleDifficultyDisplay;
            editor.StateChanged += ToggleBpmUpDown;
            editor.StateChanged += ToggleBpmDisplay;
            editor.BeatmapSwitched += UpdateSongDisplay;
            editor.BeatmapModified += UpdateBpmDisplay;
            editor.BeatmapModified += UpdateHpCsArOdDisplay;
            editor.BeatmapModified += UpdateDifficultyDisplay;
            editor.BeatmapModified += ToggleGenerateButton;
            editor.BeatmapModified += UpdateBpmUpDown;
            editor.ControlsModified += UpdateLockButtons;
            editor.ControlsModified += UpdateChecks;
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
            BpmMultiplierUpDown.Value = editor.BpmMultiplier;
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
                            //SongLabel.Text = "no beatmap";
                            //SongLabel.ForeColor = Colors.Disabled;
                            //StaticGif.Visible = true;
                            //DiffLabel.Visible = false;
                            break;

                        case BadBeatmapReason.ERROR_LOADING_BEATMAP:
                            //SongLabel.Text = "Unable to read .osu file";
                            //SongLabel.ForeColor = Colors.Disabled;
                            //StaticGif.Visible = true;
                            //DiffLabel.Visible = false;
                            break;

                        case BadBeatmapReason.EMPTY_MAP:
                            UpdateSongBg(editor.NewBeatmap);

                            //StaticGif.Visible = false;

                            SongDisplay.Artist = editor.OriginalBeatmap.Artist;
                            SongDisplay.Title = editor.OriginalBeatmap.Title;
                            SongDisplay.Difficulty = "empty map";
                            break;

                        default:
                            break;
                    }
                    break;

                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    UpdateSongBg(editor.NewBeatmap);

                    //StaticGif.Visible = false;

                    SongDisplay.Artist = editor.OriginalBeatmap.Artist;
                    SongDisplay.Title = editor.OriginalBeatmap.Title;
                    SongDisplay.Difficulty = editor.OriginalBeatmap.Version;
                    break;
            }
        }

        private void UpdateSongBg(Beatmap map)
        {
            // crop height to aspect ratio
            //Image bg =
            SongDisplay.Cover = GetSongBackground(map);
        }

        private Image GetSongBackground(Beatmap beatmap)
        {
            if (string.IsNullOrWhiteSpace(beatmap.Background))
                return nobg;

            string imageAbsolutePath = Path.Combine(Path.GetDirectoryName(beatmap.Filename), beatmap.Background);
            if (!File.Exists(imageAbsolutePath))
                return nobg;

            return Image.FromFile(imageAbsolutePath);
        }

        private void ToggleDumbLabels(object sender, EventArgs e)
        {
            Color labelColor = Colors.AccentSalmon;
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    labelColor = Colors.Disabled;
                    break;

                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    labelColor = Colors.AccentSalmon;
                    break;
            }

            if (editor.State == EditorState.NOT_READY && editor.NotReadyReason == BadBeatmapReason.EMPTY_MAP)
            {
                SongDisplay.ErrorMessage = "No beatmap loaded";
            }
            else
            {
                SongDisplay.ErrorMessage = null;
            }
            SongDisplay.Invalidate(false);

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
                    NewBpmTextBox.BackColor = Colors.Disabled;
                    OriginalBpmTextBox.BackColor = Colors.Disabled;
                    break;

                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    OriginalBpmTextBox.BackColor = Colors.ReadOnlyBg;
                    NewBpmTextBox.BackColor = Colors.TextBoxBg;
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
                    OriginalBpmTextBox.Text = Math.Round(oldbpm).ToString("0");
                    NewBpmTextBox.Text = Math.Round(newbpm).ToString("0");
                    if (newbpm > oldbpm)
                        NewBpmTextBox.ForeColor = Colors.AccentRed;
                    else if (newbpm < oldbpm)
                        NewBpmTextBox.ForeColor = Colors.Easier;
                    else
                        NewBpmTextBox.ForeColor = Colors.TextBoxFg;

                    // bpm range
                    OriginalBpmRangeTextBox.Text = $"({Math.Round(oldmin).ToString("0")} - {Math.Round(oldmax).ToString("0")})";
                    NewBpmRangeTextBox.Text = $"({Math.Round(newmin).ToString("0")} - {Math.Round(newmax).ToString("0")})";
                    OriginalBpmRangeTextBox.Visible = (oldmin != oldmax);
                    NewBpmRangeTextBox.Visible = (oldmin != oldmax);
                    break;
            }
        }

        private void UpdateLockButtons(object sender, EventArgs e)
        {
            HPLockCheck.ForeColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.HpIsLocked ? Colors.AccentBlue : Color.Gray;
            HPLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.HpIsLocked ? Colors.AccentBlue : Color.Gray;

            CSLockCheck.ForeColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.CsIsLocked ? Colors.AccentBlue : Color.Gray;
            CSLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.CsIsLocked ? Colors.AccentBlue : Color.Gray;

            ARLockCheck.ForeColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ArIsLocked ? Colors.AccentBlue : Color.Gray;
            ARLockCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ArIsLocked ? Colors.AccentBlue : Color.Gray;

            var odLock = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.OdIsLocked ? Colors.AccentBlue : Color.Gray;
            ODLockCheck.ForeColor = odLock;
            ODLockCheck.FlatAppearance.BorderColor = odLock;
        }

        private void UpdateChecks(object sender, EventArgs e)
        {
            var enabled = editor.State != EditorState.NOT_READY;
            ChangePitchCheck.Enabled = enabled;
            ScaleODCheck.Enabled = enabled;
            ScaleARCheck.Enabled = enabled;
        }

        private void UpdateChanGePitchButton(object sender, EventArgs e)
        {
            ChangePitchCheck.ForeColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ChangePitch ? Colors.AccentBlue : Color.Gray;
            ChangePitchCheck.FlatAppearance.BorderColor = (editor.State == EditorState.NOT_READY) ? Color.Gray : editor.ChangePitch ? Colors.AccentBlue : Color.Gray;
        }

        private void ToggleHpCsArOdDisplay(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            HPSlider.Enabled = enabled;
            HPDisplay.Enabled = enabled ? true : false;
            HPDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            HPDisplay.ForeColor = Colors.ReadOnlyFg;
            HPDisplay.Font = new Font(this.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            CSSlider.Enabled = enabled;
            CSDisplay.Enabled = enabled ? true : false;
            CSDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            CSDisplay.ForeColor = Colors.ReadOnlyFg;
            CSDisplay.Font = new Font(this.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            ARSlider.Enabled = enabled;
            ARDisplay.Enabled = enabled ? true : false;
            ARDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            ARDisplay.ForeColor = Colors.ReadOnlyFg;
            ARDisplay.Font = new Font(this.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY);
            ODSlider.Enabled = enabled;
            ODDisplay.Enabled = enabled ? true : false;
            ODDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            ODDisplay.ForeColor = Colors.ReadOnlyFg;
            ODDisplay.Font = new Font(this.Font, FontStyle.Regular);
        }

        private void UpdateHpCsArOdDisplay(object sender, EventArgs e)
        {
            // HP
            decimal newHP = editor.NewBeatmap.HPDrainRate;
            decimal originalHP = editor.OriginalBeatmap.HPDrainRate;
            HPDisplay.Text = newHP.ToString();
            HPSlider.Value = (decimal)newHP;
            if (newHP > originalHP)
            {
                HPDisplay.ForeColor = Colors.AccentRed;
                HPDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else if (newHP < originalHP)
            {
                HPDisplay.ForeColor = Colors.Easier;
                HPDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else
            {
                HPDisplay.ForeColor = Colors.TextBoxFg;
                HPDisplay.Font = new Font(this.Font, FontStyle.Regular);
            }

            // CS
            decimal newCS = editor.NewBeatmap.CircleSize;
            decimal originalCS = editor.OriginalBeatmap.CircleSize;
            CSDisplay.Text = newCS.ToString();
            CSSlider.Value = (decimal)newCS;
            if (newCS > originalCS)
            {
                CSDisplay.ForeColor = Colors.AccentRed;
                CSDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else if (newCS < originalCS)
            {
                CSDisplay.ForeColor = Colors.Easier;
                CSDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else
            {
                CSDisplay.ForeColor = Colors.TextBoxFg;
                CSDisplay.Font = new Font(this.Font, FontStyle.Regular);
            }

            // AR
            decimal newAR = editor.NewBeatmap.ApproachRate;
            ARDisplay.Text = newAR.ToString();
            ARSlider.Value = (decimal)newAR;
            if (newAR > editor.GetScaledAR())
            {
                ARDisplay.ForeColor = Colors.AccentRed;
                ARDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else if (newAR < editor.GetScaledAR())
            {
                ARDisplay.ForeColor = Colors.Easier;
                ARDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else
            {
                ARDisplay.ForeColor = Colors.TextBoxFg;
                ARDisplay.Font = new Font(this.Font, FontStyle.Regular);
            }

            // OD
            decimal newOD = editor.NewBeatmap.OverallDifficulty;
            decimal originalOD = editor.OriginalBeatmap.OverallDifficulty;
            ODDisplay.Text = newOD.ToString();
            ODSlider.Value = (decimal)newOD;
            if (newOD > editor.GetScaledOD())
            {
                ODDisplay.ForeColor = Colors.AccentRed;
                ODDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else if (newOD < editor.GetScaledOD())
            {
                ODDisplay.ForeColor = Colors.Easier;
                ODDisplay.Font = new Font(this.Font, FontStyle.Bold);
            }
            else
            {
                ODDisplay.ForeColor = Colors.TextBoxFg;
                ODDisplay.Font = new Font(this.Font, FontStyle.Regular);
            }
        }

        private void ToggleDifficultyDisplay(object sender, EventArgs e)
        {
            if (editor.State == EditorState.NOT_READY)
            {
                AimSpeedBar.LeftColor = Colors.Disabled;
                AimSpeedBar.RightColor = Colors.Disabled;
            }
            else if (editor.State == EditorState.READY)
            {
                AimSpeedBar.RightColor = Colors.Aim;
                AimSpeedBar.RightColor = Colors.Speed;
            }
        }

        private void ToggleBpmUpDown(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            BpmMultiplierUpDown.Enabled = enabled ? true : false;
            BpmMultiplierUpDown.BackColor = enabled ? Colors.TextBoxBg : SystemColors.ControlDark;
        }

        private void UpdateDifficultyDisplay(object sender, EventArgs e)
        {
            StarsDisplay.Stars = (float)editor.StarRating;

            var mode = editor.GetMode();
            if (mode.HasValue)
                StarsDisplay.GameMode = mode.Value;

            AimSpeedBar.LeftText = $"aim: {editor.AimRating:0.0}";
            AimSpeedBar.RightText = $"spd: {editor.SpeedRating:0.0}";

            bool enableSpeed = editor.StarRating != 0;
            bool enableAim = (editor.AimRating != 0) && enableSpeed;

            AimSpeedBar.LeftColor = enableAim ? Colors.Aim : Colors.Disabled;
            AimSpeedBar.RightColor = enableSpeed ? Colors.Speed : Colors.Disabled;

            if (enableSpeed)
            {
                decimal aimPercent = 100 * editor.AimRating / (editor.AimRating + editor.SpeedRating);
                AimSpeedBar.LeftPercent = (int)aimPercent;
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
            GenerateMapButton.Enabled = enabled;
            GenerateMapButton.ForeColor = enabled ? Color.White : Color.DimGray;
            GenerateMapButton.Color = GenerateMapButton.Enabled ? Colors.AccentPink : Colors.TextBoxBg;
            GenerateMapButton.Text = editor.State == EditorState.GENERATING_BEATMAP ? "Working..." : "Create Map";
        }

        #endregion Callbacks for updating GUI controls

        #region User input event handlers

        private void HpSlider_ValueChanged(object sender, EventArgs e) => editor.SetHP((decimal)HPSlider.Value);

        private void CsSlider_ValueChanged(object sender, EventArgs e) => editor.SetCS((decimal)CSSlider.Value);

        private void ArSlider_ValueChanged(object sender, EventArgs e) => editor.SetAR((decimal)ARSlider.Value);

        private void OdSlider_ValueChanged(object sender, EventArgs e) => editor.SetOD((decimal)ODSlider.Value);

        private void HpLockCheck_CheckedChanged(object sender, EventArgs e) => editor.SetHPLock(HPLockCheck.Checked);

        private void CsLockCheck_CheckedChanged(object sender, EventArgs e) => editor.SetCSLock(CSLockCheck.Checked);

        private void ArLockCheck_CheckedChanged(object sender, EventArgs e) => editor.SetARLock(ARLockCheck.Checked);

        private void OdLockCheck_CheckedChanged(object sender, EventArgs e) => editor.SetODLock(ODLockCheck.Checked);

        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e) => editor.SetScaleAR(!editor.ScaleAR);

        private void ScaleODCheck_CheckedChanged(object sender, EventArgs e) => editor.SetScaleOD(!editor.ScaleOD);

        private void ChangePitchButton_CheckedChanged(object sender, EventArgs e) => editor.ToggleChangePitchSetting();

        private void BpmMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (BpmMultiplierUpDown.Value != editor.BpmMultiplier)
                editor.SetBpmMultiplier(BpmMultiplierUpDown.Value);
        }

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

        private void GenerateMapButton_Click(object sender, EventArgs e) => BackgroundWorker.RunWorkerAsync();

        private void Unfocus(object sender, EventArgs e) => ActiveControl = Panel3;

        #endregion User input event handlers

        #region Timer events

        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (gameLoaded == false || gameLoaded == null)
                return;

            // this can be cleaned up...
            // Read memory for current map
            string beatmapFolder = osuReader.GetMapFolderName();
            string beatmapFilename = osuReader.GetOsuFileName();

            var invalidChars = Path.GetInvalidFileNameChars();

            // Read unsuccessful
            if (string.IsNullOrWhiteSpace(beatmapFilename) || beatmapFilename.Any(c => invalidChars.Contains(c)))
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
            sound.Stream = Properties.Resources.match_confirm;
            sound.Play();
        }

        private string TruncateLabelText(string txt, Label label)
        {
            if (TextRenderer.MeasureText(txt, label.Font).Width > label.Width)
            {
                string truncated = txt;
                while (TextRenderer.MeasureText(truncated, label.Font).Width > label.Width - 10)
                    truncated = truncated.Substring(0, truncated.Length - 1);
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

        #endregion borderless window title bar

        #region Background Worker

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GenerateMapButton.Progress = e.ProgressPercentage / 100f;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            editor.GenerateBeatmap();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"Your map couldn't be generated because of the following error:\n{e.Error.ToString()}", "Generating Map failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GenerateMapButton.Progress = 0f;
                PlayDoneSound();
            }
        }

        #endregion Background Worker

        private void ThemeControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control == closeButton || control == minimizeButton)
                    continue;

                ThemeControls(control.Controls);

                if (control == StarsDisplay)
                {
                    control.Font = new Font(this.Font.FontFamily, 12, FontStyle.Bold, GraphicsUnit.Point);
                }
                else
                {
                    control.Font = this.Font;
                }
            }
        }

        private void titlePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            e.Graphics.DrawIcon(Icon, new Rectangle(10, 8, 16, 16));
            e.Graphics.DrawString(Text, Font, Brushes.White, 10 + 16 + 4, 10);
        }
    }
}