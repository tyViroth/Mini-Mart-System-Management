namespace Mart.Forms
{
    partial class FormInsertEmployee
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
            this.txtEmployeeID = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Gender = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chCreateAccount = new System.Windows.Forms.CheckBox();
            this.rdMale = new System.Windows.Forms.RadioButton();
            this.rdFemale = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelAcount = new System.Windows.Forms.Panel();
            this.txtRetypePassword = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pBanner = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbCloseDialog = new System.Windows.Forms.PictureBox();
            this.pbDeleteAccount = new System.Windows.Forms.PictureBox();
            this.pbEmployeePhoto = new System.Windows.Forms.PictureBox();
            this.panelAcount.SuspendLayout();
            this.pBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseDialog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Employee ID";
            // 
            // txtEmployeeID
            // 
            this.txtEmployeeID.BackColor = System.Drawing.SystemColors.Window;
            this.txtEmployeeID.Location = new System.Drawing.Point(118, 45);
            this.txtEmployeeID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmployeeID.Name = "txtEmployeeID";
            this.txtEmployeeID.ReadOnly = true;
            this.txtEmployeeID.Size = new System.Drawing.Size(140, 25);
            this.txtEmployeeID.TabIndex = 0;
            this.txtEmployeeID.Text = "Auto Number";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(118, 80);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFirstName.MaxLength = 100;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(140, 25);
            this.txtFirstName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "First Name";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(118, 115);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLastName.MaxLength = 100;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(140, 25);
            this.txtLastName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Last Name";
            // 
            // Gender
            // 
            this.Gender.AutoSize = true;
            this.Gender.Location = new System.Drawing.Point(20, 154);
            this.Gender.Name = "Gender";
            this.Gender.Size = new System.Drawing.Size(51, 17);
            this.Gender.TabIndex = 6;
            this.Gender.Text = "Gender";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "BirthDate";
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBirthDate.Location = new System.Drawing.Point(118, 186);
            this.dtpBirthDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(140, 25);
            this.dtpBirthDate.TabIndex = 5;
            // 
            // cboRole
            // 
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cboRole.Location = new System.Drawing.Point(118, 223);
            this.cboRole.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(140, 25);
            this.cboRole.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Role";
            // 
            // chCreateAccount
            // 
            this.chCreateAccount.AutoSize = true;
            this.chCreateAccount.Location = new System.Drawing.Point(23, 258);
            this.chCreateAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chCreateAccount.Name = "chCreateAccount";
            this.chCreateAccount.Size = new System.Drawing.Size(142, 21);
            this.chCreateAccount.TabIndex = 24;
            this.chCreateAccount.Text = "Create new Account";
            this.chCreateAccount.UseVisualStyleBackColor = true;
            // 
            // rdMale
            // 
            this.rdMale.AutoSize = true;
            this.rdMale.Location = new System.Drawing.Point(118, 152);
            this.rdMale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdMale.Name = "rdMale";
            this.rdMale.Size = new System.Drawing.Size(55, 21);
            this.rdMale.TabIndex = 3;
            this.rdMale.Text = "Male";
            this.rdMale.UseVisualStyleBackColor = true;
            // 
            // rdFemale
            // 
            this.rdFemale.AutoSize = true;
            this.rdFemale.Location = new System.Drawing.Point(181, 152);
            this.rdFemale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdFemale.Name = "rdFemale";
            this.rdFemale.Size = new System.Drawing.Size(67, 21);
            this.rdFemale.TabIndex = 4;
            this.rdFemale.Text = "Female";
            this.rdFemale.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.Location = new System.Drawing.Point(75, 401);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(170, 401);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panelAcount
            // 
            this.panelAcount.Controls.Add(this.pbDeleteAccount);
            this.panelAcount.Controls.Add(this.txtRetypePassword);
            this.panelAcount.Controls.Add(this.txtNewPassword);
            this.panelAcount.Controls.Add(this.label6);
            this.panelAcount.Controls.Add(this.label8);
            this.panelAcount.Controls.Add(this.txtUserName);
            this.panelAcount.Controls.Add(this.label7);
            this.panelAcount.Enabled = false;
            this.panelAcount.Location = new System.Drawing.Point(20, 281);
            this.panelAcount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelAcount.Name = "panelAcount";
            this.panelAcount.Size = new System.Drawing.Size(276, 114);
            this.panelAcount.TabIndex = 31;
            // 
            // txtRetypePassword
            // 
            this.txtRetypePassword.Location = new System.Drawing.Point(97, 77);
            this.txtRetypePassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRetypePassword.MaxLength = 16;
            this.txtRetypePassword.Name = "txtRetypePassword";
            this.txtRetypePassword.PasswordChar = '●';
            this.txtRetypePassword.Size = new System.Drawing.Size(140, 25);
            this.txtRetypePassword.TabIndex = 2;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(97, 43);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewPassword.MaxLength = 16;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.Size = new System.Drawing.Size(140, 25);
            this.txtNewPassword.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 17);
            this.label6.TabIndex = 24;
            this.label6.Text = "Username";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 17);
            this.label8.TabIndex = 28;
            this.label8.Text = "Re-type";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(97, 9);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(140, 25);
            this.txtUserName.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 17);
            this.label7.TabIndex = 26;
            this.label7.Text = "New password";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(290, 167);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 30);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // pBanner
            // 
            this.pBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(85)))), ((int)(((byte)(130)))));
            this.pBanner.Controls.Add(this.pictureBox1);
            this.pBanner.Controls.Add(this.lblTitle);
            this.pBanner.Controls.Add(this.pbMinimize);
            this.pBanner.Controls.Add(this.pbCloseDialog);
            this.pBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBanner.ForeColor = System.Drawing.Color.White;
            this.pBanner.Location = new System.Drawing.Point(0, 0);
            this.pBanner.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pBanner.Name = "pBanner";
            this.pBanner.Size = new System.Drawing.Size(393, 30);
            this.pBanner.TabIndex = 33;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(32, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(128, 21);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Insert Employee";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(85)))), ((int)(((byte)(130)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 442);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(393, 30);
            this.panel1.TabIndex = 34;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::Mart.Properties.Resources.Employee_48;
            this.pictureBox1.Location = new System.Drawing.Point(2, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMinimize.Image = global::Mart.Properties.Resources.Minimizing;
            this.pbMinimize.Location = new System.Drawing.Point(331, -1);
            this.pbMinimize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(32, 32);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimizeInsertDialog);
            // 
            // pbCloseDialog
            // 
            this.pbCloseDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCloseDialog.Image = global::Mart.Properties.Resources.Exit;
            this.pbCloseDialog.Location = new System.Drawing.Point(361, 0);
            this.pbCloseDialog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbCloseDialog.Name = "pbCloseDialog";
            this.pbCloseDialog.Size = new System.Drawing.Size(32, 32);
            this.pbCloseDialog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCloseDialog.TabIndex = 0;
            this.pbCloseDialog.TabStop = false;
            this.pbCloseDialog.Click += new System.EventHandler(this.pbCloseInsertDialog);
            // 
            // pbDeleteAccount
            // 
            this.pbDeleteAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDeleteAccount.Image = global::Mart.Properties.Resources.delete;
            this.pbDeleteAccount.Location = new System.Drawing.Point(245, 76);
            this.pbDeleteAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbDeleteAccount.Name = "pbDeleteAccount";
            this.pbDeleteAccount.Size = new System.Drawing.Size(26, 27);
            this.pbDeleteAccount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDeleteAccount.TabIndex = 3;
            this.pbDeleteAccount.TabStop = false;
            this.pbDeleteAccount.Visible = false;
            // 
            // pbEmployeePhoto
            // 
            this.pbEmployeePhoto.BackColor = System.Drawing.Color.Transparent;
            this.pbEmployeePhoto.Image = global::Mart.Properties.Resources.no;
            this.pbEmployeePhoto.Location = new System.Drawing.Point(285, 45);
            this.pbEmployeePhoto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbEmployeePhoto.Name = "pbEmployeePhoto";
            this.pbEmployeePhoto.Size = new System.Drawing.Size(96, 116);
            this.pbEmployeePhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEmployeePhoto.TabIndex = 14;
            this.pbEmployeePhoto.TabStop = false;
            // 
            // frmInsertEmployee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(393, 472);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pBanner);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.panelAcount);
            this.Controls.Add(this.rdFemale);
            this.Controls.Add(this.rdMale);
            this.Controls.Add(this.chCreateAccount);
            this.Controls.Add(this.pbEmployeePhoto);
            this.Controls.Add(this.Gender);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.dtpBirthDate);
            this.Controls.Add(this.cboRole);
            this.Controls.Add(this.txtEmployeeID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInsertEmployee";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmInsertEmployee_Load);
            this.panelAcount.ResumeLayout(false);
            this.panelAcount.PerformLayout();
            this.pBanner.ResumeLayout(false);
            this.pBanner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseDialog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmployeeID;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Gender;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbEmployeePhoto;
        private System.Windows.Forms.CheckBox chCreateAccount;
        private System.Windows.Forms.RadioButton rdMale;
        private System.Windows.Forms.RadioButton rdFemale;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelAcount;
        private System.Windows.Forms.TextBox txtRetypePassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel pBanner;
        private System.Windows.Forms.PictureBox pbCloseDialog;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbDeleteAccount;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}