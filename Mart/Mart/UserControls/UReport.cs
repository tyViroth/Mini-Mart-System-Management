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
    public partial class UReport : UserControl
    {        
        private static UReport _instance;       
        public static UReport Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UReport();
                return _instance;
            }
        }
        public UReport()
        {
            InitializeComponent();
            this.Load += UReport_Load;
            rdSold.CheckedChanged += rdSold_CheckedChanged;
            rdImport.CheckedChanged += rdSold_CheckedChanged;            
        }

        void rdSold_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rdSold)
            {
                if (rdSold.Checked)
                {
                    AddUSupSoldIntoPanel();
                }
            }
            else
            {

            }
        }

        private void AddUSupSoldIntoPanel()
        {
            USubSold subSold = null;
            if (subSold == null)
            {
                subSold = new USubSold();
            }
            subSold.Dock = DockStyle.Fill;
            pContainer.Controls.Add(subSold);
        }
      
        void UReport_Load(object sender, EventArgs e)
        {
            rdSold.Checked = true;
            AddUSupSoldIntoPanel();
        }
               
    }
}
