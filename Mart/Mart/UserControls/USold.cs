using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart
{
    public partial class USold : UserControl
    {
        private static USold _instance;

        public static USold Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new USold();
                return _instance;
            }
        }
        public USold()
        {
            InitializeComponent();
            btnAdd.Click += btnAdd_Click;
            btnSell.Click += btnSell_Click;

        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            double myvalue;
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            double _amount = 0;
            string name = txtName.Text;
            int quantity = int.Parse(txtQty.Text);
            double price = Double.Parse(txtPrice.Text);
            string _date = date.Value.ToString("yyyy-MM-dd");
            double discount = Double.Parse(txtDiscount.Text);
            double tax = Double.Parse(txtTax.Text.ToString());

            _amount = quantity * price;
            //string[] row = { name, quantity, product_id, discount, tax };
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(tblSold);
            row.Cells[1].Value = name.ToString();
            row.Cells[2].Value = quantity.ToString();
            row.Cells[3].Value = price.ToString();
            row.Cells[4].Value = _amount.ToString();
            row.Cells[5].Value = discount.ToString();
            row.Cells[6].Value = _date.ToString();
            tblSold.Rows.Add(row);


          

        }
    }
}
