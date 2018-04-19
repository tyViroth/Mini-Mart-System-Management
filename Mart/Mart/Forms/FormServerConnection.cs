using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Mart.Forms
{
    public partial class FormServerConnection : Form
    {
        readonly string PCName = Environment.MachineName;
        private string conString = "";
        private Point mouseLocation;
        public FormServerConnection()
        {
            InitializeComponent();
            this.Shown += frmServerConnection_Shown;
            this.Load += frmServerConnection_Load;

            this.btnTestConnection.Click += btnConnect_Click;
            this.btnSave.Click += btnSave_Click;
            this.pbClose.Click += pbClose_Click;
            this.pbClose.MouseHover += pbClose_MouseHover;
            this.cboAuthentication.SelectedIndex = 0;
            pAccount.Enabled = false;
            btnSave.Enabled = false; /*If our connection is successful*/
            this.cboAuthentication.SelectedIndexChanged += cboAuthentication_SelectedIndexChanged;


            this.MouseMove += frmServerConnection_MouseMove;
            this.MouseDown += frmServerConnection_MouseDown;
        }

        void frmServerConnection_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X,-e.Y);
        }

        void frmServerConnection_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X,mouseLocation.Y);
                Location = mousePose;
            }
        }

        void pbClose_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbClose,"Close Connection");
        }

        void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (conString != "")
            {
                Connection conn = new Connection();
                if (conn.SaveConnectionString("LocalConnection", conString))
                {
                    MessageBox.Show("Your Connection is saved successfully", "Connection");
                    FormLogin frm = new FormLogin();
                    this.Hide();
                    frm.ShowDialog();
                };
            }           
        }

        void cboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            if (cboAuthentication.SelectedIndex == 0)
            {
                pAccount.Enabled = false;
            }
            else pAccount.Enabled = true;
        }
        void btnConnect_Click(object sender, EventArgs e)
        {            
            if (cboAuthentication.SelectedIndex == 0)
            {
               conString = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", cboServerName.Text.Trim(), cboDatabaseName.Text.Trim());
            }
            else
            {
                if (txtPassword.Text.Trim() == "" || txtUsername.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Username or Password to access your database","Connect",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                    return;
                }
               conString = string.Format(@"Data Source={0};Initial Catalog={1};User={3};Password={4};", cboServerName.Text.Trim(), cboDatabaseName.Text.Trim(),txtUsername.Text.Trim(),txtPassword.Text.Trim());         
            }

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(conString);
                if (con.State == ConnectionState.Closed)
                {                
                    con.Open();
                    MessageBox.Show("Your connection is successful.", "Connection");
                }
                btnSave.Enabled = true;   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Connection");
                btnSave.Enabled = false;
            }
            finally
            {
                con.Close();
            }
        }

        void frmServerConnection_Load(object sender, EventArgs e)
        {
            cboServerName.Items.Add(".");
            cboServerName.Items.Add("(local)");
            cboServerName.Items.Add(@".\SQLEXPRESS");
            cboServerName.Items.Add(string.Format(@"{0}/SQLEXPRESS",PCName));
            cboServerName.Items.Add(PCName);
            cboServerName.Items.Add(string.Format(@"{0}\MSSQLSERVER",PCName));
            
            string connectionString = "Data Source=.; Integrated Security=True;";

            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open(); 
                }                               
                SqlDataAdapter da = new SqlDataAdapter("SELECT name from sys.databases", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboDatabaseName.DataSource = dt;
                cboDatabaseName.DisplayMember = "name";
                cboDatabaseName.ValueMember = "name";                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Retrieve all Server ");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }                    
            }
            
        }
        void frmServerConnection_Shown(object sender, EventArgs e)
        {
            //SqlTestInfo();
        }


        public void SqlTestInfo()
        {
            RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

            foreach (string s in key.GetValueNames())
            {
                MessageBox.Show(s);
            }

            key.Close();
            baseKey.Close(); 
        }    
    }
}