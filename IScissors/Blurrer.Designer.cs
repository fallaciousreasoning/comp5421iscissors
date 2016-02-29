namespace IScissors
{
    partial class Blurrer
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
            this.okayButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.blurRadiusSlider = new System.Windows.Forms.TrackBar();
            this.blurRadiusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.blurRadiusSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // okayButton
            // 
            this.okayButton.Location = new System.Drawing.Point(175, 58);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 0;
            this.okayButton.Text = "Okay";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(94, 58);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // blurRadiusSlider
            // 
            this.blurRadiusSlider.Location = new System.Drawing.Point(12, 12);
            this.blurRadiusSlider.Name = "blurRadiusSlider";
            this.blurRadiusSlider.Size = new System.Drawing.Size(222, 45);
            this.blurRadiusSlider.TabIndex = 2;
            this.blurRadiusSlider.Scroll += new System.EventHandler(this.blurRadiusSlider_Scroll);
            // 
            // blurRadiusLabel
            // 
            this.blurRadiusLabel.AutoSize = true;
            this.blurRadiusLabel.Location = new System.Drawing.Point(237, 21);
            this.blurRadiusLabel.Name = "blurRadiusLabel";
            this.blurRadiusLabel.Size = new System.Drawing.Size(13, 13);
            this.blurRadiusLabel.TabIndex = 3;
            this.blurRadiusLabel.Text = "1";
            // 
            // Blurrer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 90);
            this.Controls.Add(this.blurRadiusLabel);
            this.Controls.Add(this.blurRadiusSlider);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okayButton);
            this.Name = "Blurrer";
            this.Text = "Blurrer";
            ((System.ComponentModel.ISupportInitialize)(this.blurRadiusSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okayButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TrackBar blurRadiusSlider;
        private System.Windows.Forms.Label blurRadiusLabel;
    }
}