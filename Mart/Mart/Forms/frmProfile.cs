using Mart.InstanceClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Mart.ControlClasses;
using Mart.Intefaces;
using System.Data.SqlClient;

namespace Mart.Forms
{
    public partial class frmProfile : Form,IMessageType
    {
        private Point mouseLocation;
        private string imagePath = "";

        private bool firstNameChanged = false;
        private bool lastNameChanged = false;
        private bool genderChanged = false;
        private bool birthDateChanged = false;
        private bool passwordChanged = false;
        private bool usernameChanged = false;

        private Employee empLogin;
        public frmProfile(Employee empLogin):this()
        {
            this.empLogin = empLogin;          
        }

        public frmProfile()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.CenterToScreen();

            RegisterEvents();            
        }

        private void LoadDatatoControls()
        {
            if (empLogin != null)
            {
                if (empLogin.Roles.Name == "Admin")
                {
                    /* Allow to Edit Their USERNAME */                
                    txtUsername.ReadOnly = false;                    
                }

                txtEmployeeID.Text = empLogin.ID.ToString();
                txtFirstName.Text = empLogin.FirstName.Trim();
                txtLastName.Text = empLogin.LastName.Trim();
                if(empLogin.Gender.CompareTo("Male")==0)
                    rdMale.Checked = true;
                else rdFemale.Checked = true;
                dtpBirthDate.Value = empLogin.BirthDate;
                txtRole.Text = empLogin.Roles.Name;

                txtUsername.Text = empLogin.UserName;

                /*Image*/
                pbEmployeePhoto.Image = Image.FromStream(new MemoryStream(empLogin.Photo));
            }
        }

        private void RegisterEvents()
        {            
            this.Load += frmProfile_Load;

            /*Picture Box*/
            pbEmployeePhoto.MouseHover += pbEmployeePhoto_MouseHover;
            pbEmployeePhoto.Click += pbEmployeePhoto_Click;

            pbCloseDialog.Click += pbCloseDialog_Click;
            pbCloseDialog.MouseHover += pbCloseDialog_MouseHover;

            pBanner.MouseMove += DoMove;
            pBanner.MouseDown += DoDown;            

            lblTitle.MouseMove += DoMove;
            lblTitle.MouseDown += DoDown;

                        
            /* Button */
            btnBrowse.Click += btnBrowse_Click;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            /* Text Box */
            txtFirstName.KeyPress += AllowText;
            txtLastName.KeyPress += AllowText;

            txtFirstName.TextChanged += DoTextChanged;
            txtLastName.TextChanged += DoTextChanged;

            txtUsername.TextChanged += txtUsername_TextChanged;

            txtOldPassword.TextChanged += DoCheckOldPassword;
            txtNewPassword.TextChanged += DoCheckChangePassword;
          
            /*Radio Button*/
            rdFemale.Click += DoRDClicked;
            rdMale.Click += DoRDClicked;

            /*DateTimePicker*/
            dtpBirthDate.ValueChanged += dtpBirthDate_ValueChanged;
        }

        void pbEmployeePhoto_Click(object sender, EventArgs e)
        {
            if (empLogin != null)
            {
                byte[] pho = null;                
                if (imagePath =="")
                {
                    pho = empLogin.Photo;
                }
                else
                {
                    pho = File.ReadAllBytes(imagePath);                    
                }
                frmPreviewPhoto preview = new frmPreviewPhoto(pho);
                preview.ShowDialog();
            }
        }

