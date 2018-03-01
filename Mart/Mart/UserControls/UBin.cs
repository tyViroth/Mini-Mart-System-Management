using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.UserControls;

namespace Mart
{
    public partial class UBin : UserControl
    {
        private static UBin _instance;

        public static UBin Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UBin();
                return _instance;
            }
        }
        public UBin()
        {
            InitializeComponent();
            tpEmployee.Click += tpEmployee_Click;                
        }

        void tpEmployee_Click(object sender, EventArgs e)
        {
            UBinEmployee binEmp = null;
            if (binEmp == null)
            {
                binEmp = new UBinEmployee();
            }
            binEmp.Dock = DockStyle.Fill;
            tpEmployee.Controls.Add(binEmp);          
        }

        private void UBin_Load(object sender, EventArgs e)
        {
            tpEmployee_Click(tpEmployee, e);
        }

    }
}
