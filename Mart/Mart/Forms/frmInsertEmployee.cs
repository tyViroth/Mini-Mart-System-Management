using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Mart.InstanceClasses;
using Mart.ControlClasses;
namespace Mart.Forms
{
    public partial class frmInsertEmployee : Form
    {
        Point mouseLocation;
        private Employee emp;
        private string imagePath = "";
        private string username;
        private string password;
        private bool comboboxChanged;
        private bool textBoxChanged;
        private bool radioButtonChanged;
        private bool datetimepickerChanged;
        private bool imagePathChanged;

        /* This constructor is called when Update Employee */
        public frmInsertEmployee(Employee emp):this()
        {
            this.emp = emp;
            username = emp.UserName;
            password = emp.Password;
        }

        /* This constructor is called when Add new Employee */
        public frmInsertEmployee()
        {
            InitializeComponent();
            RegisterEventMove();              
        }

        private void frmInsertEmployee_Load(object sender, EventArgs e)
        {
            Controller.FillComboBoxValue(cboRole, "roleID", "roleName", "SetRoleToComboBox");
            if (emp != null)
            {
                lblTitle.Text = "Update Employee";
                SetDataToControls();
                btnCancel.Enabled = false;
                btnSave.Enabled = false;
                if (emp.UserName == "") chCreateAccount.Text = "Create new Account";
                else chCreateAccount.Text = "Update Username and Password";
            }
            else
            {
                cboRole.SelectedIndex = -1;
            }
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";
            
            this.CenterToParent();                 
        }

        private void SetDataToControls()
        {
            /* Set recovery All Data */
           if (emp ==null ) return;
           username = emp.UserName;
           password = emp.Password;

           
           txtEmployeeID.Text = emp.ID.ToString();
           txtFirstName.Text = emp.FirstName.ToString();
           txtLastName.Text = emp.LastName.ToString();
           if (emp.Gender == "Male") rdMale.Checked = true;
           else rdFemale.Checked = true;
           cboRole.SelectedValue = emp.Roles.ID;
                      
           txtUserName.Text = emp.UserName;
           txtNewPassword.Text = emp.Password;
           txtRetypePassword.Text = emp.Password;
           dtpBirthDate.Value = emp.BirthDate;

           MemoryStream ms = new MemoryStream(emp.Photo);
           Image img = Image.FromStream(ms);
           pbEmployeePhoto.Image = img;           

        }

        private void RegisterEventMove()
        {
            /* Set Event Update Time */
            txtFirstName.TextChanged += DoTextBoxValueChanged;
            txtLastName.TextChanged += DoTextBoxValueChanged;
            txtUserName.TextChanged += DoTextBoxValueChanged;
            txtNewPassword.TextChanged += DoTextBoxValueChanged;

            rdMale.CheckedChanged += DoRadioButtonValueChanged;
            rdFemale.CheckedChanged += DoRadioButtonValueChanged;
            dtpBirthDate.ValueChanged += DoDateTimePickerValueChanged;

            cboRole.SelectedValueChanged += DoComboBoxValueChange;

            chCreateAccount.CheckedChanged += chCreateAccount_CheckedChanged;
            pbDeleteAccount.Click += DoDeleteAccount;
            pbDeleteAccount.MouseHover += DoHoverShowToolTip;
            pbCloseDialog.MouseHover += DoHoverShowToolTip;
            pbMinimize.MouseHover += DoHoverShowToolTip;

            pbEmployeePhoto.Click += pbEmployeePhoto_Click;
            pbEmployeePhoto.MouseHover += pbEmployeePhoto_MouseHover;

            pBanner.MouseDown += DoMouseDown;            
            pBanner.MouseMove += DoMouseMove;

            lblTitle.MouseDown += DoMouseDown;
            lblTitle.MouseMove += DoMouseMove;
           
            btnSave.Click += DoClick;
            btnBrowse.Click += DoClick;
            btnCancel.Click += DoClick;

            /* Key Press TextBox Event */
            txtFirstName.KeyPress += AllowTextOnly;
            txtLastName.KeyPress += AllowTextOnly;           
        }

        void pbEmployeePhoto_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip((PictureBox)sender,"Click to Preview");
        }

        void pbEmployeePhoto_Click(object sender, EventArgs e)
        {
            byte[] photo = null;
            /*Insert new Employee*/
            if (emp == null)
            {
                if (imagePath == "")
                {
                    object O = Mart.Properties.Resources.ResourceManager.GetObject("no");
                    Image img = (Image)O;
                    var ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    photo = ms.ToArray();
                }
                else
                {
                    photo = File.ReadAllBytes(imagePath);
                }
            }
            /*Update Employee*/
            else
            {
                if (imagePath == "")
                {
                    photo = emp.Photo;
                }
                else
                {
                    photo = File.ReadAllBytes(imagePath);
                }
            }
            frmPreviewPhoto preview = new frmPreviewPhoto(photo);
            preview.ShowDialog();
        }

