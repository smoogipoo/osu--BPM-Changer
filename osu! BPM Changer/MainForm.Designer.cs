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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentSongText = new System.Windows.Forms.TextBox();
            this.BpmMultiplierUpDown = new System.Windows.Forms.NumericUpDown();
            this.SelectMapButton = new System.Windows.Forms.Button();
            this.AutoDetectMapCheckbox = new System.Windows.Forms.CheckBox();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.OsuFolderTextBox = new System.Windows.Forms.TextBox();
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.OriginalBpmTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewBpmTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ARUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Beatmap:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "BPM Multiplier";
            // 
            // CurrentSongText
            // 
            this.CurrentSongText.Location = new System.Drawing.Point(104, 32);
            this.CurrentSongText.Name = "CurrentSongText";
            this.CurrentSongText.ReadOnly = true;
            this.CurrentSongText.Size = new System.Drawing.Size(669, 20);
            this.CurrentSongText.TabIndex = 3;
            // 
            // BpmMultiplierUpDown
            // 
            this.BpmMultiplierUpDown.DecimalPlaces = 2;
            this.BpmMultiplierUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.BpmMultiplierUpDown.Location = new System.Drawing.Point(134, 126);
            this.BpmMultiplierUpDown.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BpmMultiplierUpDown.Name = "BpmMultiplierUpDown";
            this.BpmMultiplierUpDown.Size = new System.Drawing.Size(58, 20);
            this.BpmMultiplierUpDown.TabIndex = 4;
            this.BpmMultiplierUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BpmMultiplierUpDown.ValueChanged += new System.EventHandler(this.BpmMultiplierUpDown_ValueChanged);
            // 
            // SelectMapButton
            // 
            this.SelectMapButton.Location = new System.Drawing.Point(637, 67);
            this.SelectMapButton.Name = "SelectMapButton";
            this.SelectMapButton.Size = new System.Drawing.Size(134, 23);
            this.SelectMapButton.TabIndex = 5;
            this.SelectMapButton.Text = "Manually Select Map";
            this.SelectMapButton.UseVisualStyleBackColor = true;
            this.SelectMapButton.Click += new System.EventHandler(this.SelectMapButton_Click);
            // 
            // AutoDetectMapCheckbox
            // 
            this.AutoDetectMapCheckbox.AutoSize = true;
            this.AutoDetectMapCheckbox.Checked = true;
            this.AutoDetectMapCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoDetectMapCheckbox.Location = new System.Drawing.Point(472, 71);
            this.AutoDetectMapCheckbox.Name = "AutoDetectMapCheckbox";
            this.AutoDetectMapCheckbox.Size = new System.Drawing.Size(147, 17);
            this.AutoDetectMapCheckbox.TabIndex = 6;
            this.AutoDetectMapCheckbox.Text = "Automatically Detect Map";
            this.AutoDetectMapCheckbox.UseVisualStyleBackColor = true;
            this.AutoDetectMapCheckbox.CheckedChanged += new System.EventHandler(this.AutoDetectMapCheckbox_CheckedChanged);
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.Enabled = false;
            this.GenerateMapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateMapButton.Location = new System.Drawing.Point(82, 265);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Size = new System.Drawing.Size(607, 126);
            this.GenerateMapButton.TabIndex = 7;
            this.GenerateMapButton.Text = "Please select a beatmap first";
            this.GenerateMapButton.UseVisualStyleBackColor = true;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "osu! Folder";
            // 
            // OsuFolderTextBox
            // 
            this.OsuFolderTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.OsuFolderTextBox.Location = new System.Drawing.Point(104, 6);
            this.OsuFolderTextBox.Name = "OsuFolderTextBox";
            this.OsuFolderTextBox.ReadOnly = true;
            this.OsuFolderTextBox.Size = new System.Drawing.Size(667, 20);
            this.OsuFolderTextBox.TabIndex = 3;
            this.OsuFolderTextBox.Click += new System.EventHandler(this.OsuFolderTextBox_Click);
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 500;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // OriginalBpmTextBox
            // 
            this.OriginalBpmTextBox.Location = new System.Drawing.Point(299, 126);
            this.OriginalBpmTextBox.Name = "OriginalBpmTextBox";
            this.OriginalBpmTextBox.ReadOnly = true;
            this.OriginalBpmTextBox.Size = new System.Drawing.Size(273, 20);
            this.OriginalBpmTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(214, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Original BPM(s)";
            // 
            // NewBpmTextBox
            // 
            this.NewBpmTextBox.Location = new System.Drawing.Point(299, 152);
            this.NewBpmTextBox.Name = "NewBpmTextBox";
            this.NewBpmTextBox.ReadOnly = true;
            this.NewBpmTextBox.Size = new System.Drawing.Size(273, 20);
            this.NewBpmTextBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "New BPM(s)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "AR";
            // 
            // ARUpDown
            // 
            this.ARUpDown.BackColor = System.Drawing.SystemColors.Window;
            this.ARUpDown.DecimalPlaces = 1;
            this.ARUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ARUpDown.Location = new System.Drawing.Point(134, 189);
            this.ARUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ARUpDown.Name = "ARUpDown";
            this.ARUpDown.Size = new System.Drawing.Size(58, 20);
            this.ARUpDown.TabIndex = 4;
            this.ARUpDown.ValueChanged += new System.EventHandler(this.ARUpDown_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NewBpmTextBox);
            this.Controls.Add(this.OriginalBpmTextBox);
            this.Controls.Add(this.GenerateMapButton);
            this.Controls.Add(this.AutoDetectMapCheckbox);
            this.Controls.Add(this.SelectMapButton);
            this.Controls.Add(this.ARUpDown);
            this.Controls.Add(this.BpmMultiplierUpDown);
            this.Controls.Add(this.OsuFolderTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CurrentSongText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CurrentSongText;
        private System.Windows.Forms.NumericUpDown BpmMultiplierUpDown;
        private System.Windows.Forms.Button SelectMapButton;
        private System.Windows.Forms.CheckBox AutoDetectMapCheckbox;
        private System.Windows.Forms.Button GenerateMapButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox OsuFolderTextBox;
        private System.Windows.Forms.Timer BeatmapUpdateTimer;
        private System.Windows.Forms.TextBox OriginalBpmTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NewBpmTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ARUpDown;
    }
}