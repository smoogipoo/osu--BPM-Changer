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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentSongText = new System.Windows.Forms.TextBox();
            this.BpmMultiplierUpDown = new System.Windows.Forms.NumericUpDown();
            this.SelectMapButton = new System.Windows.Forms.Button();
            this.AutoDetectMapCheckbox = new System.Windows.Forms.CheckBox();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Beatmap:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "BPM Multiplier";
            // 
            // CurrentSongText
            // 
            this.CurrentSongText.Location = new System.Drawing.Point(105, 12);
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
            this.BpmMultiplierUpDown.Location = new System.Drawing.Point(134, 124);
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
            this.SelectMapButton.Location = new System.Drawing.Point(638, 47);
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
            this.AutoDetectMapCheckbox.Location = new System.Drawing.Point(473, 51);
            this.AutoDetectMapCheckbox.Name = "AutoDetectMapCheckbox";
            this.AutoDetectMapCheckbox.Size = new System.Drawing.Size(147, 17);
            this.AutoDetectMapCheckbox.TabIndex = 6;
            this.AutoDetectMapCheckbox.Text = "Automatically Detect Map";
            this.AutoDetectMapCheckbox.UseVisualStyleBackColor = true;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GenerateMapButton);
            this.Controls.Add(this.AutoDetectMapCheckbox);
            this.Controls.Add(this.SelectMapButton);
            this.Controls.Add(this.BpmMultiplierUpDown);
            this.Controls.Add(this.CurrentSongText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.BpmMultiplierUpDown)).EndInit();
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
    }
}