namespace Mart.UserControls
{
    partial class USelling
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSalesHistory = new System.Windows.Forms.Label();
            this.lblSellProduct = new System.Windows.Forms.Label();
            this.pContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pContainer, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(841, 458);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(153)))), ((int)(((byte)(168)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblSalesHistory);
            this.panel1.Controls.Add(this.lblSellProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(841, 45);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(333, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 40);
            this.label3.TabIndex = 2;
            this.label3.Text = "SALES INFORMATION";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSalesHistory
            // 
            this.lblSalesHistory.BackColor = System.Drawing.Color.Transparent;
            this.lblSalesHistory.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesHistory.Image = global::Mart.Properties.Resources.Sales_history_32;
            this.lblSalesHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSalesHistory.Location = new System.Drawing.Point(140, 5);
            this.lblSalesHistory.Name = "lblSalesHistory";
            this.lblSalesHistory.Size = new System.Drawing.Size(129, 35);
            this.lblSalesHistory.TabIndex = 1;
            this.lblSalesHistory.Text = "Sales History";
            this.lblSalesHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSellProduct
            // 
            this.lblSellProduct.BackColor = System.Drawing.Color.Transparent;
            this.lblSellProduct.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSellProduct.Image = global::Mart.Properties.Resources.Sell_32;
            this.lblSellProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSellProduct.Location = new System.Drawing.Point(5, 5);
            this.lblSellProduct.Name = "lblSellProduct";
            this.lblSellProduct.Size = new System.Drawing.Size(129, 35);
            this.lblSellProduct.TabIndex = 0;
            this.lblSellProduct.Text = "Sell Product";
            this.lblSellProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pContainer
            // 
            this.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContainer.Location = new System.Drawing.Point(0, 45);
            this.pContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pContainer.Name = "pContainer";
            this.pContainer.Size = new System.Drawing.Size(841, 413);
            this.pContainer.TabIndex = 1;
            // 
            // USelling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(174)))), ((int)(((byte)(190)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "USelling";
            this.Size = new System.Drawing.Size(841, 458);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSellProduct;
        private System.Windows.Forms.Label lblSalesHistory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pContainer;
    }
}