        private void DoMouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X,-e.Y);
        }

        private void DoComboBoxValueChange(object sender, EventArgs e)
        {
            if (cboRole.SelectedIndex == -1 || emp == null) return;
            try
            {
                if ((int)cboRole.SelectedValue != emp.Roles.ID)
                {
                    EnabledButtonSaveCancel(true);
                    comboboxChanged = true;
                }
                else
                {                    
                    comboboxChanged = false;
                    if (!textBoxChanged && !datetimepickerChanged && !radioButtonChanged && !imagePathChanged)
                    {
                        EnabledButtonSaveCancel(false);   
                    }                    
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void DoDateTimePickerValueChanged(object sender, EventArgs e)
        {
            if (emp == null) return;
            if (dtpBirthDate.Value != emp.BirthDate)
            {
                datetimepickerChanged = true;
                EnabledButtonSaveCancel(true); 
            }
            else
            {
                datetimepickerChanged = false;

                if (!textBoxChanged && !comboboxChanged && !radioButtonChanged)
                {
                    EnabledButtonSaveCancel(false);
                }                                
            }
        }

        private void DoRadioButtonValueChanged(object sender, EventArgs e)
        {
            if (emp == null) return;
            if (rdFemale.Checked && rdFemale.Text.Trim() != emp.Gender.Trim() || rdMale.Checked && rdMale.Text.Trim() != emp.Gender.Trim())
            {
                EnabledButtonSaveCancel(true);
                radioButtonChanged = true;
            }
            else
            {
                radioButtonChanged = false;
                if (!textBoxChanged && !datetimepickerChanged && !comboboxChanged && !imagePathChanged)
                {
                    EnabledButtonSaveCancel(false); 
                }                         
            }
        }

        private void DoTextBoxValueChanged(object sender, EventArgs e)
        {           
            if (emp == null) return;
            if (txtFirstName.Text != emp.FirstName || txtLastName.Text.Trim() != emp.LastName || txtUserName.Text.Trim() != username || txtNewPassword.Text.Trim() != password)
            {
               textBoxChanged = true;
               EnabledButtonSaveCancel(true);
            }
            else
            {
                textBoxChanged = false;
                if (!comboboxChanged && !radioButtonChanged && !datetimepickerChanged && !imagePathChanged)
                {
                    EnabledButtonSaveCancel(false); 
                }                           
            }       
        }

        private void DoHoverShowToolTip(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            if (sender == pbDeleteAccount)
            {
                tt.SetToolTip((PictureBox)sender, "Remove account");    
            }else if(sender == pbCloseDialog){
                tt.SetToolTip((PictureBox)sender, "Close");
            }else if (sender == pbMinimize)
            {
                tt.SetToolTip((PictureBox)sender, "Minimize");                
            }            
        }

        private void DoDeleteAccount(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to remove this account?","Remove Account",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                username = "";
                password = "";

                txtUserName.Clear();
                txtNewPassword.Clear();
                txtRetypePassword.Clear();
                chCreateAccount.Checked = false;
                chCreateAccount.Text = "Create new Account";

                textBoxChanged = true;
                EnabledButtonSaveCancel(true);
            }
        }

        private void AllowTextOnly(object sender, KeyPressEventArgs e)
        {
            Input.InputString((TextBox)sender,e);
        }

        private void DoClick(object sender, EventArgs e)
        {            
            if (sender == btnSave)
            {
                GetValueFromControls();
                if (emp == null) return;
                if (emp.UserName != "" ) chCreateAccount.Text = "Update Username and Password";                  
            }
            else if (sender == btnCancel)
            {
                if (emp != null)
                {
                    SetDataToControls();
                    SetBooleanUnChanged();
                }
                else
                {
                    ClearControls();
                }
            }else if(sender == btnBrowse)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select picture";
                ofd.Filter = "Image *PNG, *JPG | *.PNG; *.JPG";
                DialogResult dialogResult = ofd.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {                    
                    imagePath = ofd.FileName;
                    pbEmployeePhoto.Image = Image.FromFile(imagePath);

                    if (emp != null)
                    {
                        imagePathChanged = true; /* To modify that image is changed */                        
                        EnabledButtonSaveCancel(true);
                    }
                }
            }
        }

        private void EnabledButtonSaveCancel(bool p)
        {
            btnSave.Enabled = p;
            btnCancel.Enabled = p;
        }

        private void ClearControls()
        {
            txtEmployeeID.Text = "Auto Number";
            txtFirstName.Clear();
            txtLastName.Clear();
            rdMale.Checked = false;
            rdFemale.Checked = false;
            dtpBirthDate.Value = DateTime.Now;
            cboRole.SelectedIndex = -1;
            chCreateAccount.Checked = false;
            txtUserName.Clear();
            txtNewPassword.Clear();
            txtRetypePassword.Clear();
            pbEmployeePhoto.Image = Mart.Properties.Resources.no;
            imagePath = "";
        }

        private bool GetValueFromControls()
        {
            byte[] pho;
            if (txtFirstName.Text.Trim() == "")
            {
                RequiredMessage("Enter First name");
                return false;
            }else if(txtLastName.Text.Trim() == ""){
                RequiredMessage("Enter Last name");
                return false;
            }
            else if (cboRole.SelectedIndex == -1)
            {
                RequiredMessage("Select role to Employee");
                return false;
            }
            else if(!rdFemale.Checked && !rdMale.Checked){
                RequiredMessage("Select Gender");
                return false;
            }
            else if (txtUserName.Text.Trim().CompareTo(username) != 0 && Controller.IsExistUsername(txtUserName.Text.Trim()))
            {
                RequiredMessage("This username is already exist!");
                return false;                    
            }
            if (chCreateAccount.Checked == true)
            {
                if (txtUserName.Text.Trim() == ""){
                    RequiredMessage("Enter Username");
                    return false;
                }else if(txtNewPassword.Text.Trim() == ""){
                    RequiredMessage("Enter new Password");
                    return false;
                }
                else if(txtNewPassword.Text.Trim() != txtRetypePassword.Text.Trim()){
                    RequiredMessage("Password is not match!");
                    return false;
                }
            }            
                      
            if (emp == null)  /* Add new Employee */
            {
                if (imagePath == "") /* User didn't browse new Picture */
                {
                    object O = Mart.Properties.Resources.ResourceManager.GetObject("no");
                    Image img = (Image)O;
                    var ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    pho = ms.ToArray();
                }
                else pho = File.ReadAllBytes(imagePath); /* User browsed new Picture */
                
                int id = Controller.GetLastAutoIncrement("Employee") + 1;

                Employee.CreatedInstance(
                           id,
                           txtFirstName.Text.Trim(),
                           txtLastName.Text.Trim(),
                           (rdMale.Checked)?"Male":"Female",
                           dtpBirthDate.Value,
                           txtUserName.Text.Trim(),
                           ConvertHashCode.ConvertPasswordToHashCode(txtNewPassword.Text.Trim()),
                           new Role((int)cboRole.SelectedValue, cboRole.Text.Trim()),
                           true,
                           pho
                          );
                ClearControls();
            }
            else /* Update Employee */
            {
                if (imagePath == "") pho = emp.Photo;  /* User didn't browse new Picture */
                else pho = File.ReadAllBytes(imagePath); /* User browsed new Picture */
                username = txtUserName.Text;
                /*Check condition whether user change password or not*/
                if (txtNewPassword.Text.Trim() == "") password = "";
                else password = (emp.Password.Trim().CompareTo(txtNewPassword.Text.Trim()) == 0) ? emp.Password : ConvertHashCode.ConvertPasswordToHashCode(txtNewPassword.Text.Trim());
                emp.SetEmployeeData(
                          emp.ID,
                          txtFirstName.Text.Trim(),
                          txtLastName.Text.Trim(),
                          (rdMale.Checked) ? "Male" : "Female",
                          dtpBirthDate.Value,
                          username,
                          password,
                          new Role((int)cboRole.SelectedValue, cboRole.Text.Trim()),
                          true,
                          pho
                        );
                if (username != "") pbDeleteAccount.Visible = true;

                /* Set Variable to Status unchanged any DATA after Saving (Updated) */
                SetBooleanUnChanged();
            }
            return true;
        }

        private void SetBooleanUnChanged()
        {
            EnabledButtonSaveCancel(false);
            textBoxChanged = false;
            radioButtonChanged = false;
            comboboxChanged = false;
            datetimepickerChanged = false;
            imagePathChanged = false;
        }

        private void RequiredMessage(string des)
        {
            MessageBox.Show(des, "Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                    
        }

        void DoMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X,mouseLocation.Y);
                Location = mousePose;
            }            
        }
      
        private void chCreateAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chCreateAccount.Checked)
            {
                panelAcount.Enabled = true;
                if (emp == null) return;
                if (username != "") pbDeleteAccount.Visible = true;
                else pbDeleteAccount.Visible = false;
            }
            else
            {
                panelAcount.Enabled = false;
                if (emp == null)
                {
                    txtUserName.Clear();
                    txtNewPassword.Clear();
                    txtRetypePassword.Clear();
                }
                else
                {
                    txtUserName.Text = username;
                    txtNewPassword.Text = password;
                    txtRetypePassword.Text = password;
                }
                pbDeleteAccount.Visible = false;
            }          
        }

        private void pbCloseInsertDialog(object sender, EventArgs e)
        {
            if (emp != null)
            {
                if (btnSave.Enabled)
                {
                    DialogResult dialog = MessageBox.Show("Do you want to save ?", "Update Employee", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        /* If The controls are not empty and It was updated successfully */
                        if (GetValueFromControls()) this.Close();
                    }
                    else if (dialog == DialogResult.No)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }                              
        }

        private void pbMinimizeInsertDialog(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
   
    }
}
