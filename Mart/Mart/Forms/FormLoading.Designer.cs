namespace Mart.Forms
{
    partial class FormLoading
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
            this.spinningCircles1 = new Mart.ControlClasses.SpinningCircles();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spinningCircles1
            // 
            this.spinningCircles1.BackColor = System.Drawing.Color.Transparent;
            this.spinningCircles1.Location = new System.Drawing.Point(10, 4);
            this.spinningCircles1.Name = "spinningCircles1";
            this.spinningCircles1.Size = new System.Drawing.Size(100, 100);
            this.spinningCircles1.TabIndex = 0;
            this.spinningCircles1.Text = "spinningCircles1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(36, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Loading...";
            // 
            // frmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 108);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spinningCircles1);
            this.Name = "frmLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmLoad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlClasses.SpinningCircles spinningCircles1;
        private System.Windows.Forms.Label label1;
    }
}