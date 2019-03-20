using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;


namespace WindowsFormsApp1
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            this.readGraphButton = new System.Windows.Forms.Button();
            this.addQueryFromManualInputButton = new System.Windows.Forms.Button();
            this.addQueryFromFileButton = new System.Windows.Forms.Button();
            this.discardGraphButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.hideAndSeekPictureBox = new System.Windows.Forms.PictureBox();
            this.graphPanel = new System.Windows.Forms.Panel();
            this.executeQueryButton = new System.Windows.Forms.Button();
            this.executeAllQueryAndSaveToFileButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.hideAndSeekPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // readGraphButton
            // 
            this.readGraphButton.Location = new System.Drawing.Point(1, 274);
            this.readGraphButton.Name = "readGraphButton";
            this.readGraphButton.Size = new System.Drawing.Size(447, 182);
            this.readGraphButton.TabIndex = 1;
            this.readGraphButton.Text = "Read Graph From File";
            this.readGraphButton.UseVisualStyleBackColor = true;
            this.readGraphButton.Click += new System.EventHandler(this._readGraphButtonClick);
            // 
            // addQueryFromManualInputButton
            // 
            this.addQueryFromManualInputButton.Location = new System.Drawing.Point(1, 274);
            this.addQueryFromManualInputButton.Name = "addQueryFromManualInputButton";
            this.addQueryFromManualInputButton.Size = new System.Drawing.Size(447, 35);
            this.addQueryFromManualInputButton.TabIndex = 2;
            this.addQueryFromManualInputButton.Text = "Add Query From Manual Input";
            this.addQueryFromManualInputButton.UseVisualStyleBackColor = true;
            this.addQueryFromManualInputButton.Visible = false;
            this.addQueryFromManualInputButton.Click += new System.EventHandler(this._addQueryFromManualInputButtonClick);
            // 
            // addQueryFromFileButton
            // 
            this.addQueryFromFileButton.Location = new System.Drawing.Point(1, 315);
            this.addQueryFromFileButton.Name = "addQueryFromFileButton";
            this.addQueryFromFileButton.Size = new System.Drawing.Size(447, 35);
            this.addQueryFromFileButton.TabIndex = 3;
            this.addQueryFromFileButton.Text = "Add Query From File";
            this.addQueryFromFileButton.UseVisualStyleBackColor = true;
            this.addQueryFromFileButton.Visible = false;
            this.addQueryFromFileButton.Click += new System.EventHandler(this._addQueryFromFileButtonClick);
            // 
            // discardGraphButton
            // 
            this.discardGraphButton.Location = new System.Drawing.Point(1, 421);
            this.discardGraphButton.Name = "discardGraphButton";
            this.discardGraphButton.Size = new System.Drawing.Size(447, 35);
            this.discardGraphButton.TabIndex = 4;
            this.discardGraphButton.Text = "Discard Graph";
            this.discardGraphButton.UseVisualStyleBackColor = true;
            this.discardGraphButton.Visible = false;
            this.discardGraphButton.Click += new System.EventHandler(this._discardGraphButtonClick);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(12, 459);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(102, 31);
            this.quitButton.TabIndex = 5;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this._quitButtonClick);
            // 
            // hideAndSeekPictureBox
            // 
            this.hideAndSeekPictureBox.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + "TeamPhoto.jpg";
            this.hideAndSeekPictureBox.Location = new System.Drawing.Point(1, 0);
            this.hideAndSeekPictureBox.Name = "hideAndSeekPictureBox";
            this.hideAndSeekPictureBox.Size = new System.Drawing.Size(447, 268);
            this.hideAndSeekPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.hideAndSeekPictureBox.TabIndex = 6;
            this.hideAndSeekPictureBox.TabStop = false;
            // 
            // graphPanel
            // 
            this.graphPanel.Location = new System.Drawing.Point(454, 0);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(942, 500);
            this.graphPanel.TabIndex = 7;
            // 
            // executeQueryButton
            // 
            this.executeQueryButton.Location = new System.Drawing.Point(1, 356);
            this.executeQueryButton.Name = "executeQueryButton";
            this.executeQueryButton.Size = new System.Drawing.Size(447, 30);
            this.executeQueryButton.TabIndex = 8;
            this.executeQueryButton.Text = "Execute Query";
            this.executeQueryButton.UseVisualStyleBackColor = true;
            this.executeQueryButton.Visible = false;
            this.executeQueryButton.Click += new System.EventHandler(this._executeQueryButtonClick);
            // 
            // executeAllQueryAndSaveToFileButton
            // 
            this.executeAllQueryAndSaveToFileButton.Location = new System.Drawing.Point(1, 392);
            this.executeAllQueryAndSaveToFileButton.Name = "executeAllQueryAndSaveToFileButton";
            this.executeAllQueryAndSaveToFileButton.Size = new System.Drawing.Size(447, 23);
            this.executeAllQueryAndSaveToFileButton.TabIndex = 9;
            this.executeAllQueryAndSaveToFileButton.Text = "Execute All Query and Save to FIle";
            this.executeAllQueryAndSaveToFileButton.UseVisualStyleBackColor = true;
            this.executeAllQueryAndSaveToFileButton.Visible = false;
            this.executeAllQueryAndSaveToFileButton.Click += new System.EventHandler(this._executeAllQueryAndSaveToFIleButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1395, 502);
            this.Controls.Add(this.readGraphButton);
            this.Controls.Add(this.executeAllQueryAndSaveToFileButton);
            this.Controls.Add(this.executeQueryButton);
            this.Controls.Add(this.addQueryFromFileButton);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.hideAndSeekPictureBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.discardGraphButton);
            this.Controls.Add(this.addQueryFromManualInputButton);
            this.Name = "MainForm";
            this.Text = "DFS Hide and Seek";
            ((System.ComponentModel.ISupportInitialize)(this.hideAndSeekPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button readGraphButton;
        private System.Windows.Forms.Button addQueryFromManualInputButton;
        private System.Windows.Forms.Button addQueryFromFileButton;
        private System.Windows.Forms.Button discardGraphButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.PictureBox hideAndSeekPictureBox;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.Button executeQueryButton;
        private Button executeAllQueryAndSaveToFileButton;
    }
}

