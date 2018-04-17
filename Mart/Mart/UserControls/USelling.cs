﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.Forms;

namespace Mart.UserControls
{
    public partial class USelling : UserControl
    {
        private static USelling _instance;

        public static USelling Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new USelling();
                return _instance;
            }
        }
        private USelling()
        {
            InitializeComponent();
            lblSalesHistory.Click += DoClick;
            lblSellProduct.Click += DoClick;

         }

        private void DoClick(object sender, EventArgs e)
        {
            lblSellProduct.ForeColor = Color.Black;
            lblSalesHistory.ForeColor = Color.Black;

            if (sender == lblSellProduct)
            {
                lblSellProduct.ForeColor = Color.White;
                if (!pContainer.Controls.Contains(USold.Instance))
                {
                    pContainer.Controls.Add(USold.Instance);
                    USold.Instance.Dock = DockStyle.Fill;
                    USold.Instance.BringToFront();
                }
                else
                {
                    USold.Instance.BringToFront();
                }
            }
            else if (sender == lblSalesHistory)
            {
                lblSalesHistory.ForeColor = Color.White;
                frmSoldInformation sold = null;
                if (sold == null) 
                    sold = new frmSoldInformation();                
                if (!pContainer.Controls.Contains(sold))
                {                    
                    sold.TopLevel = false;
                    sold.AutoScroll = true;
                    pContainer.Controls.Add(sold);                                  
                    sold.FormBorderStyle = FormBorderStyle.None;
                    sold.Dock = DockStyle.Fill;
                    sold.Show();
                    sold.BringToFront();
                }
                else
                {
                    sold.Show();
                    sold.BringToFront();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