        void pbEmployeePhoto_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip((PictureBox)sender,"Click to Preview");
        }

        void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() != empLogin.UserName)
                usernameChanged = true;
            else usernameChanged = false;

            CheckDataToEnableButton();            
        }

        private void DoCheckChangePassword(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Trim() == "") passwordChanged = false;            
            else passwordChanged = true;
            CheckDataToEnableButton();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            LoadDatatoControls();
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            ClearStatusEditOnData();

            /*Clear Password textbox*/
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtRetypePassword.Clear();
        }

        void dtpBirthDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBirthDate.Value != empLogin.BirthDate)
                birthDateChanged = true;
            else birthDateChanged = false;
            CheckDataToEnableButton();
        }

        private void DoRDClicked(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Text != empLogin.Gender)
            {
                genderChanged = true;
            }
            else genderChanged = false;
            CheckDataToEnableButton();
        }

        private void CheckDataToEnableButton()
        {            
            if (!firstNameChanged && !lastNameChanged && !genderChanged && !birthDateChanged && !passwordChanged && !usernameChanged && imagePath == "")
            {
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnCancel.Enabled = true;            
            } 
        }

        private void DoTextChanged(object sender, EventArgs e)
        {
            if(sender == txtFirstName){
                if (txtFirstName.Text.Trim() != empLogin.FirstName)
                    firstNameChanged = true;                
                else 
                    firstNameChanged = false;
            }else if(sender == txtLastName){
                if (txtLastName.Text.Trim() != empLogin.LastName) 
                    lastNameChanged = true;
                else 
                    lastNameChanged = false;
            }
            CheckDataToEnableButton();
        }

        private void DoCheckOldPassword(object sender, EventArgs e)
        {
            /*if it is empty set automatically FAIL compare and return*/
            if (txtOldPassword.Text.Trim() == "")
            {
                txtNewPassword.Clear();
                txtRetypePassword.Clear();

                txtNewPassword.Enabled = false;
                txtRetypePassword.Enabled = false;                
                return; 
            }

            if (ConvertHashCode.CompareDbHashWithInputHash(empLogin.Password, txtOldPassword.Text.Trim()))
            {
                txtNewPassword.Enabled = true;
                txtRetypePassword.Enabled = true;               
            }
            else
            {
                txtNewPassword.Clear();
                txtRetypePassword.Clear();

                txtNewPassword.Enabled = false;
                txtRetypePassword.Enabled = false;                
            }          
        }

        void btnSave_Click(object sender, EventArgs e)
        {            
            if (txtFirstName.Text.Trim() == "")
            {
                MessageError("Enter First Name!","Require");
                return;
            }else if(txtLastName.Text.Trim() == ""){
                MessageError("Enter Last Name!","Require");
                return;
            }else if(txtUsername.Text.Trim() == ""){
                MessageError("Enter Username!", "Require");
                return;
            }
            else if (Controller.IsExistUsername(txtUsername.Text.Trim()) && txtUsername.Text.Trim().CompareTo(empLogin.UserName) !=0 )
            {
                MessageError("This Username is already exist!", "Require");
                return;                        
            }
            else if(txtNewPassword.Enabled && txtNewPassword.Text.Trim() != ""){
                if (txtNewPassword.Text.Trim().CompareTo(txtRetypePassword.Text.Trim())!=0)
                {
                    MessageError("Password is not match!", "Require");
                    return;
                }
            }

            byte[] pho = null;
            string newPass = txtNewPassword.Text.Trim();
            string password = ( newPass != "") ? ConvertHashCode.ConvertPasswordToHashCode(newPass) : empLogin.Password;
            if (imagePath == "") pho = empLogin.Photo; /*User doesn't browse any photo*/
            else{
                pho = File.ReadAllBytes(imagePath);
            }

            empLogin.SetEmployeeData(
                         empLogin.ID,
                         txtFirstName.Text.Trim(),
                         txtLastName.Text.Trim(),
                         (rdMale.Checked) ? "Male" : "Female",
                         dtpBirthDate.Value,
                         txtUsername.Text.Trim(),
                         password,
                         empLogin.Roles,
                         true,
                         pho
                       );
            if (UpdateProfile(empLogin))
            {
                MessageSuccess("Profile was updated.","Update Profile");
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                ClearStatusEditOnData();
            }
            else
            {
                MessageError("Profile was not updated.", "Update Profile");
            }                
        }

        private void ClearStatusEditOnData()
        {
            /* To Clear all status that user has edit on any field or data */
            imagePath = "";
            firstNameChanged = false;
            lastNameChanged = false;
            genderChanged = false;
            birthDateChanged = false;
            passwordChanged = false;
            usernameChanged = false;
        }

        private void AllowText(object sender, KeyPressEventArgs e)
        {
            Input.InputString((TextBox)sender,e);
        }

        void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select image";
            ofd.Filter = "*.JPG, *.PNG| *.JPG; *.PNG;";            

            DialogResult dialog = ofd.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                imagePath = ofd.FileName.ToString();
                pbEmployeePhoto.Image = Image.FromFile(imagePath);
            }
            CheckDataToEnableButton();
        }
       
        void pbCloseDialog_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip((PictureBox)sender,"Close");
        }

        private void DoDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X,-e.Y);
        }

        private void DoMove(object sender, MouseEventArgs e)
        {                
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X,mouseLocation.Y);
                Location = mousePose;
            }
        }

        void pbCloseDialog_Click(object sender, EventArgs e)
        {
            if(btnSave.Enabled){
                DialogResult dialog = MessageVerify("Do you wan to save?", "Update Profile");
                if (dialog == DialogResult.Yes)
                {
                    btnSave_Click(btnSave,e);
                    this.Close();
                }
                else if(dialog == DialogResult.No)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }                     
        }

        void frmProfile_Load(object sender, EventArgs e)
        {
            LoadDatatoControls();
        }

        public void MessageSuccess(string des, string title)
        {
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public void MessageError(string des, string title)
        {
            MessageBox.Show(des, title, MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        public void MessageWarning(string des, string title)
        {
            MessageBox.Show(des, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);        
        }

        public DialogResult MessageVerify(string des, string title)
        {
            return MessageBox.Show(des, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);        
        }

        public bool UpdateProfile(Employee emp)
        {
            SqlConnection con = Connection.getConnection();
            SqlCommand cmd = null;

            bool success = false;
            try
            {
                con.Open();
                cmd = new SqlCommand("UPDATE Employee SET lastName = @ln, firstName = @fn, gender = @g, birthDate = @bd, username = @un, password = @pw, roleID = @roleID, status = @s, photo = @pho WHERE empID = @empID", con);
                cmd.Parameters.AddWithValue("@ln", emp.LastName);
                cmd.Parameters.AddWithValue("@fn", emp.FirstName);
                cmd.Parameters.AddWithValue("@g", emp.Gender);
                cmd.Parameters.AddWithValue("@bd", emp.BirthDate);
                cmd.Parameters.AddWithValue("@un", emp.UserName);
                cmd.Parameters.AddWithValue("@pw", emp.Password);
                cmd.Parameters.AddWithValue("@roleID", emp.Roles.ID);
                cmd.Parameters.AddWithValue("@s", emp.Status);
                cmd.Parameters.AddWithValue("@pho", emp.Photo);
                cmd.Parameters.AddWithValue("@empID", emp.ID);
                if (cmd.ExecuteNonQuery() > 0) success = true;
            }
            catch (Exception e)
            {
                success = false;
                MessageError(e.Message, "Error Update Profile");
            }
            finally
            {                
                cmd.Dispose();
                con.Close();                
            }
            return success;
        }

    }
}
