using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.Forms;
using System.Data.SqlClient;

namespace Mart.UserControls
{
    public partial class UImportStock : UserControl
    {
        private SqlCommand cmd;
        private SqlConnection con = Connection.getConnection();
        private static UImportStock _instance;

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
            btnAdd.Click += BtnAdd_Click;
            this.Load += UImportStock_Load;
            btnRefresh.Click += BtnRefresh_Click;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void UImportStock_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            frmInsertImport frmInsertImport = new frmInsertImport();
            frmInsertImport.Closed += FrmInsertImport_Closed;

            frmInsertImport.Show();
        }

        private void FrmInsertImport_Closed(object sender, EventArgs e)
        {
            BtnRefresh_Click(btnRefresh, null);
        }

        protected void LoadDataFromDatabase()
        {
            try
            {
                con.Open();
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select * from import";

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvImport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error happened!");
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }

        private void dgvImport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
