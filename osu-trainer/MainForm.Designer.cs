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
            this.SelectMapButton = new System.Windows.Forms.Button();
            this.AutoDetectMapCheckbox = new System.Windows.Forms.CheckBox();
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.OriginalBpmTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewBpmTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            this.MiddlePanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Middle3Panel = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.optionSlider1 = new osu_trainer.OptionSlider();
            this.Middle2Panel = new System.Windows.Forms.Panel();
            this.ARUpDown = new osu_trainer.NumericUpDownFix();
            this.BpmMultiplierUpDown = new osu_trainer.NumericUpDownFix();
            this.Middle1Panel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.SongLabel = new System.Windows.Forms.TextBox();
            this.DiffLabel = new System.Windows.Forms.TextBox();
            this.BgPanel = new System.Windows.Forms.Panel();
            this.StaticGif = new System.Windows.Forms.PictureBox();
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.MiddlePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Middle3Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.Middle2Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).BeginInit();
            this.Middle1Panel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.BgPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StaticGif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label2.Location = new System.Drawing.Point(18, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "BPM Multiplier";
            // 
            // SelectMapButton
            // 
            this.SelectMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(122)))), ((int)(((byte)(44)))));
            this.SelectMapButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SelectMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectMapButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectMapButton.ForeColor = System.Drawing.Color.White;
            this.SelectMapButton.Location = new System.Drawing.Point(114, 36);
            this.SelectMapButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SelectMapButton.Name = "SelectMapButton";
            this.SelectMapButton.Size = new System.Drawing.Size(195, 29);
            this.SelectMapButton.TabIndex = 5;
            this.SelectMapButton.Text = "Manually Select Map";
            this.SelectMapButton.UseVisualStyleBackColor = false;
            this.SelectMapButton.Click += new System.EventHandler(this.SelectMapButton_Click);
            // 
            // AutoDetectMapCheckbox
            // 
            this.AutoDetectMapCheckbox.AutoSize = true;
            this.AutoDetectMapCheckbox.Checked = true;
            this.AutoDetectMapCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoDetectMapCheckbox.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoDetectMapCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.AutoDetectMapCheckbox.Location = new System.Drawing.Point(114, 11);
            this.AutoDetectMapCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AutoDetectMapCheckbox.Name = "AutoDetectMapCheckbox";
            this.AutoDetectMapCheckbox.Size = new System.Drawing.Size(132, 20);
            this.AutoDetectMapCheckbox.TabIndex = 6;
            this.AutoDetectMapCheckbox.Text = "Auto Detect Map";
            this.AutoDetectMapCheckbox.UseVisualStyleBackColor = true;
            this.AutoDetectMapCheckbox.CheckedChanged += new System.EventHandler(this.AutoDetectMapCheckbox_CheckedChanged);
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 50;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // OriginalBpmTextBox
            // 
            this.OriginalBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.OriginalBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.OriginalBpmTextBox.Location = new System.Drawing.Point(4, 6);
            this.OriginalBpmTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OriginalBpmTextBox.Name = "OriginalBpmTextBox";
            this.OriginalBpmTextBox.ReadOnly = true;
            this.OriginalBpmTextBox.Size = new System.Drawing.Size(305, 17);
            this.OriginalBpmTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label4.Location = new System.Drawing.Point(11, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Original BPM(s)";
            // 
            // NewBpmTextBox
            // 
            this.NewBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.NewBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.NewBpmTextBox.Location = new System.Drawing.Point(4, 29);
            this.NewBpmTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NewBpmTextBox.Name = "NewBpmTextBox";
            this.NewBpmTextBox.ReadOnly = true;
            this.NewBpmTextBox.Size = new System.Drawing.Size(304, 17);
            this.NewBpmTextBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label5.Location = new System.Drawing.Point(34, 107);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "New BPM(s)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label6.Location = new System.Drawing.Point(94, 15);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "AR";
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.AutoSize = true;
            this.GenerateMapButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GenerateMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(126)))), ((int)(((byte)(219)))));
            this.GenerateMapButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GenerateMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateMapButton.Font = new System.Drawing.Font("Microsoft Tai Le", 11F, System.Drawing.FontStyle.Bold);
            this.GenerateMapButton.ForeColor = System.Drawing.Color.White;
            this.GenerateMapButton.Location = new System.Drawing.Point(10, 10);
            this.GenerateMapButton.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Size = new System.Drawing.Size(420, 40);
            this.GenerateMapButton.TabIndex = 0;
            this.GenerateMapButton.Text = "Generate Map";
            this.GenerateMapButton.UseVisualStyleBackColor = false;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // MiddlePanel
            // 
            this.MiddlePanel.Controls.Add(this.panel2);
            this.MiddlePanel.Controls.Add(this.Middle1Panel);
            this.MiddlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiddlePanel.Location = new System.Drawing.Point(0, 100);
            this.MiddlePanel.Margin = new System.Windows.Forms.Padding(10);
            this.MiddlePanel.Name = "MiddlePanel";
            this.MiddlePanel.Size = new System.Drawing.Size(440, 402);
            this.MiddlePanel.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Middle3Panel);
            this.panel2.Controls.Add(this.Middle2Panel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(122, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(318, 402);
            this.panel2.TabIndex = 11;
            // 
            // Middle3Panel
            // 
            this.Middle3Panel.Controls.Add(this.numericUpDown1);
            this.Middle3Panel.Controls.Add(this.optionSlider1);
            this.Middle3Panel.Controls.Add(this.OriginalBpmTextBox);
            this.Middle3Panel.Controls.Add(this.NewBpmTextBox);
            this.Middle3Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Middle3Panel.Location = new System.Drawing.Point(0, 77);
            this.Middle3Panel.Name = "Middle3Panel";
            this.Middle3Panel.Size = new System.Drawing.Size(318, 325);
            this.Middle3Panel.TabIndex = 7;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(90, 206);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 24);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // optionSlider1
            // 
            this.optionSlider1.BodyColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.optionSlider1.FillNipple = false;
            this.optionSlider1.Location = new System.Drawing.Point(29, 97);
            this.optionSlider1.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.optionSlider1.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.optionSlider1.Name = "optionSlider1";
            this.optionSlider1.NippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(84)))), ((int)(((byte)(149)))));
            this.optionSlider1.NippleDiameter = 10;
            this.optionSlider1.NippleExpandedDiameter = 15;
            this.optionSlider1.Size = new System.Drawing.Size(151, 40);
            this.optionSlider1.TabIndex = 9;
            this.optionSlider1.Text = "optionSlider1";
            this.optionSlider1.Thickness = 5;
            this.optionSlider1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Middle2Panel
            // 
            this.Middle2Panel.Controls.Add(this.SelectMapButton);
            this.Middle2Panel.Controls.Add(this.ARUpDown);
            this.Middle2Panel.Controls.Add(this.AutoDetectMapCheckbox);
            this.Middle2Panel.Controls.Add(this.BpmMultiplierUpDown);
            this.Middle2Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Middle2Panel.Location = new System.Drawing.Point(0, 0);
            this.Middle2Panel.Name = "Middle2Panel";
            this.Middle2Panel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Middle2Panel.Size = new System.Drawing.Size(318, 77);
            this.Middle2Panel.TabIndex = 7;
            // 
            // ARUpDown
            // 
            this.ARUpDown.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ARUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ARUpDown.DecimalPlaces = 1;
            this.ARUpDown.Font = new System.Drawing.Font("Microsoft New Tai Lue", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ARUpDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ARUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ARUpDown.Location = new System.Drawing.Point(4, 11);
            this.ARUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ARUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ARUpDown.Name = "ARUpDown";
            this.ARUpDown.Size = new System.Drawing.Size(77, 24);
            this.ARUpDown.TabIndex = 4;
            this.ARUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ARUpDown.ValueChanged += new System.EventHandler(this.ARUpDown_ValueChanged);
            // 
            // BpmMultiplierUpDown
            // 
            this.BpmMultiplierUpDown.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BpmMultiplierUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BpmMultiplierUpDown.DecimalPlaces = 2;
            this.BpmMultiplierUpDown.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BpmMultiplierUpDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BpmMultiplierUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BpmMultiplierUpDown.Location = new System.Drawing.Point(4, 41);
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
            this.BpmMultiplierUpDown.Size = new System.Drawing.Size(77, 24);
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
            this.Middle1Panel.Controls.Add(this.label4);
            this.Middle1Panel.Controls.Add(this.label6);
            this.Middle1Panel.Controls.Add(this.label5);
            this.Middle1Panel.Controls.Add(this.label2);
            this.Middle1Panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.Middle1Panel.Location = new System.Drawing.Point(0, 0);
            this.Middle1Panel.Name = "Middle1Panel";
            this.Middle1Panel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Middle1Panel.Size = new System.Drawing.Size(122, 402);
            this.Middle1Panel.TabIndex = 10;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.GenerateMapButton);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 502);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(10);
            this.BottomPanel.Size = new System.Drawing.Size(440, 60);
            this.BottomPanel.TabIndex = 11;
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.TopPanel.Controls.Add(this.SongLabel);
            this.TopPanel.Controls.Add(this.DiffLabel);
            this.TopPanel.Controls.Add(this.BgPanel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Padding = new System.Windows.Forms.Padding(10);
            this.TopPanel.Size = new System.Drawing.Size(440, 100);
            this.TopPanel.TabIndex = 12;
            // 
            // SongLabel
            // 
            this.SongLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.SongLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SongLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.SongLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(222)))), ((int)(((byte)(93)))));
            this.SongLabel.Location = new System.Drawing.Point(10, 78);
            this.SongLabel.Name = "SongLabel";
            this.SongLabel.ReadOnly = true;
            this.SongLabel.Size = new System.Drawing.Size(275, 14);
            this.SongLabel.TabIndex = 2;
            this.SongLabel.Text = "Artist - Title";
            // 
            // DiffLabel
            // 
            this.DiffLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.DiffLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DiffLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.DiffLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(222)))), ((int)(((byte)(93)))));
            this.DiffLabel.Location = new System.Drawing.Point(291, 78);
            this.DiffLabel.Name = "DiffLabel";
            this.DiffLabel.ReadOnly = true;
            this.DiffLabel.Size = new System.Drawing.Size(139, 14);
            this.DiffLabel.TabIndex = 2;
            this.DiffLabel.Text = "Extra";
            this.DiffLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BgPanel
            // 
            this.BgPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(96)))), ((int)(((byte)(104)))));
            this.BgPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BgPanel.Controls.Add(this.StaticGif);
            this.BgPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BgPanel.Location = new System.Drawing.Point(10, 10);
            this.BgPanel.Name = "BgPanel";
            this.BgPanel.Size = new System.Drawing.Size(420, 62);
            this.BgPanel.TabIndex = 0;
            // 
            // StaticGif
            // 
            this.StaticGif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StaticGif.ErrorImage = null;
            this.StaticGif.ImageLocation = "img\\static.gif";
            this.StaticGif.InitialImage = null;
            this.StaticGif.Location = new System.Drawing.Point(0, 0);
            this.StaticGif.Name = "StaticGif";
            this.StaticGif.Size = new System.Drawing.Size(420, 62);
            this.StaticGif.TabIndex = 0;
            this.StaticGif.TabStop = false;
            this.StaticGif.Visible = false;
            // 
            // mainFormBindingSource
            // 
            this.mainFormBindingSource.DataSource = typeof(osu_trainer.MainForm);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(440, 562);
            this.Controls.Add(this.MiddlePanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Beatmap Speed Changer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.MiddlePanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.Middle3Panel.ResumeLayout(false);
            this.Middle3Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.Middle2Panel.ResumeLayout(false);
            this.Middle2Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).EndInit();
            this.Middle1Panel.ResumeLayout(false);
            this.Middle1Panel.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.BgPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StaticGif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private NumericUpDownFix BpmMultiplierUpDown;
        private System.Windows.Forms.Button SelectMapButton;
        private System.Windows.Forms.CheckBox AutoDetectMapCheckbox;
        private System.Windows.Forms.Timer BeatmapUpdateTimer;
        private System.Windows.Forms.TextBox OriginalBpmTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NewBpmTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private NumericUpDownFix ARUpDown;
        private System.Windows.Forms.Button GenerateMapButton;
        private System.Windows.Forms.Panel MiddlePanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel Middle3Panel;
        private System.Windows.Forms.Panel Middle2Panel;
        private System.Windows.Forms.Panel Middle1Panel;
        private System.Windows.Forms.TextBox DiffLabel;
        private System.Windows.Forms.TextBox SongLabel;
        private System.Windows.Forms.Panel BgPanel;
        private System.Windows.Forms.PictureBox StaticGif;
        private System.Windows.Forms.BindingSource mainFormBindingSource;
        private OptionSlider optionSlider1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}