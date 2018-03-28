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

namespace Mart.Forms
{
    public partial class frmLoadSoldDetail : Form
    {
        SqlConnection con = Connection.getConnection();
        SqlCommand cmd;
        SqlDataAdapter adapt;

        public frmLoadSoldDetail()
        {
            InitializeComponent();
            DisplayData();
        }

        public void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select id.impQty, p.proName, cateID from ImportDetail id inner join Product p on p.proId = id.proId where proID = 2", con);
            //adapt = new SqlDataAdapter("select impQty from ImportDetail", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}
