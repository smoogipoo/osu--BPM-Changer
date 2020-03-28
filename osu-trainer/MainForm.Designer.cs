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
            this.label2 = new System.Windows.Forms.Label();
            this.SelectMapButton = new System.Windows.Forms.Button();
            this.AutoDetectMapCheckbox = new System.Windows.Forms.CheckBox();
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.OriginalBpmTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewBpmTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ARUpDown = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BpmMultiplierUpDown = new osu_trainer.NumericUpDownFix();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label2.Location = new System.Drawing.Point(14, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "BPM Multiplier";
            // 
            // SelectMapButton
            // 
            this.SelectMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(249)))), ((int)(((byte)(246)))));
            this.SelectMapButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SelectMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectMapButton.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectMapButton.ForeColor = System.Drawing.Color.White;
            this.SelectMapButton.Location = new System.Drawing.Point(314, 41);
            this.SelectMapButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SelectMapButton.Name = "SelectMapButton";
            this.SelectMapButton.Size = new System.Drawing.Size(179, 29);
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
            this.AutoDetectMapCheckbox.Location = new System.Drawing.Point(314, 12);
            this.AutoDetectMapCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AutoDetectMapCheckbox.Name = "AutoDetectMapCheckbox";
            this.AutoDetectMapCheckbox.Size = new System.Drawing.Size(188, 20);
            this.AutoDetectMapCheckbox.TabIndex = 6;
            this.AutoDetectMapCheckbox.Text = "Automatically Detect Map";
            this.AutoDetectMapCheckbox.UseVisualStyleBackColor = true;
            this.AutoDetectMapCheckbox.CheckedChanged += new System.EventHandler(this.AutoDetectMapCheckbox_CheckedChanged);
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 500;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // OriginalBpmTextBox
            // 
            this.OriginalBpmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(27)))), ((int)(((byte)(47)))));
            this.OriginalBpmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OriginalBpmTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(134)))), ((int)(((byte)(144)))));
            this.OriginalBpmTextBox.Location = new System.Drawing.Point(128, 85);
            this.OriginalBpmTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OriginalBpmTextBox.Name = "OriginalBpmTextBox";
            this.OriginalBpmTextBox.ReadOnly = true;
            this.OriginalBpmTextBox.Size = new System.Drawing.Size(370, 17);
            this.OriginalBpmTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label4.Location = new System.Drawing.Point(9, 85);
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
            this.NewBpmTextBox.Location = new System.Drawing.Point(128, 116);
            this.NewBpmTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NewBpmTextBox.Name = "NewBpmTextBox";
            this.NewBpmTextBox.ReadOnly = true;
            this.NewBpmTextBox.Size = new System.Drawing.Size(370, 17);
            this.NewBpmTextBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(126)))), ((int)(((byte)(114)))));
            this.label5.Location = new System.Drawing.Point(32, 116);
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
            this.label6.Location = new System.Drawing.Point(90, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "AR";
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
            this.ARUpDown.Location = new System.Drawing.Point(128, 12);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.BpmMultiplierUpDown);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.NewBpmTextBox);
            this.panel1.Controls.Add(this.ARUpDown);
            this.panel1.Controls.Add(this.OriginalBpmTextBox);
            this.panel1.Controls.Add(this.SelectMapButton);
            this.panel1.Controls.Add(this.AutoDetectMapCheckbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 150);
            this.panel1.TabIndex = 10;
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
            this.BpmMultiplierUpDown.Location = new System.Drawing.Point(128, 47);
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
            this.BpmMultiplierUpDown.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BpmMultiplierUpDown_MouseWheel);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GenerateMapButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 151);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(515, 65);
            this.panel2.TabIndex = 11;
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
            this.GenerateMapButton.Size = new System.Drawing.Size(495, 45);
            this.GenerateMapButton.TabIndex = 0;
            this.GenerateMapButton.Text = "Generate Map";
            this.GenerateMapButton.UseVisualStyleBackColor = false;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(35)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(515, 216);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ARUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.NumericUpDown ARUpDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button GenerateMapButton;
    }
}