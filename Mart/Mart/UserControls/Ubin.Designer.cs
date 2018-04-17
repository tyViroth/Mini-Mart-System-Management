﻿namespace Mart
{
    partial class UBin
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
            this.tc = new System.Windows.Forms.TabControl();
            this.tpEmployee = new System.Windows.Forms.TabPage();
            this.tpRole = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tpImportStock = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tpSoldProduct = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tpProduct = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tpCategory = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.tpSupplier = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpRole.SuspendLayout();
            this.tpImportStock.SuspendLayout();
            this.tpSoldProduct.SuspendLayout();
            this.tpProduct.SuspendLayout();
            this.tpCategory.SuspendLayout();
            this.tpSupplier.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tc, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 19);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(780, 472);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpEmployee);
            this.tc.Controls.Add(this.tpRole);
            this.tc.Controls.Add(this.tpImportStock);
            this.tc.Controls.Add(this.tpSoldProduct);
            this.tc.Controls.Add(this.tpProduct);
            this.tc.Controls.Add(this.tpCategory);
            this.tc.Controls.Add(this.tpSupplier);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Margin = new System.Windows.Forms.Padding(0);
            this.tc.Multiline = true;
            this.tc.Name = "tc";
            this.tc.Padding = new System.Drawing.Point(0, 0);
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(780, 472);
            this.tc.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tc.TabIndex = 1;
            // 
            // tpEmployee
            // 
            this.tpEmployee.Location = new System.Drawing.Point(4, 26);
            this.tpEmployee.Margin = new System.Windows.Forms.Padding(0);
            this.tpEmployee.Name = "tpEmployee";
            this.tpEmployee.Size = new System.Drawing.Size(772, 442);
            this.tpEmployee.TabIndex = 0;
            this.tpEmployee.Text = "Employee";
            this.tpEmployee.UseVisualStyleBackColor = true;
            // 
            // tpRole
            // 
            this.tpRole.Controls.Add(this.label3);
            this.tpRole.Location = new System.Drawing.Point(4, 26);
            this.tpRole.Margin = new System.Windows.Forms.Padding(0);
            this.tpRole.Name = "tpRole";
            this.tpRole.Size = new System.Drawing.Size(772, 442);
            this.tpRole.TabIndex = 1;
            this.tpRole.Text = "Role";
            this.tpRole.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(141, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "Recovery Role";
            // 
            // tpImportStock
            // 
            this.tpImportStock.Controls.Add(this.label4);
            this.tpImportStock.Location = new System.Drawing.Point(4, 26);
            this.tpImportStock.Margin = new System.Windows.Forms.Padding(0);
            this.tpImportStock.Name = "tpImportStock";
            this.tpImportStock.Size = new System.Drawing.Size(772, 442);
            this.tpImportStock.TabIndex = 2;
            this.tpImportStock.Text = "Import Stock";
            this.tpImportStock.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(130, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Recovery Import Stock";
            // 
            // tpSoldProduct
            // 
            this.tpSoldProduct.Controls.Add(this.label5);
            this.tpSoldProduct.Location = new System.Drawing.Point(4, 26);
            this.tpSoldProduct.Margin = new System.Windows.Forms.Padding(0);
            this.tpSoldProduct.Name = "tpSoldProduct";
            this.tpSoldProduct.Size = new System.Drawing.Size(772, 442);
            this.tpSoldProduct.TabIndex = 3;
            this.tpSoldProduct.Text = "Sold Product";
            this.tpSoldProduct.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(336, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 21);
            this.label5.TabIndex = 3;
            this.label5.Text = "Recovery Sold Product";
            // 
            // tpProduct
            // 
            this.tpProduct.Controls.Add(this.label6);
            this.tpProduct.Location = new System.Drawing.Point(4, 26);
            this.tpProduct.Margin = new System.Windows.Forms.Padding(0);
            this.tpProduct.Name = "tpProduct";
            this.tpProduct.Size = new System.Drawing.Size(772, 442);
            this.tpProduct.TabIndex = 4;
            this.tpProduct.Text = "Product";
            this.tpProduct.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(336, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 21);
            this.label6.TabIndex = 3;
            this.label6.Text = "Recovery Product";
            // 
            // tpCategory
            // 
            this.tpCategory.Controls.Add(this.label7);
            this.tpCategory.Location = new System.Drawing.Point(4, 26);
            this.tpCategory.Margin = new System.Windows.Forms.Padding(0);
            this.tpCategory.Name = "tpCategory";
            this.tpCategory.Size = new System.Drawing.Size(772, 442);
            this.tpCategory.TabIndex = 5;
            this.tpCategory.Text = "Category";
            this.tpCategory.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(336, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 21);
            this.label7.TabIndex = 3;
            this.label7.Text = "Recovery Category";
            // 
            // tpSupplier
            // 
            this.tpSupplier.Controls.Add(this.label8);
            this.tpSupplier.Location = new System.Drawing.Point(4, 26);
            this.tpSupplier.Name = "tpSupplier";
            this.tpSupplier.Padding = new System.Windows.Forms.Padding(3);
            this.tpSupplier.Size = new System.Drawing.Size(772, 442);
            this.tpSupplier.TabIndex = 6;
            this.tpSupplier.Text = "Supplier";
            this.tpSupplier.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(336, 135);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 21);
            this.label8.TabIndex = 3;
            this.label8.Text = "Recovery Supplier";
            // 
            // UBin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(174)))), ((int)(((byte)(190)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UBin";
            this.Size = new System.Drawing.Size(814, 508);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpRole.ResumeLayout(false);
            this.tpRole.PerformLayout();
            this.tpImportStock.ResumeLayout(false);
            this.tpImportStock.PerformLayout();
            this.tpSoldProduct.ResumeLayout(false);
            this.tpSoldProduct.PerformLayout();
            this.tpProduct.ResumeLayout(false);
            this.tpProduct.PerformLayout();
            this.tpCategory.ResumeLayout(false);
            this.tpCategory.PerformLayout();
            this.tpSupplier.ResumeLayout(false);
            this.tpSupplier.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tpRole;
        private System.Windows.Forms.TabPage tpImportStock;
        private System.Windows.Forms.TabPage tpSoldProduct;
        private System.Windows.Forms.TabPage tpProduct;
        private System.Windows.Forms.TabPage tpCategory;
        private System.Windows.Forms.TabPage tpSupplier;
        private System.Windows.Forms.TabPage tpEmployee;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tc;





    }
}
