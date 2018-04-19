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
    public partial class USetting : UserControl
    {
        private static USetting _instance;

        public static USetting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new USetting();
                return _instance;
            }
        }
        public USetting()
        {
            InitializeComponent();
        }
    }
}
