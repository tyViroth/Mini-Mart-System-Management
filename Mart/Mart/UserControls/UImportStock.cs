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
using Mart.Forms;

namespace Mart.UserControls
{

    public partial class UImportStock : UserControl
    {
       
        private List<string> search = new List<string>();
        private string searchCondition = "";
        private int searchFor = 0;
        private int selectedYear = 0;
        private int selectedMonth = 0;
        private bool integratedSearch = false;

        private bool searchBetweenDates = false;
        private SqlConnection con = Connection.getConnection();
        private SqlCommand cmd;
        private enum LoadFunction{
            LoadAllSoldData,
            SearchImportID,
            SearchImportDate,
            SearchSupplierID,
            SearchSupplierName,
            SearchProductID,
            SerachProductName,
            SearchBetweenDate,
            SearchImportConditionWithDates,
            SearchImportConditionWithYear,
            SearchImportConditionWithYearAndMonth,
            SearchImportWithYear,
            SearchImportWithYearAndMonth
        }

        public static UImportStock _instance;
        public static UImportStock Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UImportStock();
                return _instance;
            }
        }

        public UImportStock()
        {
            InitializeComponent();
            LoadSoldData(LoadFunction.LoadAllSoldData);
            RegisterEvent();
            LoadSearchTypeToCombobox();

            LoadImportYearToComboBoxYear();

            VisibleSearchDates(false);
        }

        private void LoadImportYearToComboBoxYear()
        {
            Controller.FillComboBoxValue(cboYear, "year", "year", "SelectYearlyImport");
            cboYear.SelectedIndex = -1;
        }

        private void LoadSearchTypeToCombobox()
        {
            search.Add("  Select here");
            search.Add("  Import ID");
            search.Add("  Import Date");
            search.Add("  Supplier ID");
            search.Add("  Supplier Name");       
            cboSearchType.Items.AddRange(search.ToArray());
            cboSearchType.SelectedIndex = 0;  
        }

        private void RegisterEvent()
        {
            chImportDetails.Click += chImportDetails_Click;
            chSearchDates.Click += chSearchDates_Click;           
            btnEdit.Click += Do_Click;
            btnRefresh.Click += btnRefresh_Click;

            dtpImportDate.ValueChanged += dtpImportDate_ValueChanged;

            cboSearchType.SelectedIndexChanged += cboSearchType_SelectedIndexChanged;
            cboYear.SelectedIndexChanged += cboYear_SelectedIndexChanged;
            cboMonth.SelectedIndexChanged += cboMonth_SelectedIndexChanged;

            txtSearchBox.GotFocus += txtSearchBox_GotFocus;
            txtSearchBox.LostFocus += txtSearchBox_LostFocus;
            txtSearchBox.KeyPress += AllowNumberOnly;
            txtSearchBox.KeyDown += txtSearchBox_KeyDown;

            pbSearch.Click += pbSearch_Click;
            pbSearch.MouseHover += pbSearch_MouseHover;
            pbSearch.MouseLeave += pbSearch_MouseLeave;
            dtpFrom.ValueChanged += DateChaged;
            dtpTo.ValueChanged += DateChaged;

            btnImport.Click += btnImport_Click;
        }

        void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            pbSearch.BackColor = Color.FromArgb(99, 174, 190);
        }

        void chSearchDates_Click(object sender, EventArgs e)
        {
            if (chSearchDates.Checked)
            {
                /*Allow search With Condition and Year | Month*/
                VisibleSearchDates(true);
                searchBetweenDates = false; 
            }
            else
            {
                /*Allow search With Condition and Between Dates*/
                VisibleSearchDates(false);
                searchBetweenDates = true;               
            }
        }

        private void VisibleSearchDates(bool ena)
        {
            lblFrom.Visible = ena;           
            dtpFrom.Visible = ena;
            lblTo.Visible = ena;
            dtpTo.Visible = ena;

            lblYear.Visible = !ena;
            cboYear.Visible = !ena;
            lblMonth.Visible = !ena;
            cboMonth.Visible = !ena;

            if (ena) 
            {
                /*Invisible Year&&Month and clear all Month items*/
                cboYear.SelectedIndex = -1;
                cboMonth.DataSource = null;
                cboMonth.Items.Clear();
                cboMonth.SelectedIndex = -1;
            }
        }
       
        void pbSearch_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbSearch,"Search");
            pbSearch.BackColor = Color.White;
        }

        void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedIndex == -1) return;
            int.TryParse(cboYear.SelectedValue.ToString(),out selectedYear);
            Controller.FillComboBoxValue(cboMonth, "monthNumber", "monthName", "GetImportDetailMonthText", selectedYear);
            cboMonth.SelectedIndex = -1;
            searchBetweenDates = false;

            integratedSearch = true;
            if (selectedYear > 0)
            {                
                chSearchDates.Visible = true;
                chSearchDates.Checked = false;
                VisibleSearchDates(false);
            }                
        }

        void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMonth.SelectedIndex == -1) return;
            searchBetweenDates = false;
            int.TryParse(cboMonth.SelectedValue.ToString(),out selectedMonth);

            integratedSearch = true;
            if (selectedMonth > 0)
            {
                chSearchDates.Visible = true;
                chSearchDates.Checked = false;
                VisibleSearchDates(false);
            }                
        }

        private void DateChaged(object sender, EventArgs e)
        {
            cboYear.SelectedIndex = -1;
            cboMonth.SelectedIndex = -1;
            searchBetweenDates = true;

            integratedSearch = true;
        }

        void pbSearch_Click(object sender, EventArgs e)
        {            
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("From Date cannot be greater than To Date.","Search",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            /*Search Sold With Between 2 Dates and Some Condition*/
            if (searchBetweenDates)
            {                
                if (cboSearchType.SelectedIndex == 0 || txtSearchBox.Text.Trim() == cboSearchType.Text.Trim() || cboSearchType.SelectedIndex == 2) //2 search by Sold Date
                {
                    /*When user doesnt' fill any data to search with dates*/
                    LoadSoldData(LoadFunction.SearchBetweenDate);
                }else{
                    /*When user fill any data to search with dates*/
                    GetSearchConditionText();
                    LoadSoldData(LoadFunction.SearchImportConditionWithDates);                   
                }
            }
            /*Search Sold With Year or Month || Year&&Month(combine) and Some Above Condition*/
            else
            {
                if (cboSearchType.SelectedIndex == 0 || txtSearchBox.Text.Trim() == cboSearchType.Text.Trim() || cboSearchType.SelectedIndex == 2) //2 search by Sold Date
                {
                    /*When user doesnt' fill any data to search with selected Year | Month*/
                    if (cboMonth.SelectedIndex == -1)
                    {
                        /*User didn't select month*/
                        LoadSoldData(LoadFunction.SearchImportWithYear);
                    }
                    else
                    {
                        /*User selected month*/
                        LoadSoldData(LoadFunction.SearchImportWithYearAndMonth);
                    }                        
                }
                else
                {
                    /*When user fill any data to search with selected Year | Month*/
                    GetSearchConditionText();
                    if (cboMonth.SelectedIndex == -1)
                    {
                        /*User didn't select month*/
                        LoadSoldData(LoadFunction.SearchImportConditionWithYear);
                    }
                    else
                    {
                        /*User selected month*/
                        LoadSoldData(LoadFunction.SearchImportConditionWithYearAndMonth);
                    }
                        
                }                
            }
        }

        private void GetSearchConditionText()
        {
           
            if (cboSearchType.SelectedIndex == 1) /*Sold ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
            }
            else if (cboSearchType.SelectedIndex == 2) /*Sold DATE*/
            {
                searchCondition = dtpImportDate.Value.ToString();
            }
            else if (cboSearchType.SelectedIndex == 3) /*Employee ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
            }
            else if (cboSearchType.SelectedIndex == 4) /*Employee Name*/
            {
                searchCondition = txtSearchBox.Text.Trim();
            }
            else if (cboSearchType.SelectedIndex == 5) /*Product ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
            }
            else if (cboSearchType.SelectedIndex == 6) /*Product Name*/
            {
                searchCondition = txtSearchBox.Text.Trim();
            }
        }
       
        void chImportDetails_Click(object sender, EventArgs e)
        {
            if (chImportDetails.Checked)
            {
                search.Add("  Product ID");
                search.Add("  Product Name");
                cboSearchType.Items.Clear();
                cboSearchType.Items.AddRange(search.ToArray());
                searchFor = 1;  /*Search for Sold* Sold Details Information*/
            }
            else
            {
                search.Remove("  Product ID");
                search.Remove("  Product Name");
                cboSearchType.Items.Clear();
                cboSearchType.Items.AddRange(search.ToArray());
                searchFor = 0; /*Search for General Sold*/
            }
            btnRefresh_Click(btnRefresh,e);           
        }

        void dtpImportDate_ValueChanged(object sender, EventArgs e)
        {
            CallFunctionToSearchSold();
        }

        void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (integratedSearch)
                {
                    /*User search with more than one Condition*/
                    pbSearch_Click(pbSearch,null);
                }
                else
                {
                    /*User search with only one Condition*/
                    CallFunctionToSearchSold();
                }                
            }
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {            
            cboSearchType.SelectedIndex = 0;
            dtpImportDate.Visible = false;

            cboYear.SelectedIndex = -1;
            cboMonth.DataSource = null;
            cboMonth.Items.Clear();

            SetDefaultDateToDTP();
            
            integratedSearch = false;
            LoadSoldData(LoadFunction.LoadAllSoldData);
        }

        private void SetDefaultDateToDTP()
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            dtpImportDate.Value = DateTime.Now;
        }

        private void AllowNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (cboSearchType.SelectedIndex == 1 || cboSearchType.SelectedIndex == 3 || cboSearchType.SelectedIndex == 5)
            {
                Input.InputNmber(txtSearchBox,e);
            }
        }

        void txtSearchBox_LostFocus(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == "")
            {
                txtSearchBox.Text = cboSearchType.Text;
            }
        }

        void txtSearchBox_GotFocus(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == cboSearchType.Text && cboSearchType.SelectedIndex != 0)
            {
                txtSearchBox.Clear();
            }
        }      

        void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSearchType.SelectedIndex == -1) return;
            txtSearchBox.Text = cboSearchType.Text;
            if (cboSearchType.SelectedIndex == 0)
            {
                dtpImportDate.Visible = false;
                txtSearchBox.ReadOnly = true;
                txtSearchBox.BackColor = Color.White;
            }
            else
            {
                txtSearchBox.ReadOnly = false;
                if (cboSearchType.SelectedIndex == 2)
                {
                    dtpImportDate.Visible = true;
                }
                else dtpImportDate.Visible = false;
            }                       
        }
        private void Do_Click(object sender, EventArgs e)
        {
            
        }

        private void CallFunctionToSearchSold()
        {
            if (cboSearchType.SelectedIndex == 0 || txtSearchBox.Text.Trim().CompareTo(cboSearchType.Text.Trim())==0 && cboSearchType.SelectedIndex != 2 || txtSearchBox.Text.Trim() == "") return;           

            if (cboSearchType.SelectedIndex == 1) /*Import ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchImportID);
            }
            else if (cboSearchType.SelectedIndex == 2) /*Import DATE*/
            {
                searchCondition = dtpImportDate.Value.ToString();
                LoadSoldData(LoadFunction.SearchImportDate);
            }
            else if (cboSearchType.SelectedIndex == 3) /*Supplier ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchSupplierID);
            }
            else if (cboSearchType.SelectedIndex == 4) /*Supplier Name*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchSupplierName);
            }
            else if (cboSearchType.SelectedIndex == 5) /*Product ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchProductID);
            }
            else if (cboSearchType.SelectedIndex == 6) /*Product Name*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SerachProductName);
            }
        }

        private void LoadSoldData(LoadFunction load)
        {
            /*
              if searchFor = 0 : Search Sold Information
              if searchFor = 1 : Search Sold Detail Information
             */
            try
            {                
                if (con.State == ConnectionState.Closed)                
                    con.Open();                               
                if (load == LoadFunction.LoadAllSoldData) /*Search General Sold  and  Sold Details*/
                {
                    cmd = new SqlCommand("GetImportInformation", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@searchFor",searchFor);
                }
                else if (load == LoadFunction.SearchImportID || load == LoadFunction.SearchImportDate || load == LoadFunction.SearchSupplierID || load == LoadFunction.SearchSupplierName || load == LoadFunction.SearchProductID || load == LoadFunction.SerachProductName)
                {
                        cmd = new SqlCommand("SearchImportInformation", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                        cmd.Parameters.AddWithValue("@condition", searchCondition);
                        cmd.Parameters.AddWithValue("@searchFor",searchFor);        
                }                           
                else if (load == LoadFunction.SearchBetweenDate) 
                {
                    /*Search Between two Dates => for General Import  and  Sold Details*/
                    cmd = new SqlCommand("SearchImportBetweenDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);              
                }
                else if(load == LoadFunction.SearchImportConditionWithDates)
                {
                    /*Search Between two Dates And Combine with General Search (Import ID, Product Name,...)*/
                    cmd = new SqlCommand("SearchImportConditionWithDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                    cmd.Parameters.AddWithValue("@condition",searchCondition);
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchImportConditionWithYear)
                {                    
                    cmd = new SqlCommand("SearchImportConditionWithYear", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                    cmd.Parameters.AddWithValue("@condition",searchCondition);
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchImportWithYear)
                {
                    cmd = new SqlCommand("SearchImportWithYear", con);
                    cmd.CommandType = CommandType.StoredProcedure;          
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchImportWithYearAndMonth)
                {
                    cmd = new SqlCommand("SearchImportWithYearAndMonth", con);
                    cmd.CommandType = CommandType.StoredProcedure;   
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@month",selectedMonth);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchImportConditionWithYearAndMonth)
                {
                    cmd = new SqlCommand("SearchImportConditionWithYearAndMonth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                    cmd.Parameters.AddWithValue("@condition",searchCondition);
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@month",selectedMonth);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvImport.DataSource = dt;
               
                /*Align Header to Middle Center*/
                foreach (DataGridViewColumn col in dgvImport.Columns)
                {
                    Controller.AlignHeaderTextCenter(col);
                }

                /*Align Column Date to middle*/                
                dgvImport.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvImport.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvImport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvImport.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                               
                /*Display And Format all Sold Detail Information Columns*/
                if (searchFor == 1)
                {
                    /*Align number colums to Middle Right*/
   
                    int lastCol = dgvImport.Columns.Count;
                    /*Sub Amount Column*/
                    dgvImport.Columns[lastCol-1].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvImport.Columns[lastCol-1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*%Import Price Column*/
                    dgvImport.Columns[lastCol -2].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvImport.Columns[lastCol -2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*% Import Quantity Column*/
                    dgvImport.Columns[lastCol - 3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*Product Name Column*/                 
                    dgvImport.Columns[lastCol - 4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    /*Product ID Column*/
                    dgvImport.Columns[lastCol - 5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                }
                else
                {
                    int lastCol = dgvImport.Columns.Count - 1;
                    dgvImport.Columns[lastCol].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvImport.Columns[lastCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                    
                }

                double total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    total += double.Parse(row[dt.Columns.Count - 1].ToString());
                }
                lblTotalRow.Text = dt.Rows.Count.ToString();
                lblTotalAmount.Text = string.Format("{0:N2} Riel",total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Load Data");
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                    con.Close();               
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            frmInsertImport frmInsertImport = new frmInsertImport();
            frmInsertImport.Closed += FrmInsertImport_Closed;
            frmInsertImport.Show();
        }

        private void FrmInsertImport_Closed(object sender, EventArgs e)
        {
            btnRefresh_Click(btnRefresh,null);
        }      
    }   
}
