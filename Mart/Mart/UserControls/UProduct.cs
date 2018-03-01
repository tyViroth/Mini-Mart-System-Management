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

namespace Mart
{
    public partial class UProduct : UserControl
    {
        SqlConnection cnn;
        private static UProduct _instance;

        public static UProduct Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UProduct();
                return _instance;
            }
        }
        public UProduct()
        {
            InitializeComponent();

            
            cnn = Connection.getConnection();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM  Product",cnn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvProduct.DataSource = dt;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }
            
        }

      
        
        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
