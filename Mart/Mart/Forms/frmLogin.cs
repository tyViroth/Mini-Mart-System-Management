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
using Mart.Intefaces;
using Mart.InstanceClasses;
using Mart.ControlClasses;

namespace Mart.Forms
{
    public partial class frmLogin : Form,IMessageType
    {
        private readonly int MINIMUM_WIDTH = 1000;
        private readonly int MINIMUM_HEIGHT = 700;
        private readonly int WIDTH_NO_TASKBAR = Screen.PrimaryScreen.WorkingArea.Width;
        private readonly int HEIGHT_NO_TASKBAR = Screen.PrimaryScreen.WorkingArea.Height;
        private readonly string userHolder = "Enter Username";
        private readonly string passHolder = "Enter Password";

        private Employee emp;

        public frmLogin()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            Width = MINIMUM_WIDTH;
            Height = MINIMUM_HEIGHT;
            this.CenterToScreen();
            /* Set Default to TextBox Username Password placeHolder */
            txtUsername.Text = userHolder;
            txtPassword.Text = passHolder;

            /* Set Text To Symbol as Password */
            txtPassword.PasswordChar = (char)0;

            pbShow.Visible = false;
            pContainer.Focus();

            /* Set Opacity Before Form Load */
            this.Opacity = 0.1;
            this.timeLogin.Interval = 20;
            timeLogin.Tick += DoTimeLoginOpacity;
        
            /* Text Got Focus Event */
            txtUsername.GotFocus += DoGotFocus;            
            txtPassword.GotFocus += DoGotFocus;
            
            /* Text Lost Focus Event */            
            txtUsername.LostFocus += DoLostFocus;
            txtPassword.LostFocus += DoLostFocus;

            txtPassword.TextChanged += DoTextBoxChanged;

            /* PictureBox Show Password Event */
            pbShow.MouseDown += pbShow_MouseDown;
            pbShow.MouseUp += pbShow_MouseUp;
            pbShow.MouseHover += pbShow_MouseHover;

            pbCancel.MouseHover += pbCancel_MouseHover;
            btnLogin.Click += DoLoginConfirmed;
            pContainer.Click += pContainer_Click;
            this.Shown += frmLogin_Shown;

            Employee.Created += Employee_Created;            
        }

        void frmLogin_Shown(object sender, EventArgs e)
        {
            pContainer.Focus();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {            
            /* Start Opacity of Login Form*/
            this.timeLogin.Start();            
        }

        private void DoTimeLoginOpacity(object sender, EventArgs e)
        {
            if (this.Opacity < 1.0) /* Range 0% to 100% or 0.0 to 1.0 */
            {
                this.Opacity += 0.025;
            }
            else
            {
                /* Greater than 100% or 1.0 */
                this.timeLogin.Stop();
            }
        }

        void pContainer_Click(object sender, EventArgs e)
        {                       
            pContainer.Focus();
        }

        void Employee_Created(Employee emp)
        {
            this.emp = emp;           
        }

        private void DoLoginConfirmed(object sender, EventArgs e)
        {
            SqlCommand cmd = null;
            SqlConnection con = Connection.getConnection();
            SqlDataReader dr;
           
            if (txtUsername.Text.Trim() == userHolder.Trim() || txtUsername.Text.Trim() == "")
            {
                MessageError("Please enter Username!","Login");                
                return;
            }
            else if (txtPassword.Text.Trim() == passHolder.Trim() || txtPassword.Text.Trim() == "")
            {
                MessageError("Please enter Password!", "Login");
                return;
            }

            try
            {
                con.Open();
                cmd = new SqlCommand("CompareLogin",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());               
                dr = cmd.ExecuteReader();
                while(dr.Read()){          
                    /* Compare Hash Code to Log In*/
                    if (ConvertHashCode.CompareDbHashWithInputHash((string)dr["password"], txtPassword.Text.Trim()))
                    {
                        Employee.CreatedInstance((int)dr["empID"], (string)dr["firstName"], (string)dr["lastName"], (string)dr["gender"], (DateTime)dr["birthDate"], (string)dr["username"], (string)dr["password"], new Role((int)dr["roleID"], (string)dr["roleName"]), (bool)dr["status"], (byte[])dr["photo"]);
                        break;
                    }                    
                } 
            }
            catch (SqlException ex)
            {
                MessageError(ex.Message,"Login");
            }
            finally
            {
                cmd.Dispose();
                con.Close();                
            }

            if (emp == null)
            {
                MessageError("Username or Password is incorrect","Login");
            }
            else
            {
                frmMain frm = new frmMain(emp);
                this.Hide();
                frm.ShowDialog();                
            }
        }        

        void pbShow_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbShow, "Show");
        }

        void pbShow_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '●';
        }

        void pbShow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                txtPassword.PasswordChar = (char)0;
            }
        }

        void DoTextBoxChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() != "" && txtPassword.Text.Trim() != passHolder.Trim())
            {
                txtPassword.PasswordChar = '●';
                pbShow.Visible = true;
            }
            else
            {
                pbShow.Visible = false;
            }
        }

        private void DoLostFocus(object sender, EventArgs e)
        {
            if (sender == txtUsername)
            {
                if (txtUsername.Text.Trim() == "")
                    txtUsername.Text = userHolder;
            }
            else
            {
                string pass = txtPassword.Text.Trim();
                if (pass == "" || pass == passHolder.Trim())
                {
                    txtPassword.Text = passHolder;
                    txtPassword.PasswordChar = (char)0;
                    pbShow.Visible = false;
                }
                else
                {
                    txtPassword.PasswordChar = '●';
                }
                                    
            }
        }

        private void DoGotFocus(object sender, EventArgs e)
        {
            if (sender == txtUsername)
            {
                if (txtUsername.Text.Trim() == userHolder.Trim())                
                    txtUsername.Clear();
            }
            else
            {
                string pass = txtPassword.Text.Trim();
                if(pass == passHolder.Trim())
                    txtPassword.Clear();                
            }
        }

        void pbCancel_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbCancel,"Cancel");
        }        

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void MessageSuccess(string des, string title)
        {
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public void MessageError(string des, string title)
        {
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        public void MessageWarning(string des, string title)
        {
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }

        public DialogResult MessageVerify(string des, string title)
        {
            return MessageBox.Show(des,title,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
        }
    }
}
