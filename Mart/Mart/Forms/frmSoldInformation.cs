using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart.Forms
{
    public partial class frmSoldInformation : Form
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
            SearchSoldID,
            SearchSoldDate,
            SearchEmployeeID,
            SearchEmployeeName,
            SearchProductID,
            SerachProductName,
            SearchBetweenDate,
            SearchSoldConditionWithDates,
            SearchSoldConditionWithYear,
            SearchSoldConditionWithYearAndMonth,
            SearchSoldWithYear,
            SearchSoldWithYearAndMonth
        }
             
        public frmSoldInformation()
        {
            InitializeComponent();
            LoadSoldData(LoadFunction.LoadAllSoldData);
            RegisterEvent();
            LoadSearchTypeToCombobox();

            LoadSoldYearToComboBoxYear();

            VisibleSearchDates(false);
        }

        private void LoadSoldYearToComboBoxYear()
        {
            Controller.FillComboBoxValue(cboYear,"year","year","SelectYearlySold");
            cboYear.SelectedIndex = -1;
        }

        private void LoadSearchTypeToCombobox()
        {
            search.Add("  Select here");
            search.Add("  Sold ID");
            search.Add("  Sold Date");
            search.Add("  Seller ID");
            search.Add("  Seller Name");            
            cboSearchType.Items.AddRange(search.ToArray());
            cboSearchType.SelectedIndex = 0;  
        }

        private void RegisterEvent()
        {
            chSoldDetails.Click += chSoldDetails_Click;
            chSearchDates.Click += chSearchDates_Click;
            btnDelete.Click += Do_Click;
            btnEdit.Click += Do_Click;
            btnRefresh.Click += btnRefresh_Click;

            dtpSoldDate.ValueChanged += dtpSoldDate_ValueChanged;

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
            Controller.FillComboBoxValue(cboMonth, "monthNumber", "monthName", "GetSoldDetailMonthText", selectedYear);
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
                    LoadSoldData(LoadFunction.SearchSoldConditionWithDates);                   
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
                        LoadSoldData(LoadFunction.SearchSoldWithYear);
                    }
                    else
                    {
                        /*User selected month*/
                        LoadSoldData(LoadFunction.SearchSoldWithYearAndMonth);
                    }                        
                }
                else
                {
                    /*When user fill any data to search with selected Year | Month*/
                    GetSearchConditionText();
                    if (cboMonth.SelectedIndex == -1)
                    {
                        /*User didn't select month*/
                        LoadSoldData(LoadFunction.SearchSoldConditionWithYear);
                    }
                    else
                    {
                        /*User selected month*/
                        LoadSoldData(LoadFunction.SearchSoldConditionWithYearAndMonth);
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
                searchCondition = dtpSoldDate.Value.ToString();
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
       

        void chSoldDetails_Click(object sender, EventArgs e)
        {
            if (chSoldDetails.Checked)
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

        void dtpSoldDate_ValueChanged(object sender, EventArgs e)
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
            dtpSoldDate.Visible = false;

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
            dtpSoldDate.Value = DateTime.Now;
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
                dtpSoldDate.Visible = false;
                txtSearchBox.ReadOnly = true;
                txtSearchBox.BackColor = Color.White;
            }
            else
            {
                txtSearchBox.ReadOnly = false;
                if (cboSearchType.SelectedIndex == 2)
                {
                    dtpSoldDate.Visible = true;
                }
                else dtpSoldDate.Visible = false;
            }                       
        }
        private void Do_Click(object sender, EventArgs e)
        {
            
        }

        private void CallFunctionToSearchSold()
        {
            if (cboSearchType.SelectedIndex == 0 || txtSearchBox.Text.Trim().CompareTo(cboSearchType.Text.Trim())==0 && cboSearchType.SelectedIndex != 2 || txtSearchBox.Text.Trim() == "") return;           

            if (cboSearchType.SelectedIndex == 1) /*Sold ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchSoldID);
            }
            else if (cboSearchType.SelectedIndex == 2) /*Sold DATE*/
            {
                searchCondition = dtpSoldDate.Value.ToString();
                LoadSoldData(LoadFunction.SearchSoldDate);
            }
            else if (cboSearchType.SelectedIndex == 3) /*Employee ID*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchEmployeeID);
            }
            else if (cboSearchType.SelectedIndex == 4) /*Employee Name*/
            {
                searchCondition = txtSearchBox.Text.Trim();
                LoadSoldData(LoadFunction.SearchEmployeeName);
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
                    cmd = new SqlCommand("GetSoldInformation", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@searchFor",searchFor);
                }
                else if (load == LoadFunction.SearchSoldID || load == LoadFunction.SearchSoldDate || load == LoadFunction.SearchEmployeeID || load == LoadFunction.SearchEmployeeName || load == LoadFunction.SearchProductID || load == LoadFunction.SerachProductName)
                {
                        cmd = new SqlCommand("SearchSoldInformation", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                        cmd.Parameters.AddWithValue("@condition", searchCondition);
                        cmd.Parameters.AddWithValue("@searchFor",searchFor);        
                }                              
                else if (load == LoadFunction.SearchBetweenDate) 
                {
                    /*Search Between two Dates => for General Sold  and  Sold Details*/ 
                    cmd = new SqlCommand("SearchSoldBetweenDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);              
                }
                else if(load == LoadFunction.SearchSoldConditionWithDates)
                {
                    /*Search Between two Dates And Combine with General Search (SoldID, Product Name,...)*/
                    cmd = new SqlCommand("SearchSoldConditionWithDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                    cmd.Parameters.AddWithValue("@condition",searchCondition);
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchSoldConditionWithYear)
                {                    
                    cmd = new SqlCommand("SearchSoldConditionWithYear", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@searchType", cboSearchType.SelectedIndex);
                    cmd.Parameters.AddWithValue("@condition",searchCondition);
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchSoldWithYear)
                {
                    cmd = new SqlCommand("SearchSoldWithYear", con);
                    cmd.CommandType = CommandType.StoredProcedure;          
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchSoldWithYearAndMonth)
                {
                    cmd = new SqlCommand("SearchSoldWithYearAndMonth", con);
                    cmd.CommandType = CommandType.StoredProcedure;   
                    cmd.Parameters.AddWithValue("@year",selectedYear);
                    cmd.Parameters.AddWithValue("@month",selectedMonth);
                    cmd.Parameters.AddWithValue("@searchFor", searchFor);
                }
                else if (load == LoadFunction.SearchSoldConditionWithYearAndMonth)
                {
                    cmd = new SqlCommand("SearchSoldConditionWithYearAndMonth", con);
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
                dgvSoldDetail.DataSource = dt;
               
                /*Align Header to Middle Center*/
                foreach (DataGridViewColumn col in dgvSoldDetail.Columns)
                {
                    Controller.AlignHeaderTextCenter(col);
                }

                /*Align Column Date to middle*/                
                dgvSoldDetail.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvSoldDetail.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvSoldDetail.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSoldDetail.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                               
                /*Display And Format all Sold Detail Information Columns*/
                if (searchFor == 1)
                {
                    /*Align number colums to Middle Right*/
   
                    int lastCol = dgvSoldDetail.Columns.Count - 1;
                    /*Sub Amount Column*/
                    dgvSoldDetail.Columns[lastCol].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvSoldDetail.Columns[lastCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*% Discount Column*/
                    dgvSoldDetail.Columns[lastCol -1].DefaultCellStyle.Format = "##0.00 %";
                    dgvSoldDetail.Columns[lastCol -1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*% Tax Column*/
                    dgvSoldDetail.Columns[lastCol - 2].DefaultCellStyle.Format = "##0.00 %";
                    dgvSoldDetail.Columns[lastCol - 2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*Unit Price Column*/
                    dgvSoldDetail.Columns[lastCol - 3].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvSoldDetail.Columns[lastCol - 3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    /*Quantity Column*/
                    dgvSoldDetail.Columns[lastCol - 4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
                }
                else
                {
                    int lastCol = dgvSoldDetail.Columns.Count - 1;
                    dgvSoldDetail.Columns[lastCol].DefaultCellStyle.Format = "#,##0.00 R";
                    dgvSoldDetail.Columns[lastCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;                    
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
    }
}
