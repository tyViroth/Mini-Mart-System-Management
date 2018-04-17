using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Mart.DataModel;
using System.Data.SqlClient;
using System.Globalization;
using Mart.Forms;

namespace Mart
{
    public partial class USubSold : UserControl
    {
        private static USubSold _instance;
        private const string PIXEL_POINT_WIDTH = "50";

        private enum LoadFunction
        {
            YEARLY,
            MONTHLY,
            DAILY,
            SEARCH
        };

        private bool selectedYear = false;
        private bool selectedMonth = false;
        private bool selectedDay = false;
        private int yearSelected = 0;
        private string monthSelected = "";
        private string daySelected = "";
        public static USubSold Instance
        {            
            get
            {
                if (_instance == null)
                    _instance = new USubSold();
                return _instance;
            }
        }
        public USubSold()
        {
            InitializeComponent();
            LoadMainReport();
            /*
                using (frmLoading frm = new frmLoading(LoadMainReport))
                {
                    frm.ShowInTaskbar = false;
                    frm.ShowDialog(this);
                }            
            */

            Controller.FillComboBoxValue(cboSellerChart, "empID", "empName", "SetEmployeeToComboBox");
            cboSellerChart.SelectedIndex = -1;

            Controller.FillComboBoxValue(cboSellerTable, "empID", "empName", "SetEmployeeToComboBox");
            cboSellerTable.SelectedIndex = -1;

            Controller.FillComboBoxValue(cboSellerDetail, "empID", "empName", "SetEmployeeToComboBox");
            cboSellerDetail.SelectedIndex = -1;                      

            tabControlReport.SelectedIndexChanged += tabControlReport_SelectedIndexChanged;
            cboYearChart.SelectedIndexChanged += cboYear_SelectedIndexChanged;
            cboSellerChart.SelectedIndexChanged += cboSellerChart_SelectedIndexChanged;

            chartSoldPie.MouseMove += DoChartMoved;
            chartSoldPie.MouseClick += DoChartClicked;

            chartSoldBar.MouseMove += DoChartMoved;
            chartSoldBar.MouseClick += DoChartClicked;

            pbRefresh.Click += DoPictureBoxClicked;
            pbBack.Click += pbBack_Click;

            pbBack.MouseHover += DoPictureBoxHover;
            pbRefresh.MouseHover += DoPictureBoxHover;     
      
            /*Tab Page 2 "Display as Table"*/
            dgvSoldYear.SelectionChanged += dgvYear_SelectionChanged;
            dgvSoldMonth.SelectionChanged += dgvMonth_SelectionChanged;

            dgvProductDetail.KeyPress += dgvProductDetail_KeyPress;
            pbRefreshTable.Click += pbRefreshTable_Click;
            cboSellerTable.SelectedIndexChanged += cboSellerTable_SelectedIndexChanged;
            cboYearDetail.SelectedIndexChanged += cboYearDetail_SelectedIndexChanged;
            cboMonthDetail.SelectedIndexChanged += cboMonthDetail_SelectedIndexChanged;
            cboSellerDetail.SelectedIndexChanged += cboSellerDetail_SelectedIndexChanged;

            pbSearch.Click += pbSearch_Click;
            pbSearch.MouseHover += pbSearch_MouseHover;
            pbRefreshDetail.Click += pbRefreshDetail_Click;

        }

