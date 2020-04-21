using osu_trainer.funorControls;

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
            this.label2 = new System.Windows.Forms.Label();
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            this.MiddlePanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.AimSpeedBar = new osu_trainer.funorControls.RatioBar();
            this.AimLabel = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.StarLabel = new System.Windows.Forms.Label();
            this.NewBpmTextBox = new System.Windows.Forms.TextBox();
            this.NewBpmRangeTextBox = new System.Windows.Forms.TextBox();
            this.OriginalBpmRangeTextBox = new System.Windows.Forms.TextBox();
            this.OriginalBpmTextBox = new System.Windows.Forms.TextBox();
            this.changePitchButton = new System.Windows.Forms.CheckBox();
            this.ScaleODCheck = new System.Windows.Forms.CheckBox();
            this.ScaleARCheck = new System.Windows.Forms.CheckBox();
            this.BpmMultiplierUpDown = new osu_trainer.NumericUpDownFix();
            this.Middle1Panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ODLockCheck = new System.Windows.Forms.CheckBox();
            this.ARLockCheck = new System.Windows.Forms.CheckBox();
            this.CSLockCheck = new System.Windows.Forms.CheckBox();
            this.odlabel = new System.Windows.Forms.Label();
            this.ODDisplay = new System.Windows.Forms.TextBox();
            this.ODSlider = new osu_trainer.OptionSlider();
            this.arlabel = new System.Windows.Forms.Label();
            this.ARDisplay = new System.Windows.Forms.TextBox();
            this.ARSlider = new osu_trainer.OptionSlider();
            this.cslabel = new System.Windows.Forms.Label();
            this.CSDisplay = new System.Windows.Forms.TextBox();
            this.CSSlider = new osu_trainer.OptionSlider();
            this.hplabel = new System.Windows.Forms.Label();
            this.HPDisplay = new System.Windows.Forms.TextBox();
            this.HPSlider = new osu_trainer.OptionSlider();
            this.HPLockCheck = new System.Windows.Forms.CheckBox();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.icon = new System.Windows.Forms.PictureBox();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.maximizeButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.SongLabel = new System.Windows.Forms.Label();
            this.DiffLabel = new System.Windows.Forms.Label();
            this.BgPanel = new osu_trainer.funorControls.DoubleBufferedPanel();
            this.StaticGif = new System.Windows.Forms.PictureBox();
            this.OsuRunningTimer = new System.Windows.Forms.Timer(this.components);
            this.MiddlePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).BeginInit();
            this.Middle1Panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.BgPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StaticGif)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label2.Location = new System.Drawing.Point(7, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "BPM Multiplier";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 20;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label4.Location = new System.Drawing.Point(18, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Original BPM";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label5.Location = new System.Drawing.Point(41, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "New BPM";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GenerateMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(114)))), ((int)(((byte)(185)))));
            this.GenerateMapButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GenerateMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateMapButton.Font = new System.Drawing.Font("Microsoft Tai Le", 11F, System.Drawing.FontStyle.Bold);
            this.GenerateMapButton.ForeColor = System.Drawing.Color.White;
            this.GenerateMapButton.Location = new System.Drawing.Point(10, 61);
            this.GenerateMapButton.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Size = new System.Drawing.Size(407, 40);
            this.GenerateMapButton.TabIndex = 0;
            this.GenerateMapButton.Text = "3. Create Map";
            this.GenerateMapButton.UseVisualStyleBackColor = false;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // MiddlePanel
            // 
            this.MiddlePanel.Controls.Add(this.panel2);
            this.MiddlePanel.Controls.Add(this.Middle1Panel);
            this.MiddlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiddlePanel.Location = new System.Drawing.Point(0, 125);
            this.MiddlePanel.Margin = new System.Windows.Forms.Padding(10);
            this.MiddlePanel.Name = "MiddlePanel";
            this.MiddlePanel.Size = new System.Drawing.Size(427, 226);
            this.MiddlePanel.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 126);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(427, 100);
            this.panel2.TabIndex = 11;
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.AimSpeedBar);
            this.Panel3.Controls.Add(this.AimLabel);
            this.Panel3.Controls.Add(this.SpeedLabel);
            this.Panel3.Controls.Add(this.StarLabel);
            this.Panel3.Controls.Add(this.NewBpmTextBox);
            this.Panel3.Controls.Add(this.NewBpmRangeTextBox);
            this.Panel3.Controls.Add(this.OriginalBpmRangeTextBox);
            this.Panel3.Controls.Add(this.OriginalBpmTextBox);
            this.Panel3.Controls.Add(this.changePitchButton);
            this.Panel3.Controls.Add(this.ScaleODCheck);
            this.Panel3.Controls.Add(this.ScaleARCheck);
            this.Panel3.Controls.Add(this.label4);
            this.Panel3.Controls.Add(this.label5);
            this.Panel3.Controls.Add(this.label2);
            this.Panel3.Controls.Add(this.BpmMultiplierUpDown);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel3.Location = new System.Drawing.Point(0, 0);
            this.Panel3.Name = "Panel3";
            this.Panel3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Panel3.Size = new System.Drawing.Size(427, 100);
            this.Panel3.TabIndex = 7;
            this.Panel3.Click += new System.EventHandler(this.Unfocus);
            // 
            // AimSpeedBar
            // 
            this.AimSpeedBar.LeftColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(201)))));
            this.AimSpeedBar.LeftPercent = 40;
            this.AimSpeedBar.Location = new System.Drawing.Point(276, 82);
            this.AimSpeedBar.Name = "AimSpeedBar";
            this.AimSpeedBar.RightColour = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(241)))), ((int)(((byte)(184)))));
            this.AimSpeedBar.Size = new System.Drawing.Size(135, 3);
            this.AimSpeedBar.TabIndex = 21;
            this.AimSpeedBar.Text = "ratioBar1";
            // 
            // AimLabel
            // 
            this.AimLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AimLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AimLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(201)))));
            this.AimLabel.Location = new System.Drawing.Point(273, 66);
            this.AimLabel.Name = "AimLabel";
            this.AimLabel.Size = new System.Drawing.Size(66, 23);
            this.AimLabel.TabIndex = 20;
            this.AimLabel.Text = "aim: 50%";
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeedLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(241)))), ((int)(((byte)(184)))));
            this.SpeedLabel.Location = new System.Drawing.Point(350, 66);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(66, 23);
            this.SpeedLabel.TabIndex = 20;
            this.SpeedLabel.Text = "spd: 50%";
            this.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StarLabel
            // 
            this.StarLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StarLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StarLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(132)))), ((int)(((byte)(65)))));
            this.StarLabel.Location = new System.Drawing.Point(306, 43);
            this.StarLabel.Name = "StarLabel";
            this.StarLabel.Size = new System.Drawing.Size(114, 23);
            this.StarLabel.TabIndex = 20;
            this.StarLabel.Text = "0.00 ☆";
            this.StarLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // NewBpmTextBox
            // 
            this.NewBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(16)))), ((int)(((byte)(25)))));
            this.NewBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NewBpmTextBox.Location = new System.Drawing.Point(118, 72);
            this.NewBpmTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.NewBpmTextBox.Name = "NewBpmTextBox";
            this.NewBpmTextBox.Size = new System.Drawing.Size(42, 17);
            this.NewBpmTextBox.TabIndex = 10;
            this.NewBpmTextBox.Text = "200";
            this.NewBpmTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NewBpmTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewBpmTextBox_KeyDown);
            this.NewBpmTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewBpmTextBox_KeyPress);
            this.NewBpmTextBox.Leave += new System.EventHandler(this.NewBpmTextBox_Leave);
            // 
            // NewBpmRangeTextBox
            // 
            this.NewBpmRangeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.NewBpmRangeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewBpmRangeTextBox.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewBpmRangeTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.NewBpmRangeTextBox.Location = new System.Drawing.Point(166, 72);
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
            this.OriginalBpmRangeTextBox.Location = new System.Drawing.Point(166, 48);
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
            this.OriginalBpmTextBox.Location = new System.Drawing.Point(118, 48);
            this.OriginalBpmTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.OriginalBpmTextBox.Name = "OriginalBpmTextBox";
            this.OriginalBpmTextBox.ReadOnly = true;
            this.OriginalBpmTextBox.Size = new System.Drawing.Size(42, 17);
            this.OriginalBpmTextBox.TabIndex = 10;
            this.OriginalBpmTextBox.Text = "200";
            this.OriginalBpmTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OriginalBpmTextBox.Enter += new System.EventHandler(this.Unfocus);
            // 
            // changePitchButton
            // 
            this.changePitchButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.changePitchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.changePitchButton.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.changePitchButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.changePitchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.changePitchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.changePitchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changePitchButton.Font = new System.Drawing.Font("Carlito", 8.25F);
            this.changePitchButton.ForeColor = System.Drawing.Color.Gray;
            this.changePitchButton.Location = new System.Drawing.Point(311, 8);
            this.changePitchButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.changePitchButton.Name = "changePitchButton";
            this.changePitchButton.Size = new System.Drawing.Size(80, 23);
            this.changePitchButton.TabIndex = 19;
            this.changePitchButton.Text = "Change pitch";
            this.changePitchButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changePitchButton.UseVisualStyleBackColor = false;
            this.changePitchButton.CheckedChanged += new System.EventHandler(this.ChangePitchButton_CheckedChanged);
            // 
            // ScaleODCheck
            // 
            this.ScaleODCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.ScaleODCheck.AutoSize = true;
            this.ScaleODCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ScaleODCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ScaleODCheck.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(226)))), ((int)(((byte)(250)))));
            this.ScaleODCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ScaleODCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.ScaleODCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScaleODCheck.Font = new System.Drawing.Font("Carlito", 8.25F);
            this.ScaleODCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(226)))), ((int)(((byte)(250)))));
            this.ScaleODCheck.Location = new System.Drawing.Point(251, 8);
            this.ScaleODCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ScaleODCheck.Name = "ScaleODCheck";
            this.ScaleODCheck.Size = new System.Drawing.Size(56, 23);
            this.ScaleODCheck.TabIndex = 19;
            this.ScaleODCheck.Text = "Scale OD";
            this.ScaleODCheck.UseVisualStyleBackColor = false;
            this.ScaleODCheck.CheckedChanged += new System.EventHandler(this.ScaleODCheck_CheckedChanged);
            // 
            // ScaleARCheck
            // 
            this.ScaleARCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.ScaleARCheck.AutoSize = true;
            this.ScaleARCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ScaleARCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ScaleARCheck.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(226)))), ((int)(((byte)(250)))));
            this.ScaleARCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ScaleARCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.ScaleARCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScaleARCheck.Font = new System.Drawing.Font("Carlito", 8.25F);
            this.ScaleARCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(226)))), ((int)(((byte)(250)))));
            this.ScaleARCheck.Location = new System.Drawing.Point(191, 8);
            this.ScaleARCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ScaleARCheck.Name = "ScaleARCheck";
            this.ScaleARCheck.Size = new System.Drawing.Size(54, 23);
            this.ScaleARCheck.TabIndex = 19;
            this.ScaleARCheck.Text = "Scale AR";
            this.ScaleARCheck.UseVisualStyleBackColor = false;
            this.ScaleARCheck.CheckedChanged += new System.EventHandler(this.ScaleARCheck_CheckedChanged);
            // 
            // BpmMultiplierUpDown
            // 
            this.BpmMultiplierUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(16)))), ((int)(((byte)(25)))));
            this.BpmMultiplierUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BpmMultiplierUpDown.DecimalPlaces = 2;
            this.BpmMultiplierUpDown.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BpmMultiplierUpDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BpmMultiplierUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BpmMultiplierUpDown.Location = new System.Drawing.Point(116, 8);
            this.BpmMultiplierUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BpmMultiplierUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.BpmMultiplierUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BpmMultiplierUpDown.Name = "BpmMultiplierUpDown";
            this.BpmMultiplierUpDown.Size = new System.Drawing.Size(68, 24);
            this.BpmMultiplierUpDown.TabIndex = 4;
            this.BpmMultiplierUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BpmMultiplierUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BpmMultiplierUpDown.ValueChanged += new System.EventHandler(this.BpmMultiplierUpDown_ValueChanged);
            // 
            // Middle1Panel
            // 
            this.Middle1Panel.Controls.Add(this.tableLayoutPanel1);
            this.Middle1Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Middle1Panel.Location = new System.Drawing.Point(0, 0);
            this.Middle1Panel.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.Middle1Panel.Name = "Middle1Panel";
            this.Middle1Panel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.Middle1Panel.Size = new System.Drawing.Size(427, 126);
            this.Middle1Panel.TabIndex = 10;
            this.Middle1Panel.Click += new System.EventHandler(this.Unfocus);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(407, 116);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ODLockCheck
            // 
            this.ODLockCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.ODLockCheck.AutoSize = true;
            this.ODLockCheck.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.ODLockCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ODLockCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.ODLockCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ODLockCheck.Font = new System.Drawing.Font("Carlito", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ODLockCheck.ForeColor = System.Drawing.Color.Gray;
            this.ODLockCheck.Location = new System.Drawing.Point(358, 92);
            this.ODLockCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ODLockCheck.Name = "ODLockCheck";
            this.ODLockCheck.Size = new System.Drawing.Size(38, 21);
            this.ODLockCheck.TabIndex = 18;
            this.ODLockCheck.Text = "Lock";
            this.ODLockCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ODLockCheck.UseVisualStyleBackColor = true;
            this.ODLockCheck.CheckedChanged += new System.EventHandler(this.OdLockCheck_CheckedChanged);
            // 
            // ARLockCheck
            // 
            this.ARLockCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.ARLockCheck.AutoSize = true;
            this.ARLockCheck.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.ARLockCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ARLockCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.ARLockCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ARLockCheck.Font = new System.Drawing.Font("Carlito", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ARLockCheck.ForeColor = System.Drawing.Color.Gray;
            this.ARLockCheck.Location = new System.Drawing.Point(358, 63);
            this.ARLockCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ARLockCheck.Name = "ARLockCheck";
            this.ARLockCheck.Size = new System.Drawing.Size(38, 21);
            this.ARLockCheck.TabIndex = 16;
            this.ARLockCheck.Text = "Lock";
            this.ARLockCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ARLockCheck.UseVisualStyleBackColor = true;
            this.ARLockCheck.CheckedChanged += new System.EventHandler(this.ArLockCheck_CheckedChanged);
            // 
            // CSLockCheck
            // 
            this.CSLockCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.CSLockCheck.AutoSize = true;
            this.CSLockCheck.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.CSLockCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.CSLockCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.CSLockCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CSLockCheck.Font = new System.Drawing.Font("Carlito", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSLockCheck.ForeColor = System.Drawing.Color.Gray;
            this.CSLockCheck.Location = new System.Drawing.Point(358, 34);
            this.CSLockCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.CSLockCheck.Name = "CSLockCheck";
            this.CSLockCheck.Size = new System.Drawing.Size(38, 21);
            this.CSLockCheck.TabIndex = 14;
            this.CSLockCheck.Text = "Lock";
            this.CSLockCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CSLockCheck.UseVisualStyleBackColor = true;
            this.CSLockCheck.CheckedChanged += new System.EventHandler(this.CsLockCheck_CheckedChanged);
            // 
            // odlabel
            // 
            this.odlabel.AutoSize = true;
            this.odlabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.odlabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold);
            this.odlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.odlabel.Location = new System.Drawing.Point(3, 90);
            this.odlabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.odlabel.Name = "odlabel";
            this.odlabel.Size = new System.Drawing.Size(32, 26);
            this.odlabel.TabIndex = 9;
            this.odlabel.Text = "OD";
            this.odlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ODDisplay
            // 
            this.ODDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ODDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.ODDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ODDisplay.Enabled = false;
            this.ODDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ODDisplay.Location = new System.Drawing.Point(41, 92);
            this.ODDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ODDisplay.Name = "ODDisplay";
            this.ODDisplay.ReadOnly = true;
            this.ODDisplay.Size = new System.Drawing.Size(42, 21);
            this.ODDisplay.TabIndex = 10;
            this.ODDisplay.Text = "0.0";
            this.ODDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ODDisplay.Enter += new System.EventHandler(this.Unfocus);
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
            this.ODSlider.Location = new System.Drawing.Point(89, 90);
            this.ODSlider.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ODSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ODSlider.Name = "ODSlider";
            this.ODSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ODSlider.NippleDiameter = 15;
            this.ODSlider.NippleExpandedDiameter = 18;
            this.ODSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.ODSlider.NippleStrokeWidth = 2;
            this.ODSlider.Size = new System.Drawing.Size(263, 23);
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
            this.arlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.arlabel.Location = new System.Drawing.Point(3, 61);
            this.arlabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.arlabel.Name = "arlabel";
            this.arlabel.Size = new System.Drawing.Size(32, 26);
            this.arlabel.TabIndex = 6;
            this.arlabel.Text = "AR";
            this.arlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ARDisplay
            // 
            this.ARDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ARDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.ARDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ARDisplay.Enabled = false;
            this.ARDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ARDisplay.Location = new System.Drawing.Point(41, 63);
            this.ARDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ARDisplay.Name = "ARDisplay";
            this.ARDisplay.ReadOnly = true;
            this.ARDisplay.Size = new System.Drawing.Size(42, 21);
            this.ARDisplay.TabIndex = 7;
            this.ARDisplay.Text = "0.0";
            this.ARDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ARDisplay.Enter += new System.EventHandler(this.Unfocus);
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
            this.ARSlider.Location = new System.Drawing.Point(89, 61);
            this.ARSlider.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ARSlider.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ARSlider.Name = "ARSlider";
            this.ARSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ARSlider.NippleDiameter = 15;
            this.ARSlider.NippleExpandedDiameter = 18;
            this.ARSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.ARSlider.NippleStrokeWidth = 2;
            this.ARSlider.Size = new System.Drawing.Size(263, 23);
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
            this.cslabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.cslabel.Location = new System.Drawing.Point(3, 32);
            this.cslabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.cslabel.Name = "cslabel";
            this.cslabel.Size = new System.Drawing.Size(32, 26);
            this.cslabel.TabIndex = 3;
            this.cslabel.Text = "CS";
            this.cslabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CSDisplay
            // 
            this.CSDisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CSDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(24)))), ((int)(((byte)(38)))));
            this.CSDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CSDisplay.Enabled = false;
            this.CSDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CSDisplay.Location = new System.Drawing.Point(41, 34);
            this.CSDisplay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.CSDisplay.Name = "CSDisplay";
            this.CSDisplay.ReadOnly = true;
            this.CSDisplay.Size = new System.Drawing.Size(42, 21);
            this.CSDisplay.TabIndex = 4;
            this.CSDisplay.Text = "0.0";
            this.CSDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CSDisplay.Enter += new System.EventHandler(this.Unfocus);
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
            this.CSSlider.Location = new System.Drawing.Point(89, 32);
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
            this.CSSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.CSSlider.NippleDiameter = 15;
            this.CSSlider.NippleExpandedDiameter = 18;
            this.CSSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.CSSlider.NippleStrokeWidth = 2;
            this.CSSlider.Size = new System.Drawing.Size(263, 23);
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
            this.hplabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.hplabel.Location = new System.Drawing.Point(3, 3);
            this.hplabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.hplabel.Name = "hplabel";
            this.hplabel.Size = new System.Drawing.Size(32, 26);
            this.hplabel.TabIndex = 0;
            this.hplabel.Text = "HP";
            this.hplabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.HPDisplay.Size = new System.Drawing.Size(42, 21);
            this.HPDisplay.TabIndex = 1;
            this.HPDisplay.Text = "0.0";
            this.HPDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HPDisplay.Enter += new System.EventHandler(this.Unfocus);
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
            this.HPSlider.Location = new System.Drawing.Point(89, 3);
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
            this.HPSlider.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.HPSlider.NippleDiameter = 15;
            this.HPSlider.NippleExpandedDiameter = 18;
            this.HPSlider.NippleIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(134)))));
            this.HPSlider.NippleStrokeWidth = 2;
            this.HPSlider.Size = new System.Drawing.Size(263, 23);
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
            this.HPLockCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.HPLockCheck.AutoSize = true;
            this.HPLockCheck.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.HPLockCheck.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.HPLockCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(61)))), ((int)(((byte)(85)))));
            this.HPLockCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HPLockCheck.Font = new System.Drawing.Font("Carlito", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HPLockCheck.ForeColor = System.Drawing.Color.Gray;
            this.HPLockCheck.Location = new System.Drawing.Point(358, 5);
            this.HPLockCheck.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.HPLockCheck.Name = "HPLockCheck";
            this.HPLockCheck.Size = new System.Drawing.Size(38, 21);
            this.HPLockCheck.TabIndex = 12;
            this.HPLockCheck.Text = "Lock";
            this.HPLockCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPLockCheck.UseVisualStyleBackColor = true;
            this.HPLockCheck.CheckedChanged += new System.EventHandler(this.HpLockCheck_CheckedChanged);
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.DeleteButton);
            this.BottomPanel.Controls.Add(this.ResetButton);
            this.BottomPanel.Controls.Add(this.GenerateMapButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 351);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(10, 2, 10, 10);
            this.BottomPanel.Size = new System.Drawing.Size(427, 111);
            this.BottomPanel.TabIndex = 11;
            this.BottomPanel.Click += new System.EventHandler(this.Unfocus);
            // 
            // DeleteButton
            // 
            this.DeleteButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.ForeColor = System.Drawing.Color.DarkGray;
            this.DeleteButton.Location = new System.Drawing.Point(10, 31);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(407, 25);
            this.DeleteButton.TabIndex = 1;
            this.DeleteButton.Text = "2. Delete Unused MP3s";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.ForeColor = System.Drawing.Color.DarkGray;
            this.ResetButton.Location = new System.Drawing.Point(10, 0);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(407, 25);
            this.ResetButton.TabIndex = 1;
            this.ResetButton.Text = "1. Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.TopPanel.Controls.Add(this.icon);
            this.TopPanel.Controls.Add(this.minimizeButton);
            this.TopPanel.Controls.Add(this.maximizeButton);
            this.TopPanel.Controls.Add(this.closeButton);
            this.TopPanel.Controls.Add(this.Title);
            this.TopPanel.Controls.Add(this.SongLabel);
            this.TopPanel.Controls.Add(this.DiffLabel);
            this.TopPanel.Controls.Add(this.BgPanel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Padding = new System.Windows.Forms.Padding(10);
            this.TopPanel.Size = new System.Drawing.Size(427, 125);
            this.TopPanel.TabIndex = 12;
            this.TopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseDown);
            this.TopPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseMove);
            this.TopPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseUp);
            // 
            // icon
            // 
            this.icon.Image = ((System.Drawing.Image)(resources.GetObject("icon.Image")));
            this.icon.ImageLocation = "";
            this.icon.Location = new System.Drawing.Point(11, 10);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(16, 16);
            this.icon.TabIndex = 4;
            this.icon.TabStop = false;
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9F);
            this.minimizeButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.minimizeButton.Location = new System.Drawing.Point(287, -1);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(45, 30);
            this.minimizeButton.TabIndex = 3;
            this.minimizeButton.Text = "🗕";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximizeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.maximizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.maximizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.maximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maximizeButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9F);
            this.maximizeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(16)))), ((int)(((byte)(25)))));
            this.maximizeButton.Location = new System.Drawing.Point(335, -1);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(45, 30);
            this.maximizeButton.TabIndex = 3;
            this.maximizeButton.Text = "🗖";
            this.maximizeButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(10)))), ((int)(((byte)(20)))));
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9F);
            this.closeButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.closeButton.Location = new System.Drawing.Point(383, -1);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(45, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "✕";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(29, 7);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(78, 24);
            this.Title.TabIndex = 2;
            this.Title.Text = "osu trainer";
            this.Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseDown);
            this.Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseMove);
            this.Title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelMove_MouseUp);
            // 
            // SongLabel
            // 
            this.SongLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SongLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.SongLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.SongLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(222)))), ((int)(((byte)(93)))));
            this.SongLabel.Location = new System.Drawing.Point(8, 103);
            this.SongLabel.Name = "SongLabel";
            this.SongLabel.Size = new System.Drawing.Size(260, 14);
            this.SongLabel.TabIndex = 2;
            this.SongLabel.Text = "Artist - Title";
            // 
            // DiffLabel
            // 
            this.DiffLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DiffLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.DiffLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.DiffLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(222)))), ((int)(((byte)(93)))));
            this.DiffLabel.Location = new System.Drawing.Point(282, 103);
            this.DiffLabel.Name = "DiffLabel";
            this.DiffLabel.Size = new System.Drawing.Size(139, 14);
            this.DiffLabel.TabIndex = 2;
            this.DiffLabel.Text = "Extra";
            this.DiffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BgPanel
            // 
            this.BgPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BgPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(96)))), ((int)(((byte)(104)))));
            this.BgPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BgPanel.Controls.Add(this.StaticGif);
            this.BgPanel.Location = new System.Drawing.Point(10, 35);
            this.BgPanel.Name = "BgPanel";
            this.BgPanel.Size = new System.Drawing.Size(407, 62);
            this.BgPanel.TabIndex = 0;
            // 
            // StaticGif
            // 
            this.StaticGif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StaticGif.ErrorImage = null;
            this.StaticGif.ImageLocation = "resources\\static.gif";
            this.StaticGif.InitialImage = null;
            this.StaticGif.Location = new System.Drawing.Point(0, 0);
            this.StaticGif.Name = "StaticGif";
            this.StaticGif.Size = new System.Drawing.Size(407, 62);
            this.StaticGif.TabIndex = 0;
            this.StaticGif.TabStop = false;
            this.StaticGif.Visible = false;
            // 
            // OsuRunningTimer
            // 
            this.OsuRunningTimer.Interval = 500;
            this.OsuRunningTimer.Tick += new System.EventHandler(this.OsuRunningTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(427, 462);
            this.Controls.Add(this.MiddlePanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Beatmap Difficulty Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.MiddlePanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).EndInit();
            this.Middle1Panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.BgPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StaticGif)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private NumericUpDownFix BpmMultiplierUpDown;
        private System.Windows.Forms.Timer BeatmapUpdateTimer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button GenerateMapButton;
        private System.Windows.Forms.Panel MiddlePanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel Panel3;
        private System.Windows.Forms.Label DiffLabel;
        private System.Windows.Forms.Label SongLabel;
        private DoubleBufferedPanel BgPanel;
        private System.Windows.Forms.PictureBox StaticGif;
        private System.Windows.Forms.Panel Middle1Panel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label hplabel;
        private System.Windows.Forms.TextBox HPDisplay;
        private OptionSlider HPSlider;
        private System.Windows.Forms.Label odlabel;
        private System.Windows.Forms.TextBox ODDisplay;
        private OptionSlider ODSlider;
        private System.Windows.Forms.Label arlabel;
        private System.Windows.Forms.TextBox ARDisplay;
        private OptionSlider ARSlider;
        private System.Windows.Forms.Label cslabel;
        private System.Windows.Forms.TextBox CSDisplay;
        private OptionSlider CSSlider;
        private System.Windows.Forms.CheckBox HPLockCheck;
        private System.Windows.Forms.CheckBox ODLockCheck;
        private System.Windows.Forms.CheckBox ARLockCheck;
        private System.Windows.Forms.CheckBox CSLockCheck;
        private System.Windows.Forms.CheckBox ScaleARCheck;
        private System.Windows.Forms.Label StarLabel;
        private funorControls.RatioBar AimSpeedBar;
        private System.Windows.Forms.Label AimLabel;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.CheckBox ScaleODCheck;
        private System.Windows.Forms.CheckBox changePitchButton;
        private System.Windows.Forms.TextBox NewBpmTextBox;
        private System.Windows.Forms.TextBox NewBpmRangeTextBox;
        private System.Windows.Forms.TextBox OriginalBpmRangeTextBox;
        private System.Windows.Forms.TextBox OriginalBpmTextBox;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Timer OsuRunningTimer;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Button maximizeButton;
        private System.Windows.Forms.PictureBox icon;
    }
}