using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.InstanceClasses;

namespace Mart.Forms
{
    public partial class frmInsertImport : Form
    {
        private DataTable dt = null;
        private SqlCommand cmdImp, cmdImpDatial;
        private SqlConnection con = Connection.getConnection();
        private List<ImportDetail> impDetails = new List<ImportDetail>();
        double totalAmount = 0;
        public frmInsertImport()
        {
            InitializeComponent();
            CreateTable();
          
            btnAdd.Click += BtnAdd_Click;
            btnCancel.Click += BtnCancel_Click;
            btnClose.Click += BtnClose_Click;
            btnRemove.Click += BtnRemove_Click;
            btnSave.Click += BtnSave_Click;
            btnReset.Click += BtnReset_Click;
            this.Load += FrmInsertImport_Load;
            txtImpPrice.KeyPress += AllowNumberOnly;
            txtImpQty.KeyPress += AllowNumberOnly;
            txtUnitPrice.KeyPress += AllowNumberOnly;
            
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            bool sucess = false;
            int impID;
            try
            {
                con.Open();
                cmdImp = new SqlCommand("Update Import set impDate = @impDate, supId = @supID, impTotal = @impTotal", con);
                cmdImp.Parameters.AddWithValue("@impDate", dTPImpDate.Value);
                cmdImp.Parameters.AddWithValue("@supID", (int)cboSupplier.SelectedValue);
                double totalImport = 0;
                double.TryParse(txtAmount.Text, out totalImport);
                cmdImp.Parameters.AddWithValue("@impTotal", totalImport);

                if (cmdImp.ExecuteNonQuery() > 0)
                {
                    sucess = true;
                }
            }
            catch (SqlException ex)
            {
                sucess = false;
                MessageBox.Show("There was an error hapend! :" + ex.Message);
            }
            finally
            {
                con.Close();
                cmdImp.Dispose();
            }

            impID = Controller.GetLastAutoIncrement("Import");
            bool saveImportSuccess = SaveImprotDetail(impID);

            cmdImpDatial = new SqlCommand("DELETE ImportDetail WHERE impID = @impID", con);
            cmdImpDatial.Parameters.AddWithValue("@impID", impID);
            cmdImpDatial.ExecuteNonQuery();

            if (saveImportSuccess && sucess)
            {
                MessageBox.Show("Updated suceessfully!");
                btnSave.Enabled = false;
            }
            else
            {
                try
                {
                    if (sucess)
                    {
                        cmdImp = new SqlCommand("DELETE Import WHERE impID = @impID", con);
                        cmdImp.Parameters.AddWithValue("@impID", impID);
                        cmdImpDatial = new SqlCommand("DELETE ImportDetail WHERE impID = @impID", con);
                        cmdImpDatial.Parameters.AddWithValue("@impID", impID);
                        cmdImp.ExecuteNonQuery();
                        cmdImpDatial.ExecuteNonQuery();
                    }
                    MessageBox.Show("Updated unsuceessfully!");
                }
                catch (SqlException ex)
                {

                }
            }

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            dTPImpDate.Text = null;
            cboSupplier.Text = null;
            ClearTextBox();
            dgvImpDetail.Rows.Clear();
            btnSave.Enabled = true;
        }

        private void AllowNumberOnly(object sender, KeyPressEventArgs e)
        {
            Input.InputNmber(sender as TextBox, e);
        }

        private void FrmInsertImport_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlDataAdapter daSup = new SqlDataAdapter("select supName, supID from supplier", con);
                DataTable dtSup = new DataTable();
                daSup.Fill(dtSup);
                cboSupplier.DataSource = dtSup;
                cboSupplier.DisplayMember = "supName";
                cboSupplier.ValueMember = "supID";

