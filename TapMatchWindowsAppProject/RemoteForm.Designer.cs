namespace Rovio.TapMatch.WindowsApp
{
    partial class RemoteForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.levelLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // levelLayoutPanel
            // 
            this.levelLayoutPanel.AutoSize = true;
            this.levelLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.levelLayoutPanel.BackColor = System.Drawing.Color.Black;
            this.levelLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.levelLayoutPanel.Location = new System.Drawing.Point(12, 151);
            this.levelLayoutPanel.MinimumSize = new System.Drawing.Size(40, 40);
            this.levelLayoutPanel.Name = "levelLayoutPanel";
            this.levelLayoutPanel.Size = new System.Drawing.Size(40, 40);
            this.levelLayoutPanel.TabIndex = 0;
            // 
            // RemoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(811, 671);
            this.Controls.Add(this.levelLayoutPanel);
            this.Name = "RemoteForm";
            this.Text = "Tap Match Remote";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel levelLayoutPanel;
    }
}
