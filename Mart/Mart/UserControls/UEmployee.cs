using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

using Mart.UserControls;
using Mart.Intefaces;
using Mart.InstanceClasses;
using Mart.Forms;

namespace Mart
{
    public partial class UEmployee : UserControl,IFunctionModel<Employee>,IMessageType
    {
        private SqlCommand cmd;
        private SqlConnection con = Connection.getConnection();
        private List<Employee> employeeList = new List<Employee>();
        private SqlDataReader dr;
        private Employee emp;   
        private readonly string[] searchBy = new string[] { "Employee ID", "First Name", "Last Name", "Username", "Role Name","Gender"};
        private string placeHolderText;
        private int femaleNumber;
        private int AccoutNumber;

        private static UEmployee _instance;
        public static UEmployee Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UEmployee();
                return _instance;
            }
        }
        public UEmployee()
        {
            InitializeComponent();                     
            RegisterControlEvent();
            LoadData();   
        }

        void cboRole_SelectedValueChanged(object sender, EventArgs e)
        {
            femaleNumber = 0;
            AccoutNumber = 0;
            dgvEmployee.Rows.Clear();
            if (cboRole.SelectedIndex == -1) return;

            /* Set TextBox Search To default */
            if (cboSearch.Items.Count > 0)
            {
                cboSearch.SelectedIndex = 0;
                txtSearch.Text = searchBy[0];
            }

            foreach (Employee emp in employeeList)
	        {
		        if ((int)cboRole.SelectedValue == emp.Roles.ID)
                {
                    if (emp.Gender == "Female") femaleNumber++;
                    if (emp.UserName != "") AccoutNumber++;                   
                    AddDataRowToDataGridView(emp);
                }
	        }
            SetBottomInformationToTextBox();
            if (dgvEmployee.Rows.Count == 0) SetEnableButton(false);
            else SetEnableButton(true);
        }

        private void SetBottomInformationToTextBox()
        {
            txtEmployeeNumber.Text = dgvEmployee.Rows.Count.ToString();
            txtFemaleNumber.Text = femaleNumber.ToString();
            txtAccountNumber.Text = AccoutNumber.ToString();

        }

        private void AddDataRowToDataGridView(Employee emp)
        {
           int index = dgvEmployee.Rows.Add(emp.ID, emp.FirstName, emp.LastName, emp.Gender, emp.BirthDate, emp.UserName, emp.Roles.Name, emp.Photo);
           dgvEmployee.Rows[index].Tag = emp;
           emp.Tag = dgvEmployee.Rows[index];
        }

        private void RegisterControlEvent()
        {
            dgvEmployee.DataError += dgvEmployee_DataError;
            btnAdd.Click += DoClick;           
            btnUpdate.Click += DoClick;
            btnDelete.Click += DoClick;
            btnExport.Click += DoClick;
            btnRefresh.Click += DoClick;
            btnRoleDetails.Click += DoClick;
            btnPrint.Click += DoClick;

            cboRole.SelectedValueChanged += cboRole_SelectedValueChanged;
            txtSearch.KeyDown += KeyDownEnter;
            txtSearch.KeyPress += AllowNumberOnly;
            txtSearch.GotFocus +=txtSearch_GotFocus;
            txtSearch.LostFocus +=txtSearch_LostFocus;            

            cboSearch.SelectedIndexChanged += cboSearch_SelectedIndexChanged;

            /* Register Static Event of Class Employee */
            Employee.Loaded += Employee_Loaded;
            Employee.Created += Employee_Created;            
            Employee.Updated += Employee_Updated;
        }

        private void AllowNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (cboSearch.Text.Trim() == searchBy[0].Trim()) 
                Input.InputNmber((TextBox)sender,e);            
        }

        void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {                
            placeHolderText = cboSearch.Text;
            txtSearch.Text = placeHolderText;
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
            AccoutNumber = 0;
            femaleNumber = 0;

            if (txtSearch.Text.Trim() == "") return;                       
            if (e.KeyCode == Keys.Enter)
            {
                /* Set ComboBox Role to Defaut */
                if (cboRole.Items.Count > 0)
                    cboRole.SelectedIndex = -1;            

                dgvEmployee.Rows.Clear();
                if (cboSearch.SelectedIndex == 0) /* Search By Employee ID */
                {
                    if (txtSearch.Text.Trim() == searchBy[0]) return;       
                    int id = 0;
                    int.TryParse(txtSearch.Text.Trim(),out id);                   
                    foreach (Employee emp in employeeList)
                    {
                        if ( id == emp.ID)
                        {
                            if (emp.Gender == "Female") femaleNumber++;
                            if (emp.UserName != "") AccoutNumber++;        
                            AddDataRowToDataGridView(emp);
                        }
                    }
                }
                else if (cboSearch.SelectedIndex == 1) /* Search By First Name */
                {
                    if (txtSearch.Text.Trim() == searchBy[1]) return;
                    foreach (Employee emp in employeeList)
                    {
                        if (emp.FirstName.Trim().ToLower().StartsWith(txtSearch.Text.Trim().ToLower()))
                        {
                            if (emp.Gender == "Female") femaleNumber++;
                            if (emp.UserName != "") AccoutNumber++;        
                            AddDataRowToDataGridView(emp);
                        }
                    }
                }
                else if (cboSearch.SelectedIndex == 2) /* Search By Last Name */
                {
                    if (txtSearch.Text.Trim() == searchBy[2]) return;
                    foreach (Employee emp in employeeList)
                    {
                        if (emp.LastName.Trim().ToLower().StartsWith(txtSearch.Text.Trim().ToLower()))
                        {
                            if (emp.Gender == "Female") femaleNumber++;
                            if (emp.UserName != "") AccoutNumber++;        
                            AddDataRowToDataGridView(emp);
                        }
                    }
                }
                else if (cboSearch.SelectedIndex == 3) /* Search By Username (account) */
                {
                    if (txtSearch.Text.Trim() == searchBy[3]) return;
                    foreach (Employee emp in employeeList)
                    {
                        if (emp.UserName.Trim().ToLower().StartsWith(txtSearch.Text.Trim().ToLower()))
                        {
                            if (emp.Gender == "Female") femaleNumber++;
                            if (emp.UserName != "") AccoutNumber++;        
                            AddDataRowToDataGridView(emp);
                        }
                    }
                }
                else if (cboSearch.SelectedIndex == 4) /* Search By Role */
                {
                    if (txtSearch.Text.Trim() == searchBy[4]) return;
                    foreach (Employee emp in employeeList)
                    {
                        if (emp.Roles.Name.Trim().ToLower().StartsWith(txtSearch.Text.Trim().ToLower()))
                        {
                            if (emp.Gender == "Female") femaleNumber++;
                            if (emp.UserName != "") AccoutNumber++;        
                            AddDataRowToDataGridView(emp);
                        }
                    }
                }
                else if (cboSearch.SelectedIndex == 5) /* Search By Role */
                {
                    if (txtSearch.Text.Trim() == searchBy[5]) return;
                    foreach (Employee emp in employeeList)
                    {
                        if (emp.Gender.ToLower().StartsWith(txtSearch.Text.Trim().ToLower()))
                        {                            
                            if (emp.UserName != "") AccoutNumber++;
                            AddDataRowToDataGridView(emp);
                        }
                    }
                    femaleNumber = dgvEmployee.Rows.Count;
                }

                SetBottomInformationToTextBox();
                if (dgvEmployee.Rows.Count == 0) SetEnableButton(false);
                else SetEnableButton(true);
            }    
        }
       
        void Employee_Loaded(Employee emp)
        {
            employeeList.Add(emp);
            AddDataRowToDataGridView(emp);            
        }

        void Employee_Created(Employee emp)
        {            
            if (Insert(emp))
            {
                employeeList.Add(emp);
                AddDataRowToDataGridView(emp);            
                MessageSuccess("Inserted successfully", "Insert");
                RefreshBottomInformation();
            }
            else MessageError("Inserted Unsuccessfully", "Insert");
            if (employeeList.Count > 0)
            {
                btnRefresh.Enabled = true;
                SetEnableButton(true);
            }
        }

        private void Employee_Updated(Employee emp)
        {                    
            if (Update(emp))
            {
                int index = employeeList.IndexOf(emp);
                employeeList[index] = emp;               
                foreach (DataGridViewRow row in dgvEmployee.Rows)
                {
                    if (row.Tag == emp)
                    {
                        row.SetValues(emp.ID, emp.FirstName, emp.LastName, emp.Gender, emp.BirthDate, emp.UserName, emp.Roles.Name, emp.Photo);                      
                    }
                }
                MessageSuccess("Updated successfully", "Update");
                RefreshBottomInformation();
            }
            else MessageError("Updated Unsuccessfully", "Update");                                    
        }

        void dgvEmployee_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        private void DoClick(object sender, EventArgs e)
        {
            if(sender == btnAdd){
                if (cboRole.Items.Count > 0)
                {
                    frmInsertEmployee insert = new frmInsertEmployee();
                    insert.ShowDialog();
                }
                else
                {
                    MessageError("Please add role before add new Employee!","Required");
                    frmRole frm = new frmRole();                   
                    frm.Exited +=frm_Exited;
                    frm.ShowDialog();
                }
                }else if(sender == btnUpdate){                             
                emp = dgvEmployee.CurrentRow.Tag as Employee;
                frmInsertEmployee update = new frmInsertEmployee(emp);
                update.ShowDialog();        
            }else if(sender == btnDelete){
                DialogResult dr = this.MessageVerify("Do you want to delete?","Delete");
                if(dr == DialogResult.Yes){
                    foreach (DataGridViewRow row in dgvEmployee.SelectedRows)
                    {
                        int index = employeeList.IndexOf(row.Tag as Employee);
                        int id = employeeList[index].ID;
                        if (Delete(id))
                        {
                            MessageSuccess("Deleted successfully", "Delete");
                            employeeList.RemoveAt(index);
                            dgvEmployee.Rows.Remove(row);
                            RefreshBottomInformation();
                        }
                        else MessageError("Deleted Unsuccessfully", "Delete");                                            
                    }
                     if (employeeList.Count == 0) btnRefresh.Enabled = false;
                     if (dgvEmployee.Rows.Count == 0) SetEnableButton(false);
                }
            }
            else if (sender == btnExport)
            {
                Exporter.DataGridViewToExel(dgvEmployee);
            }                            
            else if (sender == btnRefresh)
            {
                femaleNumber = 0;
                AccoutNumber = 0;
                cboRole.SelectedIndex = -1;
                cboSearch.SelectedIndex = 0;
                txtSearch.Text = searchBy[0];

                if (employeeList.Count == 0 || employeeList.Count == dgvEmployee.Rows.Count) return;                              
                dgvEmployee.Rows.Clear();
                foreach (Employee emp in employeeList)
                {
                    if (emp.Gender == "Female") femaleNumber ++;
                    if (emp.UserName != "") AccoutNumber ++;
                    AddDataRowToDataGridView(emp);
                }
                SetBottomInformationToTextBox();
                SetEnableButton(true);
            }
            else if (sender == btnRoleDetails)
            {
                frmRole frm = new frmRole();                
                frm.Exited += frm_Exited;
                frm.ShowDialog();
            }
        }

        void frm_Exited(object sender)
        {            
            Controller.FillComboBoxValue(cboRole, "roleID", "roleName", "SetRoleToComboBox");         
        }

        public void LoadData()
        {
            femaleNumber = 0;
            AccoutNumber = 0;

            Controller.FillComboBoxValue(cboRole, "roleID", "roleName", "SetRoleToComboBox");
            cboRole.SelectedIndex = -1;            

            try
            {
                con.Open();
                /* Using Stored Procedure */
                cmd = new SqlCommand("GetActiveEmployees",con);
                dr = cmd.ExecuteReader();
                dgvEmployee.Rows.Clear();
                employeeList.Clear(); 
                while(dr.Read()){
                    if ((string)dr["gender"] == "Female") femaleNumber ++;
                    if ((string)dr["username"] != "") AccoutNumber ++;
                    Employee.LoadedInstance((int)dr["empID"], (string)dr["firstName"], (string)dr["lastName"], (string)dr["gender"], (DateTime)dr["birthDate"], (string)dr["username"], (string)dr["password"], new Role((int)dr["roleID"], (string)dr["roleName"]), (bool)dr["status"], (byte[])dr["photo"]);                
                }
                SetBottomInformationToTextBox();
                if (employeeList.Count == 0) btnRefresh.Enabled = false;                   
            }
            catch (Exception e)
            {
               MessageError(e.Message, "Error Load");
            }
            finally {
                cmd.Dispose();                
                dr.Close();
                con.Close();                
            }
        }
  
        public bool Insert(Employee emp)
        {
            bool success = false;
            try
            {
                con.Open();              
                cmd = new SqlCommand("Insert into Employee(lastName,firstName,gender,birthDate,username,password,roleID,status,photo) Values(@ln,@fn,@g,@bd,@un,@pw,@role,@s,@pho)",con);
                cmd.Parameters.AddWithValue("@ln", emp.LastName);
                cmd.Parameters.AddWithValue("@fn", emp.FirstName);
                cmd.Parameters.AddWithValue("@g", emp.Gender);
                cmd.Parameters.AddWithValue("@bd", emp.BirthDate);
                cmd.Parameters.AddWithValue("@un", emp.UserName);
                cmd.Parameters.AddWithValue("@pw", emp.Password);
                cmd.Parameters.AddWithValue("@role", emp.Roles.ID);
                cmd.Parameters.AddWithValue("@s", emp.Status);
                cmd.Parameters.AddWithValue("@pho",emp.Photo);
                if (cmd.ExecuteNonQuery() > 0) success = true;               
            }
            catch (Exception e)
            {
                success = false;
                MessageError(e.Message, "Error Insert");
            }
            finally { 
                con.Close();
                cmd.Dispose();
            }
            return success;
        }

        public bool Update(Employee emp)
        {
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
                cmd.Parameters.AddWithValue("@empID",emp.ID);                
                if(cmd.ExecuteNonQuery() > 0) success = true;             
            }
            catch (Exception e)
            {
                success = false;
                MessageError(e.Message, "Error Update");                
            }
            finally {
                con.Close();
                cmd.Dispose();
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = false;
            try
            {
                con.Open();
                cmd = new SqlCommand("UPDATE Employee SET deletedDate = @dDate, status = @stu WHERE empID = @id", con);
                cmd.Parameters.AddWithValue("@dDate",DateTime.Now);
                cmd.Parameters.AddWithValue("@stu",false);
                cmd.Parameters.AddWithValue("@id",id);
                if (cmd.ExecuteNonQuery() > 0) success = true;              
            }
            catch (Exception e)
            {
                MessageError(e.Message,"Error Delete");
                success = false;                
            }
            finally {
                con.Close();
                cmd.Dispose();
            }
            return success;
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
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }

        public DialogResult MessageVerify(string des, string title)
        {
            return MessageBox.Show(des, title,MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void UEmployee_Load(object sender, EventArgs e)
        {            
            if (dgvEmployee.Rows.Count == 0) SetEnableButton(false);
            else SetEnableButton(true);

            Controller.SetImageLayout(dgvEmployee.Columns[7]);            
            dgvEmployee.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";
            Controller.AlignHeaderTextCenter(dgvEmployee.Columns[0],dgvEmployee.Columns[1],dgvEmployee.Columns[2],dgvEmployee.Columns[3],dgvEmployee.Columns[4],dgvEmployee.Columns[5],dgvEmployee.Columns[6],dgvEmployee.Columns[7]);
            placeHolderText = searchBy[0];
            txtSearch.Text = placeHolderText;
            cboSearch.Items.AddRange(searchBy);
            cboSearch.SelectedIndex = 0;            
        }

        private void SetEnableButton(bool ena)
        {
            btnUpdate.Enabled = ena;
            btnDelete.Enabled = ena;
            btnExport.Enabled = ena;
            btnPrint.Enabled = ena;
        }

        private void RefreshBottomInformation()
        {
            femaleNumber = 0;
            AccoutNumber = 0;
            foreach (Employee emp in employeeList)
            {
                if (emp.Gender == "Female") femaleNumber++;
                if (emp.UserName != "") AccoutNumber++;                
            }
            SetBottomInformationToTextBox();            
        }

    }
       
}
