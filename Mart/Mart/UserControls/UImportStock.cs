using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart.UserControls
{
    public partial class UImportStock : UserControl
    {
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
        }
    }
}
