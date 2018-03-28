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
    public partial class SoldInfo : Form
    {
        SqlConnection con = Connection.getConnection();
        SqlCommand cmd;
        SqlDataAdapter adapt;   
        DataTable dt;
        public SoldInfo()
        {
            InitializeComponent();
            LoadData();
            Event();
            
        }

        private void Event()
        {
            btnSearch.Click += Do_Click;
            btnDelete.Click += Do_Click;
            btnEdit.Click += Do_Click;
        }
        private void Do_Click(object sender, EventArgs e)
        {
            if(sender == btnSearch)
            {
                
                
            }
        }

        private void LoadData()
        {
            try
            {
                con.Open();
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select I.proname, S.* from ImportDetail I inner join SoldDetail S on S.proID = I.proID;", con);
                adapter.Fill(dataTable);
                dgvSoldDetail.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Load Data");
            }
            
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                adapt = new SqlDataAdapter("select * from Sold", con);
                dt = new DataTable();
                adapt.Fill(dt);
                dgvSoldDetail.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