        void cboSellerDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYearDetail.SelectedIndex == -1 && cboMonthDetail.SelectedIndex == -1)
            {
                LoadReportToDataGrid(LoadFunction.YEARLY);
            }
            else if (cboYearDetail.SelectedIndex != -1 && cboMonthDetail.SelectedIndex == -1)
            {
                LoadReportToDataGrid(LoadFunction.MONTHLY);
            }
            else if (cboYearDetail.SelectedIndex != -1 && cboMonthDetail.SelectedIndex != -1)
            {
                LoadReportToDataGrid(LoadFunction.DAILY);
            }
        }

        void pbRefreshDetail_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            cboMonthDetail.DataSource = null;
            cboMonthDetail.Items.Clear();
            cboYearDetail.SelectedIndex = -1;
            cboMonthDetail.SelectedIndex = -1;
            cboSellerDetail.SelectedIndex = -1;
            LoadReportToDataGrid(LoadFunction.YEARLY);
        }

        void pbSearch_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip((PictureBox)sender, "Search");
        }

        void pbSearch_Click(object sender, EventArgs e)
        {
            if (dtpFromDate.Value.Date <= dtpToDate.Value.Date)
            {
                LoadReportToDataGrid(LoadFunction.SEARCH);
            }
            else
            {
                MessageBox.Show("The From Date cannot be after the To Date","Required",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        void dgvProductDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                for (int i = 0; i < (dgvProductDetail.Rows.Count); i++)
                {
                    if (dgvProductDetail.Rows[i].Cells[0].Value.ToString().ToLower().StartsWith(e.KeyChar.ToString().ToLower(), true, CultureInfo.InvariantCulture))
                    {                        
                        dgvProductDetail.ClearSelection();
                        dgvProductDetail.Rows[i].Cells[0].Selected = true;
                        return;
                    }
                }
            }
        }

        void cboMonthDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMonthDetail.SelectedIndex == -1) return;
            int month = 0;
            int.TryParse(cboMonthDetail.SelectedValue.ToString(),out month);
            if(month != 0) LoadReportToDataGrid(LoadFunction.DAILY);
        }
        void cboYearDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYearDetail.SelectedIndex == -1) return;            
            try
            {
                int year = 0;
                int.TryParse(cboYearDetail.SelectedValue.ToString(), out year);                               
                if (year != 0)
                {
                    Controller.FillComboBoxValue(cboMonthDetail, "monthNumber", "monthName", "GetSoldDetailMonthText", year);
                    cboMonthDetail.SelectedIndex = -1;
                } 
            }
            catch (Exception ex){ }
            LoadReportToDataGrid(LoadFunction.MONTHLY);
        }

        void cboSellerTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSellerTable.SelectedIndex <= -1) return;
            LoadReportIntoTable();
        }

        void pbRefreshTable_Click(object sender, EventArgs e)
        {
            cboSellerTable.SelectedIndex = -1;
            LoadReportIntoTable();
        }

        void dgvMonth_SelectionChanged(object sender, EventArgs e)
        {
            int index = dgvSoldMonth.CurrentRow.Index;
            int year = int.Parse(dgvSoldYear.Rows[dgvSoldYear.CurrentRow.Index].Cells[0].Value.ToString());
            string month = dgvSoldMonth.Rows[index].Cells[0].Value.ToString();
            lblDayTitle.Text = string.Format("Report on {0}, {1}",month,year);
            
            SoldEntities dbSold = new SoldEntities();
            int empID = (cboSellerTable.SelectedIndex == -1) ? -1 : (int)cboSellerTable.SelectedValue;           
            dgvSoldDay.DataSource = dbSold.GetDailySold(month,year,empID).ToList();
            RefreshTotal();
        }

        void dgvYear_SelectionChanged(object sender, EventArgs e)
        {
            int index = dgvSoldYear.CurrentRow.Index;
            lblMonthTitle.Text = string.Format("Report in {0}",dgvSoldYear.Rows[index].Cells[0].Value.ToString());

            SoldEntities dbSold = new SoldEntities();
            int empID = (cboSellerTable.SelectedIndex == -1) ? -1 : (int)cboSellerTable.SelectedValue;
            dgvSoldMonth.DataSource = dbSold.GetMonthlySold(int.Parse(dgvSoldYear.Rows[index].Cells[0].Value.ToString()),empID).ToList();
            RefreshTotal();
        }

        void tabControlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlReport.SelectedTab == tabPage1)
            {
                LoadMainReport();
            }
            else if (tabControlReport.SelectedTab == tabPage2)
            {
                LoadReportIntoTable();
                LoadReportToDataGrid(LoadFunction.YEARLY);
            }
        }

        private void LoadReportIntoTable()
        {
            int empID = -1;
            try
            {
                SoldEntities dbSold = new SoldEntities();                
                empID = (cboSellerTable.SelectedIndex == -1) ? -1 : (int)cboSellerTable.SelectedValue;
                dgvSoldYear.DataSource = dbSold.GetYearlySold(empID).ToList();
                dgvSoldYear.Columns[1].DefaultCellStyle.Format = "#,##0.00 R";
                dgvSoldYear.Columns[0].HeaderText = "Year";
                dgvSoldYear.Columns[1].HeaderText = "Amount";
                Controller.AlignHeaderTextCenter(dgvSoldYear.Columns[0], dgvSoldYear.Columns[1]);
 

                int cellSelectedYear = (dgvSoldYear.RowCount == 0) ? 0 : (int)dgvSoldYear.CurrentRow.Cells[0].Value;
                dgvSoldMonth.DataSource = dbSold.GetMonthlySold(cellSelectedYear, empID).ToList();
                dgvSoldMonth.Columns[1].DefaultCellStyle.Format = "#,##0.00 R";
                dgvSoldMonth.Columns[0].HeaderText = "Month";
                dgvSoldMonth.Columns[1].HeaderText = "Amount";
                Controller.AlignHeaderTextCenter(dgvSoldMonth.Columns[0], dgvSoldMonth.Columns[1]); 

                string cellSelectedMonth = (dgvSoldMonth.RowCount == 0) ? "" : dgvSoldMonth.CurrentRow.Cells[0].Value.ToString();
                dgvSoldDay.DataSource = dbSold.GetDailySold(cellSelectedMonth, cellSelectedYear, empID);
                dgvSoldDay.Columns[1].DefaultCellStyle.Format = "#,##0.00 R";
                dgvSoldDay.Columns[0].HeaderText = "Day";
                dgvSoldDay.Columns[1].HeaderText = "Amount";
                Controller.AlignHeaderTextCenter(dgvSoldDay.Columns[0],dgvSoldDay.Columns[1]);               
                
                
                dgvSoldYear.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSoldMonth.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSoldDay.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;   

                /*Product Detail*/
                cboYearDetail.DataSource = dbSold.GetYearlySold(empID).ToList();
                cboYearDetail.DisplayMember = "year";
                cboYearDetail.ValueMember = "year";
                cboYearDetail.SelectedIndex = -1;

                RefreshTotal();
            }
            catch (Exception){  }                     
        }

        private void LoadReportToDataGrid(LoadFunction type)
        {
            SqlConnection con = Connection.getConnection();
            dgvProductDetail.Columns.Clear();
            SqlCommand cmd = null;
            try
            {
                int soldYear = (cboYearDetail.SelectedIndex == -1) ? -1 : (int)cboYearDetail.SelectedValue;
                int soldMonth = (cboMonthDetail.SelectedIndex == -1) ? -1 : (int)cboMonthDetail.SelectedValue;
                int empID = (cboSellerDetail.SelectedIndex == -1) ? -1 : (int)cboSellerDetail.SelectedValue; 
                
                con.Open();
                if (type == LoadFunction.YEARLY)
                {
                    cmd = new SqlCommand("GetTableSoldByYear", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empID", empID);
                }
                else if (type == LoadFunction.MONTHLY)
                {
                    cmd = new SqlCommand("GetTableSoldByMonth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empID", empID);
                    cmd.Parameters.AddWithValue("@soldYear",soldYear);
                }
                else if (type == LoadFunction.DAILY)
                {
                    cmd = new SqlCommand("GetTableSoldByDay", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empID", empID);
                    cmd.Parameters.AddWithValue("@soldYear", soldYear);
                    cmd.Parameters.AddWithValue("@soldMonth", soldMonth);
                }
                else if (type == LoadFunction.SEARCH)
                {
                    cmd = new SqlCommand("GetSoldBetweenDates",con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate",dtpFromDate.Value);
                    cmd.Parameters.AddWithValue("@toDate",dtpToDate.Value);
                    cmd.Parameters.AddWithValue("@empID",empID);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtProduct = new DataTable();
                da.Fill(dtProduct);                
                
                if (dtProduct.Rows.Count != 0)
                {
                    /*Add new Column to DataTable */
                    dtProduct.Columns.Add("Total", typeof(double));

                    /*Begin Total Amount of Each Rows*/
                    foreach (DataRow row in dtProduct.Rows)
                    {
                        double rowSum = 0;
                        foreach (DataColumn col in dtProduct.Columns)
                        {
                            if (!row.IsNull(col))
                            {
                                double value;
                                if (double.TryParse(row[col].ToString().Trim(), out value))
                                    rowSum += value;
                            }
                        }
                        row.SetField("Total", rowSum);
                    }
                    /*End Total Amount of Each Rows*/


                    /*Begin Total Amount of Each Columns of Datatable*/
                    var newRow = dtProduct.NewRow();
                    for (int i = 0; i < dtProduct.Columns.Count; i++)
                    {
                        if (i == 0) newRow[i] = "Total";
                        else
                        {
                            double ColumnTotal = 0;
                            foreach (DataRow dr in dtProduct.Rows)
                            {
                                double value = 0;
                                double.TryParse(dr[i].ToString(), out value);
                                ColumnTotal += value;
                            }
                            newRow[i] = ColumnTotal;
                        }
                    }
                    dtProduct.Rows.Add(newRow);
                    /*End Total Amount of Each Columns of Datatable*/
                }
                              
                dgvProductDetail.DataSource = dtProduct;
                dgvProductDetail.DataError += dgvProductDetail_DataError;          

                if(dgvProductDetail.Rows.Count > 0)
                {
                    dgvProductDetail.DefaultCellStyle.NullValue = "0.00 R";

                    foreach (DataGridViewColumn col in dgvProductDetail.Columns)
                    {
                        if (col.Index == 0) continue;
                        dgvProductDetail.Columns[col.Index].DefaultCellStyle.Format = "#,##0.00 R";
                        dgvProductDetail.Columns[col.Index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    foreach (DataGridViewColumn col in dgvProductDetail.Columns)
                    {
                        Controller.AlignHeaderTextCenter(col);
                    }

                    Controller.NonSortableDataGridView(dgvProductDetail);

                    /*Set Total ROW to BOLD Text*/
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.Font = new Font(dgvProductDetail.Font, FontStyle.Bold);
                    dgvProductDetail.Rows[dgvProductDetail.Rows.Count - 1].DefaultCellStyle = style;
                }                                                 
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message,"LOAD FUNCTION");
            }
            finally
            {
                con.Close();
            }
        }

        void dgvProductDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error DataGrid","DataGridView");
        }

        private void RefreshTotal()
        {
            double totalYear = 0;
            double totalMonth = 0;
            double totalDay = 0;
            
            foreach (DataGridViewRow row in dgvSoldYear.Rows)
            {
                totalYear += double.Parse(row.Cells[1].Value.ToString());
            }          
            
            foreach (DataGridViewRow row in dgvSoldMonth.Rows)
            {
                totalMonth += double.Parse(row.Cells[1].Value.ToString());
            }
           
            foreach (DataGridViewRow row in dgvSoldDay.Rows)
            {
                totalDay += double.Parse(row.Cells[1].Value.ToString());
            }
            lblYear.Text = string.Format("Total : {0:#, ##0.00 R}", totalYear);
            lblMonth.Text = string.Format("Total : {0:#, ##0.00 R}", totalMonth);           
            lblDay.Text = string.Format("Total : {0:#, ##0.00 R}", totalDay);
        }

        #region .................CHART REPORT...................
        /* @ @ @ @ @ @ @ @ @ @ @ @ ... C H A R T  R E P O R T ...@ @ @ @ @ @ @ @ @ @ */

        /*...............................*/

        void cboSellerChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSellerChart.SelectedIndex == -1) return;
            LoadMainReport();
        }

        void DoPictureBoxHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            if (sender == pbBack)
            {
                tt.SetToolTip((PictureBox)sender,"Go Back");
            }
            else
            {
                tt.SetToolTip((PictureBox)sender, "Refresh");            
            }
        }

        void pbBack_Click(object sender, EventArgs e)
        {
            /* This section is to call any function to Previous Step */
            if (selectedYear && !selectedMonth && !selectedDay)
            {
                /*User now already selected a Year*/
                DoPictureBoxClicked(pbRefresh, e);  /* Refresh => it means get to the first LOAD */
                selectedYear = false;
            }
            else if (selectedYear && selectedMonth && !selectedDay)
            {
                /*User now already selected A month*/
                selectedMonth = false;
                GetMonthlySoldByYear(yearSelected);
            }
            else if (selectedYear && selectedMonth && selectedDay)
            {
                /*User now already selected A specific Day of Month*/
                selectedDay = false;
                GetDailySoldByMonth(monthSelected, yearSelected);
            }
        }

        void DoPictureBoxClicked(object sender, EventArgs e)
        {
            cboSellerChart.SelectedIndex = -1;
            LoadMainReport();
        }

        private void DoChartMoved(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            if (!selectedDay)
            {
                /* It is not allowed to clicked if User navigate until the specific DAY of Month */
                HitTestResult hit = chart.HitTest(e.X, e.Y);
                var dp = hit.Object as DataPoint;
                Cursor = (dp == null) ? Cursors.Default : Cursors.Hand;
            }
            else Cursor = Cursors.Default;
        }

        void DoChartClicked(object sender, MouseEventArgs e)
        {
            Chart chart = ((Chart)sender);
            HitTestResult hit = chart.HitTest(e.X, e.Y, ChartElementType.DataPoint);
            if (hit.PointIndex >= 0 && Cursor == Cursors.Hand)
            {
                DataPoint dp = chart.Series[0].Points[hit.PointIndex];

                /*User hasn't selected any option yet*/
                if (!selectedYear && !selectedMonth && !selectedDay)
                {
                    selectedYear = true;
                    string selectedText = chart.Series[0].Points[hit.PointIndex].XValue.ToString();
                    int.TryParse(selectedText, out yearSelected);
                    GetMonthlySoldByYear(yearSelected);

                    /*Set combox value to match the clicked value on Pie or Bar chart*/
                    if(cboYearChart.Items.Count > 0 ) cboYearChart.SelectedValue = yearSelected;
                }
                /*User has selected Y E A R option already*/
                else if (selectedYear && !selectedMonth && !selectedDay)
                {
                    selectedMonth = true;
                    monthSelected = chart.Series[0].Points[hit.PointIndex].AxisLabel.ToString();
                    GetDailySoldByMonth(monthSelected, yearSelected);
                }
                /*User has selected M O N T H already*/
                else if (selectedYear && selectedMonth && !selectedDay)
                {
                    selectedDay = true;
                    daySelected = chart.Series[0].Points[hit.PointIndex].XValue.ToString();
                    DateTime date = DateTime.Parse(daySelected + monthSelected + yearSelected);                
                    GetProductNameDetails(date);
                }
            }
        }
        
        private void GetProductNameDetails(DateTime date)
        {
            if (date == null) return;
            try
            {
                SoldEntities dbSold = new SoldEntities();
                int empID = (cboSellerChart.SelectedIndex == -1) ? -1 : (int)cboSellerChart.SelectedValue;            
                BindingSource bs = new BindingSource(dbSold.GetDayDetailsSold(date,empID).ToList(), null);
                chartSoldBar.DataSource = bs;
                chartSoldBar.Series[0].XValueMember = "proname";
                chartSoldBar.Series[0].XValueType = ChartValueType.String;
                chartSoldBar.Series[0].YValueMembers = "amount";
                chartSoldBar.Series[0].YValueType = ChartValueType.Double;
                chartSoldBar.Series[0]["PixelPointWidth"] = PIXEL_POINT_WIDTH;

                chartSoldPie.DataSource = bs;
                chartSoldPie.Series[0].XValueMember = "proname";
                chartSoldPie.Series[0].XValueType = ChartValueType.String;
                chartSoldPie.Series[0].YValueMembers = "amount";
                chartSoldPie.Series[0].YValueType = ChartValueType.Double;

                string titles = "Report on " + date.ToString("dd-MMM-yyyy");
                chartSoldBar.Titles[0].Text = titles;
                chartSoldPie.Titles[0].Text = titles;         
            }
            catch (Exception){ } 
        }

        private void GetDailySoldByMonth(string monthSelected, int yearSelected)
        {
            if (monthSelected.Trim() != "" && yearSelected != 0)
            {
                try
                {
                    SoldEntities dbSold = new SoldEntities();
                    int empID = (cboSellerChart.SelectedIndex == -1) ? -1 : (int)cboSellerChart.SelectedValue;            

                    BindingSource bs = new BindingSource(dbSold.GetDailySold(monthSelected.Trim(), yearSelected,empID).ToList(), null);
                    chartSoldPie.DataSource = bs;
                    chartSoldPie.Series[0].XValueMember = "days";
                    chartSoldPie.Series[0].XValueType = ChartValueType.Int32;
                    chartSoldPie.Series[0].YValueMembers = "amount";
                    chartSoldPie.Series[0].YValueType = ChartValueType.Double;
                
                    chartSoldBar.DataSource = bs;
                    chartSoldBar.Series[0].XValueMember = "days";
                    chartSoldBar.Series[0].XValueType = ChartValueType.Int32;
                    chartSoldBar.Series[0].YValueMembers = "amount";
                    chartSoldBar.Series[0].YValueType = ChartValueType.Double;               

                    string titles = string.Format("Daily Sold Report on {0}, {1}", monthSelected, yearSelected);
                    chartSoldBar.Titles[0].Text = titles;
                    chartSoldPie.Titles[0].Text = titles; 
                }
                catch (Exception){   }              
            }
        }

        void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYearChart.SelectedIndex == -1) return;
            int.TryParse(cboYearChart.SelectedValue.ToString(), out yearSelected);
            if (yearSelected > 0)
            {
                GetMonthlySoldByYear(yearSelected);
                selectedYear = true; /* Set Clicked */
                selectedMonth = false;
                selectedDay = false;
            }
        }

        private void GetMonthlySoldByYear(int year)
        {
            if (year > 0)
            {
                try
                {
                    SoldEntities dbSold = new SoldEntities();
                    int empID = (cboSellerChart.SelectedIndex == -1) ? -1 : (int)cboSellerChart.SelectedValue;

                    BindingSource bs = new BindingSource(dbSold.GetMonthlySold(year, empID).ToList(), null);

                    chartSoldPie.DataSource = bs;
                    chartSoldPie.Series[0].XValueMember = "month";
                    chartSoldPie.Series[0].XValueType = ChartValueType.String;
                    chartSoldPie.Series[0].YValueMembers = "amount";
                    chartSoldPie.Series[0].YValueType = ChartValueType.Double;

                    chartSoldBar.DataSource = bs;
                    chartSoldBar.Series[0].XValueMember = "month";
                    chartSoldBar.Series[0].XValueType = ChartValueType.String;
                    chartSoldBar.Series[0].YValueMembers = "amount";
                    chartSoldBar.Series[0].YValueType = ChartValueType.Double;

                    /* Set Tittle to Report*/
                    string titles = "Monthly Sold Report of " + year;
                    chartSoldPie.Titles[0].Text = titles;
                    chartSoldBar.Titles[0].Text = titles;             
                }
                catch (Exception)
                {
                } 
            }
        }      

        private void LoadMainReport()
        {            
            try
            {               
                SoldEntities dbSold = new SoldEntities();
                int empID = (cboSellerChart.SelectedIndex == -1) ? -1 : (int)cboSellerChart.SelectedValue;
                BindingSource bs = new BindingSource(dbSold.GetYearlySold(empID).ToList(), null);

                cboYearChart.DataSource = bs;
                cboYearChart.DisplayMember = "year";
                cboYearChart.ValueMember = "year";
                cboYearChart.SelectedIndex = -1;

                chartSoldPie.DataSource = bs;
                chartSoldPie.Series[0].XValueMember = "year";
                chartSoldPie.Series[0].XValueType = ChartValueType.Int32;
                chartSoldPie.Series[0].YValueMembers = "amount";
                chartSoldPie.Series[0].YValueType = ChartValueType.Double;

                chartSoldBar.DataSource = bs;
                chartSoldBar.Series[0].XValueMember = "year";
                chartSoldBar.Series[0].XValueType = ChartValueType.Int32;
                chartSoldBar.Series[0].YValueMembers = "amount";
                chartSoldBar.Series[0].YValueType = ChartValueType.Double;

                /*Set Style to Chart Bar*/
                chartSoldBar.Series[0]["PixelPointWidth"] = PIXEL_POINT_WIDTH;
                chartSoldBar.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
                chartSoldBar.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                chartSoldBar.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
                chartSoldBar.ChartAreas[0].AxisX2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                /*End Set Style to Chart Bar*/

                /* Set Tittle to Report*/
                string titles = "All Sold Report";
                chartSoldPie.Titles[0].Text = titles;
                chartSoldBar.Titles[0].Text = titles;
                selectedYear = false;
                selectedMonth = false;
                selectedDay = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Loading Main Report");
            }
        }
        #endregion End of Chart Report

    }
}
