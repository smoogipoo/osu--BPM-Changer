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
using Utilities;

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
        private List<OsuCheckBox> checkControls;

        // Single Object Instances
        private readonly SoundPlayer sound = new SoundPlayer();
        private readonly BeatmapEditor editor;
        private readonly globalKeyboardHook kbhook = new globalKeyboardHook();

        // other
        private string previousBeatmapRead;

        private int beatmapFindFailCounter = 0;
        private bool? gameLoaded = null;

        #region DEBUG
        private void Form1_Load(object sender, EventArgs e)
        {
            //AllocConsole();
        }

        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();
        #endregion

        public MainForm()
        {
            InitializeComponent();

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
            checkControls = new List<OsuCheckBox>
            {
                NoSpinnersCheck, HRCheck, ScaleARCheck, ScaleODCheck, ChangePitchCheck
            };

            ApplyFonts();

            // load user settings
            if (Directory.Exists(Properties.Settings.Default.SongsFolder))
                userSongsFolder = Properties.Settings.Default.SongsFolder;

            // Init object instances
            osuReader = OsuMemoryReader.Instance.GetInstanceForWindowTitleHint("");
            editor = new BeatmapEditor(this);

            // Add event handlers (observers)
            editor.StateChanged += ToggleDumbLabels;
            editor.StateChanged += TogglePrettyButtons;
            editor.StateChanged += ToggleHpCsArOdDisplay;
            editor.StateChanged += ToggleDifficultyDisplay;
            editor.StateChanged += ToggleBpmInputControls;
            editor.StateChanged += ToggleBpmDisplay;
            editor.StateChanged += ToggleLockButtons;
            editor.BeatmapSwitched += UpdateSongDisplay;
            editor.BeatmapModified += UpdateBpmDisplay;
            editor.BeatmapModified += UpdateHpCsArOdDisplay;
            editor.BeatmapModified += UpdateDifficultyDisplay;
            editor.BeatmapModified += TogglePrettyButtons;
            editor.BeatmapModified += UpdateRateInputControls;
            editor.ControlsModified += UpdateLockButtons;
            editor.ControlsModified += UpdateChecks;

            // need controls to show up as initially disabled
            editor.ForceEventStateChanged();
            editor.ForceEventBeatmapSwitched();
            editor.ForceEventControlsModified();

            // Install keyboard hooks
            // (note! this is only for the create map hotkey!!)
            kbhook.HookedKeys.Add(Keys.X);
            kbhook.KeyDown += new KeyEventHandler(CreateMapHotkeyHandler);

            BeatmapUpdateTimer.Start();
            OsuRunningTimer.Start();

            this.Focus();
        }

        private void ApplyFonts()
        {
            var comforta = Program.FontCollection.Families[0];

            foreach (var textbox in diffDisplays)
                textbox.Font = new Font(comforta, 11, FontStyle.Regular);
            foreach (var label in dumbLabels)
                label.Font = new Font(comforta, 10, FontStyle.Regular);
            foreach (var check in checkControls)
                check.Font = new Font(comforta, 9, FontStyle.Regular);

            StarsDisplay.Font = new Font(comforta, 16, FontStyle.Bold);
            AimLabel.Font = new Font(comforta, 9, FontStyle.Regular);
            SpeedLabel.Font = new Font(comforta, 9, FontStyle.Regular);

            BpmMultiplierTextBox.Font = new Font(comforta, 11, FontStyle.Bold);
            OriginalBpmTextBox.Font = new Font(comforta, 10, FontStyle.Regular);
            NewBpmTextBox.Font = new Font(comforta, 10, FontStyle.Regular);

            OriginalBpmRangeTextBox.Font = new Font(comforta, 9, FontStyle.Regular);
            NewBpmRangeTextBox.Font = new Font(comforta, 9, FontStyle.Regular);

        }

        #region Callbacks for updating GUI controls

        private void UpdateRateInputControls(object sender, EventArgs e)
        {
            BpmMultiplierTextBox.Text = editor.BpmMultiplier.ToString("0.00");
        }

        private void UpdateSongDisplay(object sender, EventArgs e)
        {
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    switch (editor.NotReadyReason)
                    {
                        case BadBeatmapReason.NO_BEATMAP_LOADED:
                            break;

                        case BadBeatmapReason.ERROR_LOADING_BEATMAP:
                            SongDisplay.Artist = "failed to load beatmap";
                            SongDisplay.Title = "";
                            SongDisplay.Difficulty = "";
                            SongDisplay.Cover = null;
                            break;

                        case BadBeatmapReason.EMPTY_MAP:
                            UpdateSongBg(editor.NewBeatmap);

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

                    SongDisplay.Artist = editor.OriginalBeatmap.Artist;
                    SongDisplay.Title = editor.OriginalBeatmap.Title;
                    SongDisplay.Difficulty = editor.OriginalBeatmap.Version;
                    break;
            }
        }

        private void UpdateSongBg(Beatmap map)
        {
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
            Color labelColor = Colors.PaleBlue;
            switch (editor.State)
            {
                case EditorState.NOT_READY:
                    labelColor = Colors.Disabled;
                    break;

                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    labelColor = Colors.PaleBlue;
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
                    NewBpmTextBox.Enabled = false;
                    NewBpmTextBox.BackColor = Colors.Disabled;
                    OriginalBpmTextBox.Enabled = false;
                    OriginalBpmTextBox.BackColor = Colors.Disabled;
                    break;

                case EditorState.READY:
                case EditorState.GENERATING_BEATMAP:
                    OriginalBpmTextBox.Enabled = true;
                    OriginalBpmTextBox.BackColor = Colors.ReadOnlyBg;
                    NewBpmTextBox.Enabled = true;
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
                    decimal newbpm, newmin, newmax;
                    if (Math.Abs(editor.BpmMultiplier - 1.0M) > 0.001M)
                        (newbpm, newmin, newmax) = editor.GetNewBpmData();
                    else
                        (newbpm, newmin, newmax) = (oldbpm, oldmin, oldmax);

                    // bpm textboxes
                    OriginalBpmTextBox.Text = Math.Round(oldbpm).ToString("0");
                    NewBpmTextBox.Text = Math.Round(newbpm).ToString("0");
                    if (newbpm > oldbpm + 0.001M)
                        NewBpmTextBox.ForeColor = Colors.AccentRed;
                    else if (newbpm < oldbpm - 0.001M)
                        NewBpmTextBox.ForeColor = Colors.Easier;
                    else
                        NewBpmTextBox.ForeColor = Colors.TextBoxFg;

                    // bpm range
                    OriginalBpmRangeTextBox.Text = $"({Math.Round(oldmin).ToString("0")} - {Math.Round(oldmax).ToString("0")})";
                    NewBpmRangeTextBox.Text = $"({Math.Round(newmin).ToString("0")} - {Math.Round(newmax).ToString("0")})";
                    OriginalBpmRangeTextBox.Visible = (oldmin != oldmax);
                    NewBpmRangeTextBox.Visible = (oldmin != oldmax);

                    // bpm slider
                    BpmSlider.Value = editor.BpmMultiplier;

                    break;
            }
        }

        private void ToggleLockButtons(object sender, EventArgs e)
        {
            bool not_ready = (editor.State == EditorState.NOT_READY);
            HPLockCheck.Enabled = not_ready ? false : true;
            CSLockCheck.Enabled = not_ready ? false : true;
            ARLockCheck.Enabled = not_ready ? false : true;
            ODLockCheck.Enabled = not_ready ? false : true;
            BpmLockCheck.Enabled = not_ready ? false : true;
        }
        private void UpdateLockButtons(object sender, EventArgs e)
        {
            // change checked state without raising any events
            HPLockCheck.CheckedChanged -= HpLockCheck_CheckedChanged;
            CSLockCheck.CheckedChanged -= CsLockCheck_CheckedChanged;
            ARLockCheck.CheckedChanged -= ArLockCheck_CheckedChanged;
            ODLockCheck.CheckedChanged -= OdLockCheck_CheckedChanged;
            BpmLockCheck.CheckedChanged -= BpmLockCheck_CheckedChanged;

            HPLockCheck.Checked = editor.HpIsLocked;
            CSLockCheck.Checked = editor.CsIsLocked;
            ARLockCheck.Checked = editor.ArIsLocked;
            ODLockCheck.Checked = editor.OdIsLocked;
            BpmLockCheck.Checked = editor.BpmIsLocked;

            HPLockCheck.CheckedChanged += HpLockCheck_CheckedChanged;
            CSLockCheck.CheckedChanged += CsLockCheck_CheckedChanged;
            ARLockCheck.CheckedChanged += ArLockCheck_CheckedChanged;
            ODLockCheck.CheckedChanged += OdLockCheck_CheckedChanged;
            BpmLockCheck.CheckedChanged += BpmLockCheck_CheckedChanged;
        }

        private void UpdateChecks(object sender, EventArgs e)
        {
            var enabled = editor.State != EditorState.NOT_READY;
            NoSpinnersCheck.Enabled = enabled;
            HRCheck.Enabled = enabled;
            ChangePitchCheck.Enabled = enabled;
            ScaleODCheck.Enabled = enabled;
            ScaleARCheck.Enabled = enabled;
            NoSpinnersCheck.ForeColor = enabled ? Colors.PaleBlue : Colors.Disabled;
            HRCheck.ForeColor = enabled ? Colors.PaleBlue : Colors.Disabled;
            ChangePitchCheck.ForeColor = enabled ? Colors.PaleBlue : Colors.Disabled;
            ScaleODCheck.ForeColor = enabled ? Colors.PaleBlue : Colors.Disabled;
            ScaleARCheck.ForeColor = enabled ? Colors.PaleBlue : Colors.Disabled;

            // change checked state without raising any events
            NoSpinnersCheck.CheckedChanged  -= NoSpinnerCheckBox_CheckedChanged;
            HRCheck.CheckedChanged          -= HRCheck_CheckedChanged;
            ChangePitchCheck.CheckedChanged -= ChangePitchButton_CheckedChanged;
            ScaleODCheck.CheckedChanged     -= ScaleODCheck_CheckedChanged;
            ScaleARCheck.CheckedChanged     -= ScaleARCheck_CheckedChanged;
            
            NoSpinnersCheck.Checked         = editor.NoSpinners;
            HRCheck.Checked                 = editor.EmulateHardrock;
            ChangePitchCheck.Checked        = editor.ChangePitch;
            ScaleODCheck.Checked            = editor.ScaleOD;
            ScaleARCheck.Checked            = editor.ScaleAR;

            NoSpinnersCheck.CheckedChanged  += NoSpinnerCheckBox_CheckedChanged;
            HRCheck.CheckedChanged          += HRCheck_CheckedChanged;
            ChangePitchCheck.CheckedChanged += ChangePitchButton_CheckedChanged;
            ScaleODCheck.CheckedChanged     += ScaleODCheck_CheckedChanged;
            ScaleARCheck.CheckedChanged     += ScaleARCheck_CheckedChanged;
        }

        private void ToggleHpCsArOdDisplay(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            HPSlider.Enabled = enabled;
            HPDisplay.Enabled = enabled ? true : false;
            HPDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            HPDisplay.ForeColor = Colors.ReadOnlyFg;
            HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            CSSlider.Enabled = enabled;
            CSDisplay.Enabled = enabled ? true : false;
            CSDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            CSDisplay.ForeColor = Colors.ReadOnlyFg;
            CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY) && (editor.GetMode() == GameMode.osu || editor.GetMode() == GameMode.CatchtheBeat);
            ARSlider.Enabled = enabled;
            ARDisplay.Enabled = enabled ? true : false;
            ARDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            ARDisplay.ForeColor = Colors.ReadOnlyFg;
            ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);

            enabled = (editor.State != EditorState.NOT_READY);
            ODSlider.Enabled = enabled;
            ODDisplay.Enabled = enabled ? true : false;
            ODDisplay.BackColor = enabled ? Colors.ReadOnlyBg : SystemColors.ControlDark;
            ODDisplay.ForeColor = Colors.ReadOnlyFg;
            ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Regular);
        }

        private void UpdateHpCsArOdDisplay(object sender, EventArgs e)
        {
            // HP
            decimal newHP = editor.NewBeatmap.HPDrainRate;
            decimal originalHP = editor.OriginalBeatmap.HPDrainRate;
            HPDisplay.Text = newHP.ToString("0.#");
            HPSlider.Value = (decimal)newHP;
            if (newHP > originalHP)
            {
                HPDisplay.ForeColor = Colors.AccentRed;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Bold);
            }
            else if (newHP < originalHP)
            {
                HPDisplay.ForeColor = Colors.Easier;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Bold);
            }
            else
            {
                HPDisplay.ForeColor = Colors.TextBoxFg;
                HPDisplay.Font = new Font(HPDisplay.Font, FontStyle.Regular);
            }

            // CS
            decimal newCS = editor.NewBeatmap.CircleSize;
            decimal originalCS = editor.OriginalBeatmap.CircleSize;
            CSDisplay.Text = newCS.ToString("0.#");
            CSSlider.Value = (decimal)newCS;
            if (newCS > originalCS)
            {
                CSDisplay.ForeColor = Colors.AccentRed;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Bold);
            }
            else if (newCS < originalCS)
            {
                CSDisplay.ForeColor = Colors.Easier;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Bold);
            }
            else
            {
                CSDisplay.ForeColor = Colors.TextBoxFg;
                CSDisplay.Font = new Font(CSDisplay.Font, FontStyle.Regular);
            }

            // AR
            decimal newAR = editor.NewBeatmap.ApproachRate;
            ARDisplay.Text = newAR.ToString("0.#");
            ARSlider.Value = (decimal)newAR;
            if (newAR > editor.GetScaledAR())
            {
                ARDisplay.ForeColor = Colors.AccentRed;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else if (newAR < editor.GetScaledAR())
            {
                ARDisplay.ForeColor = Colors.Easier;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
            else
            {
                ARDisplay.ForeColor = Colors.TextBoxFg;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Regular);
            }
            if (newAR > 10)
            {
                ARDisplay.ForeColor = Color.Magenta;
                ARDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }

            // OD
            decimal newOD = editor.NewBeatmap.OverallDifficulty;
            decimal originalOD = editor.OriginalBeatmap.OverallDifficulty;
            ODDisplay.Text = newOD.ToString("0.#");
            ODSlider.Value = (decimal)newOD;
            if (newOD > editor.GetScaledOD())
            {
                ODDisplay.ForeColor = Colors.AccentRed;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else if (newOD < editor.GetScaledOD())
            {
                ODDisplay.ForeColor = Colors.Easier;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Bold);
            }
            else
            {
                ODDisplay.ForeColor = Colors.TextBoxFg;
                ODDisplay.Font = new Font(ODDisplay.Font, FontStyle.Regular);
            }
            if (newOD > 10)
            {
                ODDisplay.ForeColor = Color.Magenta;
                ODDisplay.Font = new Font(ARDisplay.Font, FontStyle.Bold);
            }
        }

        private void ToggleDifficultyDisplay(object sender, EventArgs e)
        {
            if (editor.State == EditorState.NOT_READY)
            {
                StarsDisplay.Enabled = false;
                AimLabel.ForeColor = Colors.Disabled;
                SpeedLabel.ForeColor = Colors.Disabled;
            }
            else if (editor.State == EditorState.READY)
            {
                StarsDisplay.Enabled = true;
                AimLabel.ForeColor = Colors.PaleBlue;
                SpeedLabel.ForeColor = Colors.PaleBlue;
            }
        }

        private void ToggleBpmInputControls(object sender, EventArgs e)
        {
            bool enabled = (editor.State != EditorState.NOT_READY);
            BpmSlider.Enabled = enabled ? true : false;
            BpmMultiplierTextBox.Enabled = enabled ? true : false;
            BpmMultiplierTextBox.BackColor = enabled ? Colors.TextBoxBg : SystemColors.ControlDark;
        }

        private void UpdateDifficultyDisplay(object sender, EventArgs e)
        {
            StarsDisplay.Stars = (float)editor.StarRating;

            var mode = editor.GetMode();
            if (mode.HasValue)
                StarsDisplay.GameMode = mode.Value;

            AimLabel.Text = $"{editor.AimRating:0.0} aim";
            SpeedLabel.Text = $"{editor.SpeedRating:0.0} speed";
        }

        private void TogglePrettyButtons(object sender, EventArgs e)
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
            GenerateMapButton.ForeColor = enabled ? Color.White : Colors.Disabled;
            GenerateMapButton.Color = enabled ? Colors.AccentPink2 : Colors.TextBoxBg;
            GenerateMapButton.Text = editor.State == EditorState.GENERATING_BEATMAP ? "Working..." : "Create Map";

            ResetButton.Enabled = enabled;
            ResetButton.ForeColor = enabled ? Color.White : Colors.Disabled;
            ResetButton.Color = enabled ? Color.SteelBlue : Colors.TextBoxBg;
        }

        #endregion Callbacks for updating GUI controls

        #region User input event handlers

        private void HpSlider_ValueChanged(object sender, EventArgs e) => editor.SetHP(HPSlider.Value);

        private void CsSlider_ValueChanged(object sender, EventArgs e) => editor.SetCS(CSSlider.Value);

        private void ArSlider_ValueChanged(object sender, EventArgs e) => editor.SetAR(ARSlider.Value);

        private void OdSlider_ValueChanged(object sender, EventArgs e) => editor.SetOD(ODSlider.Value);

        private void HpLockCheck_CheckedChanged(object sender, EventArgs e) => editor.ToggleHpLock();

        private void CsLockCheck_CheckedChanged(object sender, EventArgs e) => editor.ToggleCsLock();

        private void ArLockCheck_CheckedChanged(object sender, EventArgs e) => editor.ToggleArLock();

        private void OdLockCheck_CheckedChanged(object sender, EventArgs e) => editor.ToggleOdLock();

        private void BpmLockCheck_CheckedChanged(object sender, EventArgs e) => editor.ToggleBpmLock();

        private void ScaleODCheck_CheckedChanged(object sender, EventArgs e) => editor.SetScaleOD(!editor.ScaleOD);
        private void ScaleARCheck_CheckedChanged(object sender, EventArgs e) => editor.SetScaleAR(!editor.ScaleAR);

        private void ChangePitchButton_CheckedChanged(object sender, EventArgs e) => editor.ToggleChangePitchSetting();

        private void BpmMultiplierTextBox_Submit(object sender, EventArgs e)
        {
            decimal mult;
            if (Decimal.TryParse(BpmMultiplierTextBox.Text, out mult))
                editor.SetBpmMultiplier(mult);
            else
                BpmMultiplierTextBox.Text = editor.BpmMultiplier.ToString("0.00");

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
            if (gameLoaded == true)
            {
                MessageBox.Show("Please close osu! first then try again.", "osu! is running");
                return;
            }
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

        private void GenerateMapButton_Click(object sender, EventArgs e)
        {
            if ( !Properties.Settings.Default.HighARODMessageShown && (editor.NewBeatmap.ApproachRate > 10M || editor.NewBeatmap.ApproachRate > 10M) )
            {
                MessageBox.Show("You have chosen an AR or OD greater than 10. After this map gets created, make sure to play it with Doubletime.", "Note");
                Properties.Settings.Default.HighARODMessageShown = true;
                Properties.Settings.Default.Save();
            }
            BackgroundWorker.RunWorkerAsync();
        }

        private void Unfocus(object sender, EventArgs e) => ActiveControl = Panel3;

        private void CreateMapHotkeyHandler(object sender, EventArgs e)
        {
            var k = (KeyEventArgs)e;
            if (k.Control && k.Shift)
            {
                if (GenerateMapButton.Enabled)
                {
                    sound.Stream = Properties.Resources.hotkey;
                    sound.Play();
                    GenerateMapButton_Click(sender, EventArgs.Empty);
                }
            }
        }

        #endregion User input event handlers

        #region Timer events

        private void BeatmapUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (gameLoaded == false || gameLoaded == null || editor.State == EditorState.GENERATING_BEATMAP)
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
                if (++beatmapFindFailCounter == 7)
                {
                    string msg = "Automatic beatmap detection failed 7 times in a row. ";
                    msg += "Your songs folder is probably not in your osu! install folder. ";
                    msg += "Please manually locate your Songs folder in the following window.";
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
                if (gameLoaded == false) // @ posedge gameLoaded
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
            //GenerateMapButton.Progress = e.ProgressPercentage / 100f;
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
                //GenerateMapButton.Progress = 0f;
            }
        }

        #endregion Background Worker

        private void titlePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Icon smallIcon = new Icon(this.Icon, 16, 16);
            e.Graphics.DrawIcon(smallIcon, 10, 10);
            e.Graphics.DrawString(Text, new Font(Font, FontStyle.Regular), Brushes.White, 10 + 16 + 4, 10);
        }

        private void NoSpinnerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            editor.ToggleNoSpinners();
        }

        private void BpmMultiplierTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BpmMultiplierTextBox_Submit(sender, e);
                e.Handled = e.SuppressKeyPress = true; // silence annoying windows bell
            }
        }

        private void BpmSlider_ValueChanged(object sender, EventArgs e)
        {
            editor.SetBpmMultiplier(BpmSlider.Value);
        }

        private void HRCheck_CheckedChanged(object sender, EventArgs e)
        {
            editor.ToggleHrEmulation();
        }
    }
}