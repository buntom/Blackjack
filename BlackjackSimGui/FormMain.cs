using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlackjackSim.External;
using System.Threading;

namespace BlackjackSimGui
{
    public partial class FormMain : Form
    {
        private BackgroundWorker backgroundWorkerSimulation;

        public FormMain()
        {
            InitializeComponent();

            backgroundWorkerSimulation = new BackgroundWorker();
            backgroundWorkerSimulation.DoWork += backgroundWorkerSimulation_DoWork;
            backgroundWorkerSimulation.ProgressChanged += backgroundWorkerSimulation_ProgressChanged;
            backgroundWorkerSimulation.RunWorkerCompleted += backgroundWorkerSimulation_RunWorkerCompleted;
            backgroundWorkerSimulation.WorkerReportsProgress = true;
            backgroundWorkerSimulation.WorkerSupportsCancellation = true;            
        }

        private void buttonSelectConfig_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxConfigPath.Text = openFileDialog.FileName;                
            }
        }

        public void ProgressBarSetValue(int value)
        {            
            backgroundWorkerSimulation.ReportProgress(value);            
        }

        private void ResetState()
        {
            progressBarSimulation.Value = 0;
            labelPercent.Text = "";
            labelState.Text = "READY";
            labelState.ForeColor = System.Drawing.Color.Green;            
        }
                
        private void buttonRunSimulation_Click(object sender, EventArgs e)
        {
            if (textBoxConfigPath.Text == "")
            {
                MessageBox.Show("Configuration file has not been selected yet!", "BlackjackSim: Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!backgroundWorkerSimulation.IsBusy)
            {
                buttonRunSimulation.Enabled = false;                
                labelState.Text = "SIMULATING";
                labelState.ForeColor = System.Drawing.Color.Red;
                
                backgroundWorkerSimulation.RunWorkerAsync();
            }
        }

        private void backgroundWorkerSimulation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {                
                var runner = new BlackjackSim.Runner(textBoxConfigPath.Text);
                
                runner.Run(ProgressBarSetValue);

                MessageBox.Show("Simulation finished!", "BlackjackSim: Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BlackjackSim: Exception occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void backgroundWorkerSimulation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarSimulation.Value = e.ProgressPercentage;
            progressBarSimulation.Update();
            labelPercent.Text = String.Format("{0}%", e.ProgressPercentage);
            labelPercent.Update();
        }

        private void backgroundWorkerSimulation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            ResetState();
            buttonRunSimulation.Enabled = true;
        }

        [Obsolete]
        private void buttonRunSimulation_Click_bak(object sender, EventArgs e)
        {
            if (textBoxConfigPath.Text == "")
            {
                MessageBox.Show("Configuration file has not been selected yet!", "BlackjackSim: Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var runner = new BlackjackSim.Runner(textBoxConfigPath.Text);
                labelState.Text = "SIMULATING";
                labelState.ForeColor = System.Drawing.Color.Red;

                runner.Run(ProgressBarSetValue);

                MessageBox.Show("Simulation finished!", "BlackjackSim: Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BlackjackSim: Exception occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
