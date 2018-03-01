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

namespace Mart.Forms
{
    public partial class frmRole : Form,IMessageType,IFunctionModel<Role>
    {
        private SqlDataAdapter da;
        private DataTable dt;
        private SqlCommand cmd;
        private SqlConnection con = Connection.getConnection();        
        private int index;
        private readonly string auto = "Auto Number";
        private Role role = new Role();
        private Point mouseLocation;
        private bool newRole;
        private bool updateRole;
        public delegate void ExitedHandler(object sender);
        public event ExitedHandler Exited = null; 
        public frmRole()
        {
            InitializeComponent();
            RegisterEvent();          
        }

        private void RegisterEvent()
        {
            btnNew.Click += DoButtonClick;
            btnSave.Click += DoButtonClick;
            btnExport.Click += DoButtonClick;
            btnDelete.Click += DoButtonClick;

            pbClose.Click += DoButtonClick;

            dgvRole.SelectionChanged += DoDataGridSelectedRows;

            pBanner.MouseMove += pBanner_MouseMove;
            pBanner.MouseDown += pBanner_MouseDown;

            lblTitle.MouseMove += pBanner_MouseMove;
            lblTitle.MouseDown += pBanner_MouseDown;

            txtRoleName.TextChanged += DoTextBoxChanged;
        }

        private void DoTextBoxChanged(object sender, EventArgs e)
        {
            if (updateRole) /* If user clicked on any row of datagirdview */
            {
                if (txtRoleName.Text.Trim() != role.Name) btnSave.Enabled = true;
                else btnSave.Enabled = false;
            }
            else if (newRole)
            {
                if (txtRoleName.Text.Trim() != "") btnSave.Enabled = true;
                else btnSave.Enabled = false;
            }            
        }

