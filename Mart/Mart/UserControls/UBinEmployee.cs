using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Mart.Intefaces;
using Mart.DataModel;
namespace Mart.UserControls
{
    public partial class UBinEmployee : UserControl,IMessageType
    {       
        private readonly List<string> searchBy = new List<string>() { "Employee ID", "First Name", "Last Name", "Username", "Role Name", "Gender" };
        private string placeHolderText;

        public UBinEmployee()
        {
            InitializeComponent();
            RegisterEvent();

            Controller.FillComboBoxValue(cboRole, "roleID", "roleName", "SetRoleToComboBox");
            cboRole.SelectedIndex = -1;

            /* Load Data to DataGridView without conditioin "" */
            RefreshDataGridview("", -1, false);

            cboSearch.DataSource = searchBy;
            cboSearch.SelectedIndex = 0;
            txtSearch.Text = cboSearch.SelectedValue as string;
        }

        private void RegisterEvent()
        {     

            /* Search Option */
            cboSearch.SelectedValueChanged += cboSearch_SelectedValueChanged;
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
            txtSearch.KeyDown += KeyDownEnter;
            txtSearch.KeyPress += AllowNumberOnly;

            cboRole.SelectedValueChanged += cboRole_SelectedValueChanged;
            /* End Search Option*/

            btnRefresh.Click += btnRefresh_Click;
            btnExport.Click += btnExport_Click;
        }

        void btnExport_Click(object sender, EventArgs e)
        {
            Exporter.DataGridViewToExel(dgvEmployee);
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridview("",-1,false);
        }

        void cboRole_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboRole.SelectedIndex == -1) return;           
            try
            {
                /* Search by Role ID */
                if (cboRole.Items.Count > 0)
                {
                    RefreshDataGridview(((int)cboRole.SelectedValue).ToString(), 6, false);              
                }
            }
            catch (Exception ex)
            {
                //MessageError(ex.Message,"Combox Role");
            }  
        }

        private void AllowNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (cboSearch.Text.Trim() == searchBy[0].Trim())
                Input.InputNmber((TextBox)sender, e);
        }

        void txtSearch_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
                txtSearch.Text = placeHolderText;
        }

        void txtSearch_GotFocus(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void KeyDownEnter(object sender, KeyEventArgs e)
        {           
            if (txtSearch.Text.Trim() == "") return;
            if (e.KeyCode == Keys.Enter)
            {
                int typeSearch = -1;               

                /* Set ComboBox Role to Defaut */
                if (cboRole.Items.Count > 0)
                    cboRole.SelectedIndex = -1;
               
                if (cboSearch.SelectedIndex == 0) /* Search By Employee ID */
                {
                    if (txtSearch.Text.Trim() == searchBy[0]) return;                                  
                    typeSearch = 0;
                }
                else if (cboSearch.SelectedIndex == 1) /* Search By First Name */
                {
                    if (txtSearch.Text.Trim() == searchBy[1]) return;
                    typeSearch = 1;
                }
                else if (cboSearch.SelectedIndex == 2) /* Search By Last Name */
                {
                    if (txtSearch.Text.Trim() == searchBy[2]) return;
                    typeSearch = 2;
                }
                else if (cboSearch.SelectedIndex == 3) /* Search By Username (account) */
                {
                    if (txtSearch.Text.Trim() == searchBy[3]) return;
                    typeSearch = 3;
                }
                else if (cboSearch.SelectedIndex == 4) /* Search By Role */
                {
                    if (txtSearch.Text.Trim() == searchBy[4]) return;
                    typeSearch = 4;
                }
                else if (cboSearch.SelectedIndex == 5) /* Search By Role */
                {
                    if (txtSearch.Text.Trim() == searchBy[5]) return;
                    typeSearch = 5;      
                }

                RefreshDataGridview(txtSearch.Text.Trim(),typeSearch,false);
            }
        }

        private void RefreshDataGridview(string condition, int searchType, bool status)
        {
            EmployeesEntity db = new EmployeesEntity();
            dgvEmployee.DataSource = db.PrintEmployee(condition, searchType, status).ToList();

            Controller.AlignHeaderTextCenter(dgvEmployee.Columns[0],dgvEmployee.Columns[1],dgvEmployee.Columns[2],dgvEmployee.Columns[3],dgvEmployee.Columns[4],dgvEmployee.Columns[5],dgvEmployee.Columns[6]);
            dgvEmployee.Columns[0].HeaderText = "Employee ID";
            dgvEmployee.Columns[1].HeaderText = "First Name";
            dgvEmployee.Columns[2].HeaderText = "Last Name";
            dgvEmployee.Columns[3].HeaderText = "Gender";
            dgvEmployee.Columns[4].HeaderText = "BirthDate";
            dgvEmployee.Columns[5].HeaderText = "Username";
            dgvEmployee.Columns[6].HeaderText = "Role";

            dgvEmployee.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";

            txtEmployeeNumber.Text = dgvEmployee.Rows.Count.ToString();
            int female = 0;
            int account = 0;
            foreach (DataGridViewRow row in dgvEmployee.Rows)
            {
                if (row.Cells[3].Value.ToString().CompareTo("Female") == 0)
                {
                    female++;
                } if (row.Cells[5].Value.ToString().Trim() != "")
                {
                    account++;                    
                }
            }
            txtAccountNumber.Text = account.ToString();
            txtFemaleNumber.Text = female.ToString();
        }

        void cboSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            txtSearch.Text = cboSearch.SelectedValue as string;
            placeHolderText = cboSearch.SelectedValue as string;
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
