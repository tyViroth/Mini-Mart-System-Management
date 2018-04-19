namespace Mart.Forms
{
    partial class FormPreviewPhoto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreviewPhoto));
            this.pbPreviewImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreviewImage
            // 
            this.pbPreviewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPreviewImage.Location = new System.Drawing.Point(0, 0);
            this.pbPreviewImage.Name = "pbPreviewImage";
            this.pbPreviewImage.Size = new System.Drawing.Size(638, 389);
            this.pbPreviewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreviewImage.TabIndex = 0;
            this.pbPreviewImage.TabStop = false;
            // 
            // frmPreviewPhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Mart.Properties.Resources.preview_icon;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(638, 389);
            this.Controls.Add(this.pbPreviewImage);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPreviewPhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview Profile Picture";
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreviewImage;
    }
}