                SqlDataAdapter daPro = new SqlDataAdapter("select proName, proID from product", con);
                DataTable dtPro = new DataTable();
                daPro.Fill(dtPro);
                cboProduct.DataSource = dtPro;
                cboProduct.DisplayMember = "proName";
                cboProduct.ValueMember = "proID";
                cboProduct.Text = null;
                cboSupplier.Text = null;
                btnSave.Enabled = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("There was an error hapend!");
            }
            finally
            {
                con.Close();
            }
        }

        private bool SaveImprotDetail(int impID)
        {
            bool success = false;
            int proId;
            double qty, price, amount, unitPrice;
            string productName;
            int successRow = 0;
            foreach (DataGridViewRow Datarow in dgvImpDetail.Rows)
            {

                if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null && Datarow.Cells[2].Value != null && Datarow.Cells[3].Value != null && Datarow.Cells[4].Value != null && Datarow.Cells[5].Value != null)
                {
                    productName = Datarow.Cells[0].Value.ToString();
                    qty = double.Parse(Datarow.Cells[1].Value.ToString());
                    price = double.Parse(Datarow.Cells[2].Value.ToString());
                    unitPrice = double.Parse(Datarow.Cells[3].Value.ToString());
                    amount = double.Parse(Datarow.Cells[4].Value.ToString());
                    proId = int.Parse(Datarow.Cells[5].Value.ToString());

                    DataRow rw = dt.NewRow();
                    rw["impID"] = impID;
                    rw["proID"] = proId;
                    rw["impQty"] = qty;
                    rw["impPrice"] = price;
                    rw["unitPrice"] = unitPrice;
                    rw["amount"] = amount;
                    dt.Rows.Add(rw);
                }
            }

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmdImpDatial = new SqlCommand("InsertImportDetail", con);
                cmdImpDatial.CommandType = CommandType.StoredProcedure;
                cmdImpDatial.Parameters.Add("@importDetails", SqlDbType.Structured);
                cmdImpDatial.Parameters["@importDetails"].TypeName = "ImportDetailType";
                cmdImpDatial.Parameters["@importDetails"].Value = dt;
                if (cmdImpDatial.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            } catch (SqlException ex)
            {
                success = false;
                MessageBox.Show("There were errors!" + ex.Message);
            }
            finally
            {
                try
                {
                    cmdImpDatial.Dispose();
                    con.Close();
                } catch (NullReferenceException ee)
                {

                }             
            }
            return success; 
        }
        

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (cboSupplier.SelectedIndex == -1)
            {
                MessageBox.Show("Please select supplier!");
            }
            else if (dgvImpDetail.Rows.Count == 0)
            {
                MessageBox.Show("Please add product!");
            }
            else
            {
                bool sucess = false;
                int impID;
                try
                {
                    con.Open();
                    cmdImp = new SqlCommand("Insert into Import(impDate,supID,impTotal) Values(@impDate,@supplier,@impTotal)", con);
                    cmdImp.Parameters.AddWithValue("@impDate", dTPImpDate.Value);
                    cmdImp.Parameters.AddWithValue("@supplier", (int)cboSupplier.SelectedValue);
                    double totalImport = 0;
                    double.TryParse(txtAmount.Text, out totalImport);
                    cmdImp.Parameters.AddWithValue("@impTotal", totalImport);

                    if (cmdImp.ExecuteNonQuery() > 0)
                    {
                        sucess = true;
                    }
                }
                catch (SqlException ex)
                {
                    sucess = false;
                    MessageBox.Show("There was an error hapend! :" + ex.Message);
                }
                finally
                {
                    con.Close();
                    cmdImp.Dispose();
                }

                impID = Controller.GetLastAutoIncrement("Import");
                bool saveImportSuccess = SaveImprotDetail(impID);
                if (saveImportSuccess && sucess)
                {
                    MessageBox.Show("Inserted suceessfully!");
                    btnSave.Enabled = false;
                }
                else
                {
                    try
                    {
                        if (sucess)
                        {
                            cmdImp = new SqlCommand("DELETE Import WHERE impID = @impID", con);
                            cmdImp.Parameters.AddWithValue("@impID", impID);
                            cmdImpDatial = new SqlCommand("DELETE ImportDetail WHERE impID = @impID", con);
                            cmdImpDatial.Parameters.AddWithValue("@impID", impID);
                            cmdImp.ExecuteNonQuery();
                            cmdImpDatial.ExecuteNonQuery();
                        }          
                        MessageBox.Show("Inserted unsuceessfully!");
                    }
                    catch (SqlException ex)
                    {

                    }             
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dgvImpDetail.Rows.Count > 0)
            {
                int i = dgvImpDetail.CurrentCell.RowIndex;
                if (i >= 0)
                dgvImpDetail.Rows.RemoveAt(i);
            }
            
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            dTPImpDate.Text = null;
            cboSupplier.Text = null;
            ClearTextBox();
            dgvImpDetail.Rows.Clear();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex == -1)
            {
                MessageBox.Show("There is no prouct selected!");
            } else if (txtImpQty.Text == "")
            {
                MessageBox.Show("Please input quantity!");
                txtImpQty.Focus();
            } else if (txtImpPrice.Text.Trim() == "")
            {
                MessageBox.Show("Please input import price!");
                txtImpPrice.Focus();
            } else if (txtUnitPrice.Text.Trim() == "")
            {
                MessageBox.Show("Pleas input unit price!");
                txtUnitPrice.Focus();
            } else {
                string productName = cboProduct.Text.ToString();
                double price = 0;
                Double.TryParse(txtImpPrice.Text, out price);
                int qty = 0;
                int.TryParse(txtImpQty.Text, out qty);
                Double amount = price * qty;
                int proId = 0;
                int.TryParse(cboProduct.SelectedValue.ToString(), out proId);

                string[] row = { cboProduct.Text.ToString(), txtImpPrice.Text.ToString(), txtImpPrice.Text.ToString(), txtUnitPrice.Text.ToString(), amount.ToString(), proId.ToString() };
                totalAmount += amount;
                dgvImpDetail.Rows.Add(row);
                txtAmount.Text = totalAmount.ToString();
                ClearTextBox();
            }  
        }

        private void CreateTable()
        {
            dt = new DataTable();
            dt.Columns.Add("impID", typeof(int));
            dt.Columns.Add("proID", typeof(int));
            dt.Columns.Add("impQty", typeof(double));
            dt.Columns.Add("impPrice", typeof(double));
            dt.Columns.Add("unitPrice", typeof(double));
            dt.Columns.Add("amount", typeof(double));
            dt.Columns.Add("soldQty", typeof(int));
        }
       
        private void ClearTextBox()
        {
            txtImpQty.Text = null;
            txtImpPrice.Text = null;
            txtUnitPrice.Text = null;
            txtAmount.Text = null;
        }
    }
}
