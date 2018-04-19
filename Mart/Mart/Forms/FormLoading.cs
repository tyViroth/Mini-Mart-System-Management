using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart.Forms
{
    public partial class FormLoading : Form
    {             
         public Action worker{ get; set; }
         public FormLoading(Action worker)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            if (worker == null)
                throw new ArgumentNullException();
            this.worker = worker;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Factory.StartNew(worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());                   
        }
    }
}
