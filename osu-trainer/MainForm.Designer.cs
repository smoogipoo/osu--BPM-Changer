using osu_trainer.Controls;

namespace osu_trainer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.Panel3 = new System.Windows.Forms.Panel();
            this.BpmMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.NewBpmTextBox = new System.Windows.Forms.TextBox();
            this.NewBpmRangeTextBox = new System.Windows.Forms.TextBox();
            this.OriginalBpmRangeTextBox = new System.Windows.Forms.TextBox();
            this.OriginalBpmTextBox = new System.Windows.Forms.TextBox();
            this.Middle1Panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ODDisplay = new System.Windows.Forms.TextBox();
            this.ARDisplay = new System.Windows.Forms.TextBox();
            this.CSDisplay = new System.Windows.Forms.TextBox();
            this.HPDisplay = new System.Windows.Forms.TextBox();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.OsuRunningTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.AimLabel = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.closeButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.BpmLockCheck = new osu_trainer.Controls.ToggleIconButton();
            this.BpmSlider = new osu_trainer.OptionSlider();
            this.ChangePitchCheck = new osu_trainer.Controls.OsuCheckBox();
            this.NoSpinnersCheck = new osu_trainer.Controls.OsuCheckBox();
            this.HRCheck = new osu_trainer.Controls.OsuCheckBox();
            this.ScaleODCheck = new osu_trainer.Controls.OsuCheckBox();
            this.ScaleARCheck = new osu_trainer.Controls.OsuCheckBox();
            this.OriginalBpmLabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.NewBpmLabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.BpmMultiplierLabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.ODLockCheck = new osu_trainer.Controls.ToggleIconButton();
            this.ARLockCheck = new osu_trainer.Controls.ToggleIconButton();
            this.CSLockCheck = new osu_trainer.Controls.ToggleIconButton();
            this.odlabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.ODSlider = new osu_trainer.OptionSlider();
            this.arlabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.ARSlider = new osu_trainer.OptionSlider();
            this.cslabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.CSSlider = new osu_trainer.OptionSlider();
            this.hplabel = new osu_trainer.Controls.AntiAliasedLabel();
            this.HPSlider = new osu_trainer.OptionSlider();
            this.HPLockCheck = new osu_trainer.Controls.ToggleIconButton();
            this.StarsDisplay = new osu_trainer.Controls.StarsDisplay();
            this.DeleteButton = new osu_trainer.Controls.OsuButton();
            this.ResetButton = new osu_trainer.Controls.OsuButton();
            this.GenerateMapButton = new osu_trainer.Controls.OsuButton();
            this.SongDisplay = new osu_trainer.Controls.SongDisplay();
            this.Panel3.SuspendLayout();
            this.Middle1Panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 20;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.BpmLockCheck);
            this.Panel3.Controls.Add(this.BpmSlider);
            this.Panel3.Controls.Add(this.ChangePitchCheck);
            this.Panel3.Controls.Add(this.NoSpinnersCheck);
            this.Panel3.Controls.Add(this.HRCheck);
            this.Panel3.Controls.Add(this.ScaleODCheck);
            this.Panel3.Controls.Add(this.ScaleARCheck);
            this.Panel3.Controls.Add(this.BpmMultiplierTextBox);
            this.Panel3.Controls.Add(this.NewBpmTextBox);
            this.Panel3.Controls.Add(this.NewBpmRangeTextBox);
            this.Panel3.Controls.Add(this.OriginalBpmRangeTextBox);
            this.Panel3.Controls.Add(this.OriginalBpmTextBox);
            this.Panel3.Controls.Add(this.OriginalBpmLabel);
            this.Panel3.Controls.Add(this.NewBpmLabel);
            this.Panel3.Controls.Add(this.BpmMultiplierLabel);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 303);
            this.Panel3.Name = "Panel3";
            this.Panel3.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.Panel3.Size = new System.Drawing.Size(427, 128);
            this.Panel3.TabIndex = 7;
            this.Panel3.Click += new System.EventHandler(this.Unfocus);
            // 
            // BpmMultiplierTextBox
            // 
            this.BpmMultiplierTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(16)))), ((int)(((byte)(25)))));
            this.BpmMultiplierTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BpmMultiplierTextBox.Font = new System.Drawing.Font("Microsoft Tai Le", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BpmMultiplierTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BpmMultiplierTextBox.Location = new System.Drawing.Point(90, 5);
            this.BpmMultiplierTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.BpmMultiplierTextBox.Name = "BpmMultiplierTextBox";
            this.BpmMultiplierTextBox.Size = new System.Drawing.Size(42, 20);
            this.BpmMultiplierTextBox.TabIndex = 10;
            this.BpmMultiplierTextBox.Text = "1.00";
            this.BpmMultiplierTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BpmMultiplierTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BpmMultiplierTextBox_KeyDown);
            this.BpmMultiplierTextBox.Leave += new System.EventHandler(this.BpmMultiplierTextBox_Submit);
            // 
            // NewBpmTextBox
            // 
            this.NewBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(16)))), ((int)(((byte)(25)))));
            this.NewBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NewBpmTextBox.Location = new System.Drawing.Point(89, 64);
            this.NewBpmTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.NewBpmTextBox.Name = "NewBpmTextBox";
            this.NewBpmTextBox.Size = new System.Drawing.Size(42, 17);
            this.NewBpmTextBox.TabIndex = 10;
            this.NewBpmTextBox.Text = "200";
            this.NewBpmTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NewBpmTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewBpmTextBox_KeyDown);
            this.NewBpmTextBox.Leave += new System.EventHandler(this.NewBpmTextBox_Leave);
            // 
            // NewBpmRangeTextBox
            // 
            this.NewBpmRangeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.NewBpmRangeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewBpmRangeTextBox.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewBpmRangeTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.NewBpmRangeTextBox.Location = new System.Drawing.Point(163, 64);
            this.NewBpmRangeTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.NewBpmRangeTextBox.Name = "NewBpmRangeTextBox";
            this.NewBpmRangeTextBox.ReadOnly = true;
            this.NewBpmRangeTextBox.Size = new System.Drawing.Size(85, 17);
            this.NewBpmRangeTextBox.TabIndex = 10;
            this.NewBpmRangeTextBox.Text = "(180 - 210)";
            // 
            // OriginalBpmRangeTextBox
            // 
            this.OriginalBpmRangeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.OriginalBpmRangeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalBpmRangeTextBox.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalBpmRangeTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.OriginalBpmRangeTextBox.Location = new System.Drawing.Point(163, 35);
            this.OriginalBpmRangeTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.OriginalBpmRangeTextBox.Name = "OriginalBpmRangeTextBox";
            this.OriginalBpmRangeTextBox.ReadOnly = true;
            this.OriginalBpmRangeTextBox.Size = new System.Drawing.Size(85, 17);
            this.OriginalBpmRangeTextBox.TabIndex = 10;
            this.OriginalBpmRangeTextBox.Text = "(180 - 210)";
            // 
            // OriginalBpmTextBox
            // 
            this.OriginalBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.OriginalBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalBpmTextBox.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.OriginalBpmTextBox.Location = new System.Drawing.Point(89, 35);
            this.OriginalBpmTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.OriginalBpmTextBox.Name = "OriginalBpmTextBox";
            this.OriginalBpmTextBox.ReadOnly = true;
            this.OriginalBpmTextBox.Size = new System.Drawing.Size(42, 17);
            this.OriginalBpmTextBox.TabIndex = 10;
            this.OriginalBpmTextBox.Text = "200";
            this.OriginalBpmTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OriginalBpmTextBox.Enter += new System.EventHandler(this.Unfocus);
            // 
            // Middle1Panel
            // 
            this.Middle1Panel.Controls.Add(this.tableLayoutPanel1);
            this.Middle1Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Middle1Panel.Location = new System.Drawing.Point(0, 185);
            this.Middle1Panel.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.Middle1Panel.Name = "Middle1Panel";
            this.Middle1Panel.Padding = new System.Windows.Forms.Padding(10, 0, 15, 5);
            this.Middle1Panel.Size = new System.Drawing.Size(427, 118);
            this.Middle1Panel.TabIndex = 10;
            this.Middle1Panel.Click += new System.EventHandler(this.Unfocus);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Controls.Add(this.ODLockCheck, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.ARLockCheck, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.CSLockCheck, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.odlabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ODDisplay, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ODSlider, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.arlabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ARDisplay, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ARSlider, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cslabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CSDisplay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CSSlider, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.hplabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.HPDisplay, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.HPSlider, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.HPLockCheck, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Tai Le", 12F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(402, 113);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ODDisplay
            // 
            this.ODDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ODDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.ODDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ODDisplay.Enabled = false;
            this.ODDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ODDisplay.Location = new System.Drawing.Point(41, 89);
            this.ODDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ODDisplay.Name = "ODDisplay";
            this.ODDisplay.ReadOnly = true;
            this.ODDisplay.Size = new System.Drawing.Size(35, 21);
            this.ODDisplay.TabIndex = 10;
            this.ODDisplay.Text = "0.0";
            this.ODDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ODDisplay.Enter += new System.EventHandler(this.Unfocus);
            // 
            // ARDisplay
            // 
            this.ARDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ARDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.ARDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ARDisplay.Enabled = false;
            this.ARDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ARDisplay.Location = new System.Drawing.Point(41, 61);
            this.ARDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ARDisplay.Name = "ARDisplay";
            this.ARDisplay.ReadOnly = true;
            this.ARDisplay.Size = new System.Drawing.Size(35, 21);
            this.ARDisplay.TabIndex = 7;
            this.ARDisplay.Text = "0.0";
            this.ARDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ARDisplay.Enter += new System.EventHandler(this.Unfocus);
            // 
            // CSDisplay
            // 
            this.CSDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CSDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.CSDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CSDisplay.Enabled = false;
            this.CSDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CSDisplay.Location = new System.Drawing.Point(41, 33);
            this.CSDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.CSDisplay.Name = "CSDisplay";
            this.CSDisplay.ReadOnly = true;
            this.CSDisplay.Size = new System.Drawing.Size(35, 21);
            this.CSDisplay.TabIndex = 4;
            this.CSDisplay.Text = "0.0";
            this.CSDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CSDisplay.Enter += new System.EventHandler(this.Unfocus);
            // 
            // HPDisplay
            // 
            this.HPDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.HPDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.HPDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HPDisplay.Enabled = false;
            this.HPDisplay.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HPDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.HPDisplay.Location = new System.Drawing.Point(41, 5);
            this.HPDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.HPDisplay.Name = "HPDisplay";
            this.HPDisplay.ReadOnly = true;
            this.HPDisplay.Size = new System.Drawing.Size(35, 21);
            this.HPDisplay.TabIndex = 1;
            this.HPDisplay.Text = "0.0";
            this.HPDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HPDisplay.Enter += new System.EventHandler(this.Unfocus);
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.DeleteButton);
            this.BottomPanel.Controls.Add(this.ResetButton);
            this.BottomPanel.Controls.Add(this.GenerateMapButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 425);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.BottomPanel.Size = new System.Drawing.Size(427, 86);
            this.BottomPanel.TabIndex = 11;
            this.BottomPanel.Click += new System.EventHandler(this.Unfocus);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.TopPanel.Controls.Add(this.SongDisplay);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 30);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Padding = new System.Windows.Forms.Padding(10);
            this.TopPanel.Size = new System.Drawing.Size(427, 121);
            this.TopPanel.TabIndex = 12;
            // 
            // OsuRunningTimer
            // 
            this.OsuRunningTimer.Interval = 500;
            this.OsuRunningTimer.Tick += new System.EventHandler(this.OsuRunningTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AimLabel);
            this.panel1.Controls.Add(this.SpeedLabel);
            this.panel1.Controls.Add(this.StarsDisplay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 151);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.panel1.Size = new System.Drawing.Size(427, 34);
            this.panel1.TabIndex = 13;
            // 
            // AimLabel
            // 
            this.AimLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AimLabel.AutoSize = true;
            this.AimLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AimLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.AimLabel.Location = new System.Drawing.Point(266, 14);
            this.AimLabel.MinimumSize = new System.Drawing.Size(40, 0);
            this.AimLabel.Name = "AimLabel";
            this.AimLabel.Size = new System.Drawing.Size(50, 16);
            this.AimLabel.TabIndex = 23;
            this.AimLabel.Text = "1.0 aim";
            this.AimLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.SpeedLabel.Location = new System.Drawing.Point(194, 14);
            this.SpeedLabel.MinimumSize = new System.Drawing.Size(64, 0);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(65, 16);
            this.SpeedLabel.TabIndex = 23;
            this.SpeedLabel.Text = "1.0 speed";
            this.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BackgroundWorker
            // 
            this.BackgroundWorker.WorkerReportsProgress = true;
            this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.BackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(10)))), ((int)(((byte)(20)))));
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9F);
            this.closeButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.closeButton.Location = new System.Drawing.Point(382, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(45, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "✕";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.minimizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.minimizeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9F);
            this.minimizeButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.minimizeButton.Location = new System.Drawing.Point(337, 0);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(45, 30);
            this.minimizeButton.TabIndex = 3;
            this.minimizeButton.Text = "🗕";
            this.minimizeButton.UseVisualStyleBackColor = false;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.titlePanel.Controls.Add(this.minimizeButton);
            this.titlePanel.Controls.Add(this.closeButton);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(427, 30);
            this.titlePanel.TabIndex = 14;
            this.titlePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.titlePanel_Paint);
            this.titlePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseDown);
            this.titlePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseMove);
            this.titlePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseUp);
            // 
            // BpmLockCheck
            // 
            this.BpmLockCheck.CheckedImage = global::osu_trainer.Properties.Resources.lock_solid;
            this.BpmLockCheck.Location = new System.Drawing.Point(136, 62);
            this.BpmLockCheck.Name = "BpmLockCheck";
            this.BpmLockCheck.Size = new System.Drawing.Size(21, 21);
            this.BpmLockCheck.TabIndex = 15;
            this.BpmLockCheck.UncheckedImage = global::osu_trainer.Properties.Resources.unlock_solid;
            this.BpmLockCheck.UseVisualStyleBackColor = true;
            this.BpmLockCheck.CheckedChanged += new System.EventHandler(this.BpmLockCheck_CheckedChanged);
            // 
            // BpmSlider
            // 
            this.BpmSlider.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.BpmSlider.FillDraggingNipple = false;
            this.BpmSlider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BpmSlider.Location = new System.Drawing.Point(136, 2);
            this.BpmSlider.MaxValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BpmSlider.MinValue = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.BpmSlider.Name = "BpmSlider";
            this.BpmSlider.NippleColor = System.Drawing.Color.White;
            this.BpmSlider.NippleDiameter = 15;
            this.BpmSlider.NippleExpandedDiameter = 18;
            this.BpmSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BpmSlider.NippleStrokeWidth = 0;
            this.BpmSlider.Size = new System.Drawing.Size(131, 23);
            this.BpmSlider.TabIndex = 23;
            this.BpmSlider.Text = "bpmSlider";
            this.BpmSlider.Thickness = 4;
            this.BpmSlider.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BpmSlider.ValueChanged += new System.EventHandler(this.BpmSlider_ValueChanged);
            // 
            // ChangePitchCheck
            // 
            this.ChangePitchCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangePitchCheck.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.ChangePitchCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.ChangePitchCheck.Location = new System.Drawing.Point(287, 74);
            this.ChangePitchCheck.Name = "ChangePitchCheck";
            this.ChangePitchCheck.Size = new System.Drawing.Size(130, 24);
            this.ChangePitchCheck.TabIndex = 22;
            this.ChangePitchCheck.Text = "Change pitch";
            this.ChangePitchCheck.UseVisualStyleBackColor = true;
            this.ChangePitchCheck.CheckedChanged += new System.EventHandler(this.ChangePitchButton_CheckedChanged);
            // 
            // NoSpinnersCheck
            // 
            this.NoSpinnersCheck.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.NoSpinnersCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.NoSpinnersCheck.Location = new System.Drawing.Point(293, 99);
            this.NoSpinnersCheck.Name = "NoSpinnersCheck";
            this.NoSpinnersCheck.Size = new System.Drawing.Size(124, 24);
            this.NoSpinnersCheck.TabIndex = 20;
            this.NoSpinnersCheck.Text = "No Spinners";
            this.NoSpinnersCheck.UseVisualStyleBackColor = true;
            this.NoSpinnersCheck.CheckedChanged += new System.EventHandler(this.NoSpinnerCheckBox_CheckedChanged);
            // 
            // HRCheck
            // 
            this.HRCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HRCheck.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.HRCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.HRCheck.Location = new System.Drawing.Point(295, 0);
            this.HRCheck.Name = "HRCheck";
            this.HRCheck.Size = new System.Drawing.Size(122, 24);
            this.HRCheck.TabIndex = 20;
            this.HRCheck.Text = "Emulate HR";
            this.HRCheck.UseVisualStyleBackColor = true;
            this.HRCheck.CheckedChanged += new System.EventHandler(this.HRCheck_CheckedChanged);
            // 
            // ScaleODCheck
            // 
            this.ScaleODCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleODCheck.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.ScaleODCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.ScaleODCheck.Location = new System.Drawing.Point(313, 49);
            this.ScaleODCheck.Name = "ScaleODCheck";
            this.ScaleODCheck.Size = new System.Drawing.Size(104, 25);
            this.ScaleODCheck.TabIndex = 21;
            this.ScaleODCheck.Text = "Scale OD";
            this.ScaleODCheck.UseVisualStyleBackColor = true;
            this.ScaleODCheck.CheckedChanged += new System.EventHandler(this.ScaleODCheck_CheckedChanged);
            // 
            // ScaleARCheck
            // 
            this.ScaleARCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleARCheck.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.ScaleARCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.ScaleARCheck.Location = new System.Drawing.Point(315, 24);
            this.ScaleARCheck.Name = "ScaleARCheck";
            this.ScaleARCheck.Size = new System.Drawing.Size(102, 24);
            this.ScaleARCheck.TabIndex = 20;
            this.ScaleARCheck.Text = "Scale AR";
            this.ScaleARCheck.UseVisualStyleBackColor = true;
            this.ScaleARCheck.CheckedChanged += new System.EventHandler(this.ScaleARCheck_CheckedChanged);
            // 
            // OriginalBpmLabel
            // 
            this.OriginalBpmLabel.AutoSize = true;
            this.OriginalBpmLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalBpmLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.OriginalBpmLabel.Location = new System.Drawing.Point(18, 36);
            this.OriginalBpmLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.OriginalBpmLabel.Name = "OriginalBpmLabel";
            this.OriginalBpmLabel.Size = new System.Drawing.Size(62, 16);
            this.OriginalBpmLabel.TabIndex = 9;
            this.OriginalBpmLabel.Text = "Old BPM";
            this.OriginalBpmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NewBpmLabel
            // 
            this.NewBpmLabel.AutoSize = true;
            this.NewBpmLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewBpmLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.NewBpmLabel.Location = new System.Drawing.Point(15, 65);
            this.NewBpmLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.NewBpmLabel.Name = "NewBpmLabel";
            this.NewBpmLabel.Size = new System.Drawing.Size(67, 16);
            this.NewBpmLabel.TabIndex = 9;
            this.NewBpmLabel.Text = "New BPM";
            this.NewBpmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BpmMultiplierLabel
            // 
            this.BpmMultiplierLabel.AutoSize = true;
            this.BpmMultiplierLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.BpmMultiplierLabel.Location = new System.Drawing.Point(47, 7);
            this.BpmMultiplierLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BpmMultiplierLabel.Name = "BpmMultiplierLabel";
            this.BpmMultiplierLabel.Size = new System.Drawing.Size(35, 16);
            this.BpmMultiplierLabel.TabIndex = 1;
            this.BpmMultiplierLabel.Text = "Rate";
            this.BpmMultiplierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ODLockCheck
            // 
            this.ODLockCheck.CheckedImage = ((System.Drawing.Image)(resources.GetObject("ODLockCheck.CheckedImage")));
            this.ODLockCheck.Location = new System.Drawing.Point(379, 87);
            this.ODLockCheck.Name = "ODLockCheck";
            this.ODLockCheck.Size = new System.Drawing.Size(20, 21);
            this.ODLockCheck.TabIndex = 15;
            this.ODLockCheck.UncheckedImage = ((System.Drawing.Image)(resources.GetObject("ODLockCheck.UncheckedImage")));
            this.ODLockCheck.UseVisualStyleBackColor = true;
            this.ODLockCheck.CheckedChanged += new System.EventHandler(this.OdLockCheck_CheckedChanged);
            // 
            // ARLockCheck
            // 
            this.ARLockCheck.CheckedImage = ((System.Drawing.Image)(resources.GetObject("ARLockCheck.CheckedImage")));
            this.ARLockCheck.Location = new System.Drawing.Point(379, 59);
            this.ARLockCheck.Name = "ARLockCheck";
            this.ARLockCheck.Size = new System.Drawing.Size(20, 21);
            this.ARLockCheck.TabIndex = 14;
            this.ARLockCheck.UncheckedImage = ((System.Drawing.Image)(resources.GetObject("ARLockCheck.UncheckedImage")));
            this.ARLockCheck.UseVisualStyleBackColor = true;
            this.ARLockCheck.CheckedChanged += new System.EventHandler(this.ArLockCheck_CheckedChanged);
            // 
            // CSLockCheck
            // 
            this.CSLockCheck.CheckedImage = ((System.Drawing.Image)(resources.GetObject("CSLockCheck.CheckedImage")));
            this.CSLockCheck.Location = new System.Drawing.Point(379, 31);
            this.CSLockCheck.Name = "CSLockCheck";
            this.CSLockCheck.Size = new System.Drawing.Size(20, 21);
            this.CSLockCheck.TabIndex = 13;
            this.CSLockCheck.UncheckedImage = ((System.Drawing.Image)(resources.GetObject("CSLockCheck.UncheckedImage")));
            this.CSLockCheck.UseVisualStyleBackColor = true;
            this.CSLockCheck.CheckedChanged += new System.EventHandler(this.CsLockCheck_CheckedChanged);
            // 
            // odlabel
            // 
            this.odlabel.AutoSize = true;
            this.odlabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.odlabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold);
            this.odlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.odlabel.Location = new System.Drawing.Point(9, 93);
            this.odlabel.Margin = new System.Windows.Forms.Padding(9, 9, 3, 0);
            this.odlabel.Name = "odlabel";
            this.odlabel.Size = new System.Drawing.Size(26, 20);
            this.odlabel.TabIndex = 9;
            this.odlabel.Text = "OD";
            this.odlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ODSlider
            // 
            this.ODSlider.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.ODSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ODSlider.FillDraggingNipple = false;
            this.ODSlider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ODSlider.Location = new System.Drawing.Point(82, 87);
            this.ODSlider.MaxValue = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.ODSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ODSlider.Name = "ODSlider";
            this.ODSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.ODSlider.NippleDiameter = 15;
            this.ODSlider.NippleExpandedDiameter = 18;
            this.ODSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ODSlider.NippleStrokeWidth = 0;
            this.ODSlider.Size = new System.Drawing.Size(291, 23);
            this.ODSlider.TabIndex = 11;
            this.ODSlider.Text = "HPSlider";
            this.ODSlider.Thickness = 4;
            this.ODSlider.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ODSlider.ValueChanged += new System.EventHandler(this.OdSlider_ValueChanged);
            // 
            // arlabel
            // 
            this.arlabel.AutoSize = true;
            this.arlabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arlabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold);
            this.arlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.arlabel.Location = new System.Drawing.Point(9, 65);
            this.arlabel.Margin = new System.Windows.Forms.Padding(9, 9, 3, 0);
            this.arlabel.Name = "arlabel";
            this.arlabel.Size = new System.Drawing.Size(26, 19);
            this.arlabel.TabIndex = 6;
            this.arlabel.Text = "AR";
            this.arlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ARSlider
            // 
            this.ARSlider.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.ARSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ARSlider.FillDraggingNipple = false;
            this.ARSlider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ARSlider.Location = new System.Drawing.Point(82, 59);
            this.ARSlider.MaxValue = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.ARSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ARSlider.Name = "ARSlider";
            this.ARSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.ARSlider.NippleDiameter = 15;
            this.ARSlider.NippleExpandedDiameter = 18;
            this.ARSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ARSlider.NippleStrokeWidth = 0;
            this.ARSlider.Size = new System.Drawing.Size(291, 22);
            this.ARSlider.TabIndex = 8;
            this.ARSlider.Text = "HPSlider";
            this.ARSlider.Thickness = 4;
            this.ARSlider.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ARSlider.ValueChanged += new System.EventHandler(this.ArSlider_ValueChanged);
            // 
            // cslabel
            // 
            this.cslabel.AutoSize = true;
            this.cslabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cslabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold);
            this.cslabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.cslabel.Location = new System.Drawing.Point(9, 37);
            this.cslabel.Margin = new System.Windows.Forms.Padding(9, 9, 3, 0);
            this.cslabel.Name = "cslabel";
            this.cslabel.Size = new System.Drawing.Size(26, 19);
            this.cslabel.TabIndex = 3;
            this.cslabel.Text = "CS";
            this.cslabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CSSlider
            // 
            this.CSSlider.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.CSSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CSSlider.FillDraggingNipple = false;
            this.CSSlider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.CSSlider.Location = new System.Drawing.Point(82, 31);
            this.CSSlider.Margin = new System.Windows.Forms.Padding(3, 3, 28, 3);
            this.CSSlider.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.CSSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CSSlider.Name = "CSSlider";
            this.CSSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.CSSlider.NippleDiameter = 15;
            this.CSSlider.NippleExpandedDiameter = 18;
            this.CSSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.CSSlider.NippleStrokeWidth = 0;
            this.CSSlider.Size = new System.Drawing.Size(266, 22);
            this.CSSlider.TabIndex = 5;
            this.CSSlider.Text = "HPSlider";
            this.CSSlider.Thickness = 4;
            this.CSSlider.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CSSlider.ValueChanged += new System.EventHandler(this.CsSlider_ValueChanged);
            // 
            // hplabel
            // 
            this.hplabel.AutoSize = true;
            this.hplabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hplabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold);
            this.hplabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(154)))), ((int)(((byte)(233)))));
            this.hplabel.Location = new System.Drawing.Point(9, 9);
            this.hplabel.Margin = new System.Windows.Forms.Padding(9, 9, 3, 0);
            this.hplabel.Name = "hplabel";
            this.hplabel.Size = new System.Drawing.Size(26, 19);
            this.hplabel.TabIndex = 0;
            this.hplabel.Text = "HP";
            this.hplabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HPSlider
            // 
            this.HPSlider.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.HPSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HPSlider.FillDraggingNipple = false;
            this.HPSlider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.HPSlider.Location = new System.Drawing.Point(82, 3);
            this.HPSlider.Margin = new System.Windows.Forms.Padding(3, 3, 28, 3);
            this.HPSlider.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.HPSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.HPSlider.Name = "HPSlider";
            this.HPSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.HPSlider.NippleDiameter = 15;
            this.HPSlider.NippleExpandedDiameter = 18;
            this.HPSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.HPSlider.NippleStrokeWidth = 0;
            this.HPSlider.Size = new System.Drawing.Size(266, 22);
            this.HPSlider.TabIndex = 2;
            this.HPSlider.Text = "HPSlider";
            this.HPSlider.Thickness = 4;
            this.HPSlider.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.HPSlider.ValueChanged += new System.EventHandler(this.HpSlider_ValueChanged);
            // 
            // HPLockCheck
            // 
            this.HPLockCheck.CheckedImage = global::osu_trainer.Properties.Resources.lock_solid;
            this.HPLockCheck.Location = new System.Drawing.Point(379, 3);
            this.HPLockCheck.Name = "HPLockCheck";
            this.HPLockCheck.Size = new System.Drawing.Size(20, 21);
            this.HPLockCheck.TabIndex = 12;
            this.HPLockCheck.UncheckedImage = global::osu_trainer.Properties.Resources.unlock_solid;
            this.HPLockCheck.UseVisualStyleBackColor = true;
            this.HPLockCheck.CheckedChanged += new System.EventHandler(this.HpLockCheck_CheckedChanged);
            // 
            // StarsDisplay
            // 
            this.StarsDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StarsDisplay.Enabled = false;
            this.StarsDisplay.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Bold);
            this.StarsDisplay.GameMode = FsBeatmapProcessor.GameMode.osu;
            this.StarsDisplay.Location = new System.Drawing.Point(307, 3);
            this.StarsDisplay.Name = "StarsDisplay";
            this.StarsDisplay.Size = new System.Drawing.Size(93, 35);
            this.StarsDisplay.Stars = 0F;
            this.StarsDisplay.TabIndex = 22;
            this.StarsDisplay.Text = "starsDisplay1";
            // 
            // DeleteButton
            // 
            this.DeleteButton.BrightnessRange = 0.01F;
            this.DeleteButton.Color = System.Drawing.Color.Gray;
            this.DeleteButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.ForeColor = System.Drawing.Color.White;
            this.DeleteButton.Location = new System.Drawing.Point(10, 46);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Progress = 0F;
            this.DeleteButton.ProgressColor = System.Drawing.Color.Transparent;
            this.DeleteButton.Size = new System.Drawing.Size(130, 28);
            this.DeleteButton.Subtext = "";
            this.DeleteButton.SubtextColor = System.Drawing.Color.Empty;
            this.DeleteButton.TabIndex = 1;
            this.DeleteButton.Text = "Clean Up";
            this.DeleteButton.TextYOffset = 0;
            this.DeleteButton.TriangleCount = 30;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.BrightnessRange = 0.01F;
            this.ResetButton.Color = System.Drawing.Color.SteelBlue;
            this.ResetButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.ForeColor = System.Drawing.Color.White;
            this.ResetButton.Location = new System.Drawing.Point(10, 12);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Progress = 0F;
            this.ResetButton.ProgressColor = System.Drawing.Color.Transparent;
            this.ResetButton.Size = new System.Drawing.Size(130, 28);
            this.ResetButton.Subtext = "";
            this.ResetButton.SubtextColor = System.Drawing.Color.Empty;
            this.ResetButton.TabIndex = 1;
            this.ResetButton.Text = "Reset";
            this.ResetButton.TextYOffset = 0;
            this.ResetButton.TriangleCount = 30;
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.BackColor = System.Drawing.Color.Transparent;
            this.GenerateMapButton.BrightnessRange = 0.01F;
            this.GenerateMapButton.Color = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(51)))), ((int)(((byte)(131)))));
            this.GenerateMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateMapButton.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateMapButton.ForeColor = System.Drawing.Color.White;
            this.GenerateMapButton.Location = new System.Drawing.Point(146, 12);
            this.GenerateMapButton.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Progress = 0F;
            this.GenerateMapButton.ProgressColor = System.Drawing.Color.SpringGreen;
            this.GenerateMapButton.Size = new System.Drawing.Size(271, 62);
            this.GenerateMapButton.Subtext = "Ctrl+Shift+X";
            this.GenerateMapButton.SubtextColor = System.Drawing.Color.Plum;
            this.GenerateMapButton.TabIndex = 0;
            this.GenerateMapButton.Text = "Create Map";
            this.GenerateMapButton.TextYOffset = -3;
            this.GenerateMapButton.TriangleCount = 30;
            this.GenerateMapButton.UseVisualStyleBackColor = false;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // SongDisplay
            // 
            this.SongDisplay.Artist = null;
            this.SongDisplay.Cover = null;
            this.SongDisplay.Difficulty = null;
            this.SongDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SongDisplay.ErrorMessage = null;
            this.SongDisplay.Location = new System.Drawing.Point(10, 10);
            this.SongDisplay.Name = "SongDisplay";
            this.SongDisplay.Size = new System.Drawing.Size(407, 101);
            this.SongDisplay.TabIndex = 7;
            this.SongDisplay.Text = "songDisplay1";
            this.SongDisplay.Title = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(427, 511);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Middle1Panel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.titlePanel);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "osu trainer v1.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Panel3.ResumeLayout(false);
            this.Panel3.PerformLayout();
            this.Middle1Panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntiAliasedLabel BpmMultiplierLabel;
        private System.Windows.Forms.Timer BeatmapUpdateTimer;
        private AntiAliasedLabel OriginalBpmLabel;
        private AntiAliasedLabel NewBpmLabel;
        private OsuButton GenerateMapButton;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel Panel3;
        private System.Windows.Forms.Panel Middle1Panel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntiAliasedLabel hplabel;
        private System.Windows.Forms.TextBox HPDisplay;
        private OptionSlider HPSlider;
        private AntiAliasedLabel odlabel;
        private System.Windows.Forms.TextBox ODDisplay;
        private OptionSlider ODSlider;
        private AntiAliasedLabel arlabel;
        private System.Windows.Forms.TextBox ARDisplay;
        private OptionSlider ARSlider;
        private AntiAliasedLabel cslabel;
        private System.Windows.Forms.TextBox CSDisplay;
        private OptionSlider CSSlider;
        private System.Windows.Forms.TextBox NewBpmTextBox;
        private System.Windows.Forms.TextBox NewBpmRangeTextBox;
        private System.Windows.Forms.TextBox OriginalBpmRangeTextBox;
        private System.Windows.Forms.TextBox OriginalBpmTextBox;
        private OsuButton ResetButton;
        private System.Windows.Forms.Timer OsuRunningTimer;
        private OsuButton DeleteButton;
        private System.Windows.Forms.Panel panel1;
        private StarsDisplay StarsDisplay;
        private OsuCheckBox ScaleARCheck;
        private OsuCheckBox ChangePitchCheck;
        private OsuCheckBox ScaleODCheck;
        private ToggleIconButton ODLockCheck;
        private ToggleIconButton ARLockCheck;
        private ToggleIconButton CSLockCheck;
        private ToggleIconButton HPLockCheck;
        public System.ComponentModel.BackgroundWorker BackgroundWorker;
        private SongDisplay SongDisplay;
        private System.Windows.Forms.TextBox BpmMultiplierTextBox;
        private OsuCheckBox NoSpinnersCheck;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Panel titlePanel;
        private OptionSlider BpmSlider;
        private System.Windows.Forms.Label AimLabel;
        private System.Windows.Forms.Label SpeedLabel;
        private ToggleIconButton BpmLockCheck;
        private OsuCheckBox HRCheck;
    }
}