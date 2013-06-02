namespace BlackjackSimGui
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonRunSimulation = new System.Windows.Forms.Button();
            this.textBoxConfigPath = new System.Windows.Forms.TextBox();
            this.labelConfigPath = new System.Windows.Forms.Label();
            this.progressBarSimulation = new System.Windows.Forms.ProgressBar();
            this.labelStateAux = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelPercent = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSelectConfig = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRunSimulation
            // 
            this.buttonRunSimulation.Location = new System.Drawing.Point(133, 62);
            this.buttonRunSimulation.Name = "buttonRunSimulation";
            this.buttonRunSimulation.Size = new System.Drawing.Size(183, 23);
            this.buttonRunSimulation.TabIndex = 2;
            this.buttonRunSimulation.Text = "Run Simulation";
            this.buttonRunSimulation.UseVisualStyleBackColor = true;
            this.buttonRunSimulation.Click += new System.EventHandler(this.buttonRunSimulation_Click);
            // 
            // textBoxConfigPath
            // 
            this.textBoxConfigPath.Location = new System.Drawing.Point(133, 36);
            this.textBoxConfigPath.Name = "textBoxConfigPath";
            this.textBoxConfigPath.Size = new System.Drawing.Size(186, 20);
            this.textBoxConfigPath.TabIndex = 5;
            // 
            // labelConfigPath
            // 
            this.labelConfigPath.AutoSize = true;
            this.labelConfigPath.Location = new System.Drawing.Point(134, 20);
            this.labelConfigPath.Name = "labelConfigPath";
            this.labelConfigPath.Size = new System.Drawing.Size(97, 13);
            this.labelConfigPath.TabIndex = 6;
            this.labelConfigPath.Text = "Configuration Path:";
            // 
            // progressBarSimulation
            // 
            this.progressBarSimulation.Location = new System.Drawing.Point(133, 91);
            this.progressBarSimulation.Name = "progressBarSimulation";
            this.progressBarSimulation.Size = new System.Drawing.Size(183, 23);
            this.progressBarSimulation.TabIndex = 7;
            // 
            // labelStateAux
            // 
            this.labelStateAux.AutoSize = true;
            this.labelStateAux.Location = new System.Drawing.Point(134, 121);
            this.labelStateAux.Name = "labelStateAux";
            this.labelStateAux.Size = new System.Drawing.Size(35, 13);
            this.labelStateAux.TabIndex = 8;
            this.labelStateAux.Text = "State:";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.ForeColor = System.Drawing.Color.Green;
            this.labelState.Location = new System.Drawing.Point(167, 121);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(44, 13);
            this.labelState.TabIndex = 9;
            this.labelState.Text = "READY";
            // 
            // labelPercent
            // 
            this.labelPercent.AutoSize = true;
            this.labelPercent.Location = new System.Drawing.Point(322, 101);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(0, 13);
            this.labelPercent.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BlackjackSimGui.Properties.Resources.Gamble_Cards_icon1;
            this.pictureBox1.Location = new System.Drawing.Point(12, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(115, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSelectConfig
            // 
            this.buttonSelectConfig.BackgroundImage = global::BlackjackSimGui.Properties.Resources.file_open_icone_6882_1281;
            this.buttonSelectConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSelectConfig.Location = new System.Drawing.Point(325, 32);
            this.buttonSelectConfig.Name = "buttonSelectConfig";
            this.buttonSelectConfig.Size = new System.Drawing.Size(35, 27);
            this.buttonSelectConfig.TabIndex = 0;
            this.buttonSelectConfig.Tag = "Load Configuration";
            this.buttonSelectConfig.UseVisualStyleBackColor = true;
            this.buttonSelectConfig.Click += new System.EventHandler(this.buttonSelectConfig_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(375, 166);
            this.Controls.Add(this.labelPercent);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.labelStateAux);
            this.Controls.Add(this.progressBarSimulation);
            this.Controls.Add(this.labelConfigPath);
            this.Controls.Add(this.textBoxConfigPath);
            this.Controls.Add(this.buttonRunSimulation);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonSelectConfig);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "BlackjackSim";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectConfig;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonRunSimulation;
        private System.Windows.Forms.TextBox textBoxConfigPath;
        private System.Windows.Forms.Label labelConfigPath;
        private System.Windows.Forms.ProgressBar progressBarSimulation;
        private System.Windows.Forms.Label labelStateAux;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelPercent;
    }
}

