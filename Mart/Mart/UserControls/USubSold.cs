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

namespace Mart
{
    public partial class USubSold : UserControl
    {
        private static USubSold _instance;

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
            this.Load += UReport_Load;
            cboYear.SelectedIndexChanged += cboYear_SelectedIndexChanged;

            chartSoldPie.MouseMove += DoChartMoved;
            chartSoldPie.MouseClick += DoChartClicked;

            chartSoldBar.MouseMove += DoChartMoved;
            chartSoldBar.MouseClick += DoChartClicked;

            pbRefresh.Click += DoPictureBoxClicked;
            pbBack.Click += pbBack_Click;

            pbBack.MouseHover += DoPictureBoxHover;
            pbRefresh.MouseHover += DoPictureBoxHover;           
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

            SoldProductDetailEntity db = new SoldProductDetailEntity();
            BindingSource bs = new BindingSource(db.GetSoldDayDetails(date).ToList(), null);
            chartSoldBar.DataSource = bs;
            chartSoldBar.Series[0].XValueMember = "proname";
            chartSoldBar.Series[0].XValueType = ChartValueType.String;
            chartSoldBar.Series[0].YValueMembers = "amount";
            chartSoldBar.Series[0].YValueType = ChartValueType.Double;

            chartSoldPie.DataSource = bs;
            chartSoldPie.Series[0].XValueMember = "proname";
            chartSoldPie.Series[0].XValueType = ChartValueType.String;
            chartSoldPie.Series[0].YValueMembers = "amount";
            chartSoldPie.Series[0].YValueType = ChartValueType.Double;

            string titles = "Report on " + date.ToString("dd-MMM-yyyy");
            chartSoldBar.Titles[0].Text = titles;
            chartSoldPie.Titles[0].Text = titles;
        }

        private void GetDailySoldByMonth(string monthSelected, int yearSelected)
        {
            if (monthSelected.Trim() != "" && yearSelected != 0)
            {
                DailySoldEntities db9 = new DailySoldEntities();
                BindingSource bs = new BindingSource(db9.GetDailySold(monthSelected.Trim(), yearSelected).ToList(), null);
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
        }

        void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedIndex == -1) return;
            int.TryParse(cboYear.SelectedValue.ToString(), out yearSelected);
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
                SoldMonthlyEntities db = new SoldMonthlyEntities();
                BindingSource bs = new BindingSource(db.GetSoldMonthly(year).ToList(), null);
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
        }

        void UReport_Load(object sender, EventArgs e)
        {
            LoadMainReport();
        }

        private void LoadMainReport()
        {
            SoldYearEntity db = new SoldYearEntity();
            BindingSource bs = new BindingSource(db.GetSoldByYear().ToList(), null);

            cboYear.DataSource = bs;
            cboYear.DisplayMember = "year";
            cboYear.ValueMember = "year";
            cboYear.SelectedIndex = -1;
           
            chartSoldPie.DataSource = bs;
            chartSoldPie.Series[0].XValueMember = "year";
            chartSoldPie.Series[0].XValueType = ChartValueType.Int32;
            chartSoldPie.Series[0].YValueMembers = "amount";
            chartSoldPie.Series[0].YValueType = ChartValueType.Double;


            chartSoldBar.DataSource = db.GetSoldByYear().ToList();
            chartSoldBar.Series[0].XValueMember = "year";
            chartSoldBar.Series[0].XValueType = ChartValueType.Int32;
            chartSoldBar.Series[0].YValueMembers = "amount";
            chartSoldBar.Series[0].YValueType = ChartValueType.Double;

            /* Set Tittle to Report*/
            string titles = "All Sold Report";
            chartSoldPie.Titles[0].Text = titles;
            chartSoldBar.Titles[0].Text = titles;

            selectedYear = false;
            selectedMonth = false;
            selectedDay = false;
        }

    }
}