        void pBanner_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X,-e.Y);
        }

        void pBanner_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X,mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void DoDataGridSelectedRows(object sender, EventArgs e)
        {
            if (dgvRole.Rows.Count == 0) return;
            index = dgvRole.CurrentRow.Index;
            DataGridViewRow row = dgvRole.Rows[index];
            txtRoleID.Text = row.Cells[0].Value.ToString();
            txtRoleName.Text = row.Cells[1].Value.ToString();
            role = new Role((int)row.Cells[0].Value, row.Cells[1].Value.ToString());
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
            updateRole = true;           
        }

        private void DoButtonClick(object sender, EventArgs e)
        {
            if (sender == btnNew)
            {              
                ButtonClearClicked();

            }else if(sender == btnSave){
                bool existing = false;
                GetRole();
                if (newRole)
                {
                    foreach (DataGridViewRow row in dgvRole.Rows)
                    {
                        if (row.Cells[1].Value.ToString().Trim().CompareTo(role.Name.Trim()) == 0)
                        {
                            existing = true;
                            break;
                        }
                    }
                    if (existing)
                    {
                        MessageError("This role is already exist !", "Existing Role");
                        return;
                    } if (txtRoleName.Text.Trim() == "")
                    {
                        MessageError("Enter role description", "Required");
                        return;
                    }
                    if (Insert(role))
                    {
                        LoadData();
                        MessageSuccess("Saved successfully", "Insert Role");
                        if (dgvRole.Rows.Count > 0) EnabledButton(true);

                        btnSave.Enabled = false;
                        updateRole = false;
                        newRole = false;
                    }
                    else MessageError("Saved unsuccessfully", "Insert Role");
                }
                else if (updateRole)
                {
                    if (Update(role))
                    {
                        LoadData();
                        MessageSuccess("Updated successfully", "Update Role");
                    }
                    else MessageSuccess("Updated unsuccessfully", "Update Role");  
                }               
            }
            else if (sender == btnExport)
            {
                Exporter.DataGridViewToExel(dgvRole);                              
            }else if(sender == btnDelete){

                if (dgvRole.SelectedRows.Count == -1 )
                {
                    MessageError("Please any row to delete","Required");
                    return;
                }
                
                DialogResult dialog = MessageVerify("Do you want to delete Role ID = "+txtRoleID.Text.Trim()+" ?","Delete Role");
                if (dialog == DialogResult.Yes)
                {
                    GetRole();
                    if (Delete(role.ID))
                    {
                        LoadData();
                        MessageSuccess("Deleted successfully","Delete Role");
                        if (dgvRole.Rows.Count == 0)
                        {
                            EnabledButton(false);
                            ButtonClearClicked();
                        }
                    }
                    else MessageSuccess("Deleted unsuccessfully", "Delete Role");                                            
                }
            }
            else if (sender == pbClose)
            {
                if (Exited != null)
                    Exited(this);
                this.Close();
            }
        }

        private void ButtonClearClicked()
        {
            txtRoleID.Text = auto;
            txtRoleName.Clear();

            newRole = true;
            updateRole = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void EnabledButton(bool p)
        {
            btnExport.Enabled = p;
            btnDelete.Enabled = p;
            btnSave.Enabled = p;
        }

        private void GetRole()
        {
            if (txtRoleID.Text.Trim() == auto)  
                role.ID = Controller.GetLastAutoIncrement("Role") + 1;
            else 
                role.ID = int.Parse(txtRoleID.Text.Trim());
            role.Name = txtRoleName.Text.Trim();
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

        public void LoadData()
        {
            try
            {
                con.Open();
                da = new SqlDataAdapter("SELECT roleID, roleName FROM Role WHERE status != 0", con);
                dt = new DataTable();
                da.Fill(dt);
                dgvRole.DataSource = dt;
            }
            catch (Exception e)
            {
                MessageError(e.Message,"Load Data");
            }
            finally
            {
                try
                {
                    dt.Dispose();
                    da.Dispose();                             
                    con.Close();
                }
                catch (NullReferenceException ne)
                {
                    MessageError(ne.Message,"Load Data");                   
                }
            }
            dgvRole.Columns[0].HeaderText = "Role ID";
            dgvRole.Columns[1].HeaderText = "Role Name";
            Controller.AlignHeaderTextCenter(dgvRole.Columns[0],dgvRole.Columns[1]);
            if (dgvRole.Rows.Count == 0)
            {
                EnabledButton(false);
                ButtonClearClicked();
            }

            dgvRole.ClearSelection();
            dgvRole.CurrentCell = null;

        }

        public bool Insert(Role role)
        {
            bool success = false;
            try
            {
                con.Open();
                cmd = new SqlCommand("INSERT INTO Role(roleName,status) VALUES(@name, @sta)", con);
                cmd.Parameters.AddWithValue("@name",role.Name);
                cmd.Parameters.AddWithValue("@sta", true);                
                if (cmd.ExecuteNonQuery() > 0) success = true;
            }
            catch (Exception e)
            {
                MessageError(e.Message, "Insert Role");
                success = false;
            }
            finally
            {
                try
                {
                    cmd.Dispose();                    
                    con.Close();                                    
                }
                catch (NullReferenceException ne)
                {
                    MessageError(ne.Message, "Insert Role");
                }
            }
            return success;
        }

        public bool Update(Role role)
        {
            bool success = false;
            try
            {
                con.Open();
                cmd = new SqlCommand("UPDATE Role SET roleName = @name WHERE roleID = @id", con);
                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@id",role.ID);
                if (cmd.ExecuteNonQuery() > 0) success = true;
            }
            catch (Exception e)
            {
                MessageError(e.Message, "Update Role");
                success = false;
            }
            finally
            {
                try
                {
                    con.Close();
                    cmd.Dispose();
                }
                catch (NullReferenceException ne)
                {
                    MessageError(ne.Message,"Update Role");
                }
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = false;
            try
            {
                con.Open();
                cmd = new SqlCommand("UPDATE Role SET status = @sta WHERE roleID = @id",con);
                cmd.Parameters.AddWithValue("@sta",false);
                cmd.Parameters.AddWithValue("@id",id);
                if (cmd.ExecuteNonQuery() > 0) success = true;                
            }
            catch (Exception e)
            {
                MessageError(e.Message,"Delete Role");
                success = false;
            }
            finally
            {
                try
                {
                    con.Close();
                    cmd.Dispose();
                }
                catch (NullReferenceException ne)
                {
                    MessageError(ne.Message, "Delete Role");
                }
            }
            return success;
        }

        private void frmRole_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.CenterToParent();
            LoadData();
            dgvRole.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 40, FontStyle.Regular);
        }
    }
}
