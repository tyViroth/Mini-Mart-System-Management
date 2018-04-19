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
using Mart.Intefaces;
using Mart.Forms;
using Mart.ControlClasses;
using System.Data.SqlClient;
using Mart.InstanceClasses;

namespace Mart
{
    public partial class USold : UserControl, IMessageType
    {
        List<SellProduct> productList = null;
        public SqlCommand cmd = null;
        public SqlConnection con = Connection.getConnection();
        private static USold _instance;
        private DataTable dataTableSoldDetail;

        private bool textBoxChanged = false;
        private bool comboBoxChanged = false;
        private bool dgvSelected = false;

        private int proID = 0;
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
            EventRegister();            
            ControllDataGrid();
            Controller.FillComboBoxValue(cboImportID, "impID", "impID", "SelectAllImportID");
            if (cboImportID.Items.Count > 0)
            {
                cboImportID.SelectedIndex = cboImportID.Items.Count - 1;
                btnAdd.Enabled = true;
            }
            else
            {
                EnableControls(false);
                btnAdd.Enabled = false;
            }                
        }

        private void EventRegister()
        {
            btnAdd.Click += DoClick;
            btnCancel.Click += DoClick;
            btnSell.Click += DoClick;
            btnRemove.Click += DoClick;
            btnUpdate.Click += DoClick;
            btnReset.Click += DoClick;

            /* Allow Number only */
            txtQty.KeyPress += DoPress;
            txtPrice.KeyPress += DoPress;
            txtReceived.KeyPress += DoPress;
            txtChange.KeyPress += DoPress;

            txtQty.TextChanged += DoTextChange;
            txtDiscount.TextChanged += DoTextChange;
            txtTax.TextChanged += DoTextChange;              
            
            cboImportID.SelectedIndexChanged += cboImportID_SelectedIndexChanged;
            cboProductName.SelectedIndexChanged += cboProductName_SelectedIndexChanged;

            txtReceived.GotFocus += txtReceived_GotFocus;
            txtReceived.LostFocus += txtReceived_LostFocus;            

            dgvSale.MouseClick += dgvSale_MouseClick;
        }

        void cboImportID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int importID = 0;
            int.TryParse(cboImportID.SelectedValue.ToString(),out importID);
            if(importID == 0 || cboImportID.SelectedIndex == -1) return;

            SqlDataReader reader = null;
            productList = new List<SellProduct>();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("GetProductList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@impID",importID);          
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productList.Add(new SellProduct(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToDouble(reader[2])));
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load to Combobox");
            }
            finally
            {
                try
                {
                    reader.Close();
                    cmd.Dispose();                   
                    con.Close();
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message,"Dispose Load Product TO ComboBox");
                }
            }

            cboProductName.DataSource = null;
            cboProductName.Items.Clear();
            cboProductName.DataSource = productList;
            cboProductName.ValueMember = "ProductID";
            cboProductName.DisplayMember = "ProductName";
            if (cboProductName.Items.Count > 0){          
                EnableControls(true);
                cboProductName.SelectedIndex = -1;
                txtPrice.Clear();
            }
            else
            {
                EnableControls(false);
                DoClick(btnReset,null);
            }
            btnAdd.Enabled = false;
        }

        private void EnableControls(bool ena)
        {
            cboProductName.Enabled = ena;
            txtQty.Enabled = ena;
            txtPrice.Enabled = ena;
            txtDiscount.Enabled = ena;
            txtTax.Enabled = ena;
        }

        void dgvSale_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvSale.Rows.Count == 0) return;
            int index = dgvSale.CurrentRow.Index;
            txtQty.Text = dgvSale.Rows[index].Cells[4].Value.ToString();
            cboImportID.SelectedValue = int.Parse(dgvSale.Rows[index].Cells[1].Value.ToString());            
            cboProductName.SelectedValue = int.Parse(dgvSale.Rows[index].Cells[2].Value.ToString());
            txtPrice.Text = dgvSale.Rows[index].Cells[5].Value.ToString();
            txtDiscount.Text = dgvSale.Rows[index].Cells[6].Value.ToString();
            txtTax.Text = dgvSale.Rows[index].Cells[7].Value.ToString();            
            dgvSelected = true;
            btnRemove.Enabled = true;
            btnAdd.Enabled = false;
            btnReset.Visible = true;
        }

        private void DoTextChange(object sender, EventArgs e)
        {
            if (dgvSale.Rows.Count == 0) return;           
            int qty = 0;
            int index = dgvSale.CurrentRow.Index;
            int.TryParse(txtQty.Text.Trim(),out qty);
            qty = ( qty == 0)? 1: qty;
            float tax = 0;
            float.TryParse(txtTax.Text.Trim(),out tax);

            float dis = 0;
            float.TryParse(txtDiscount.Text.Trim(),out dis);

            if (int.Parse(dgvSale.Rows[index].Cells[4].Value.ToString()) != qty || int.Parse(dgvSale.Rows[index].Cells[6].Value.ToString()) != dis || int.Parse(dgvSale.Rows[index].Cells[7].Value.ToString()) != tax)
            {
                textBoxChanged = true;
                btnUpdate.Enabled = true;
            }
            else
            {
                if (!comboBoxChanged)
                {
                    btnUpdate.Enabled = false;
                }
                    
                textBoxChanged = false;
            }
        }

        private void ControllDataGrid()
        {
            foreach (DataGridViewColumn col in dgvSale.Columns)
            {
                Controller.AlignHeaderTextCenter(col);
            }

            dgvSale.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSale.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSale.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSale.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSale.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSale.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvSale.Columns[5].DefaultCellStyle.Format = "#,###,##0.00 R"; /*Column Price*/
            dgvSale.Columns[8].DefaultCellStyle.Format = "#,###,##0.00 R"; /*Column Amount*/          
        }       

        void txtReceived_GotFocus(object sender, EventArgs e)
        {
            if (txtReceived.Text  == "0.00")
            {
                txtReceived.Clear();
            }
        }

        void txtReceived_LostFocus(object sender, EventArgs e)
        {
            if (dgvSale.Rows.Count == 0) return;
            
            float receive = 0;            
            float amount = 0;
            float.TryParse(txtReceived.Text.Trim(),out receive);
            float.TryParse(txtAmount.Text.Trim(),out amount);

            if (txtReceived.Text.Trim() == "")
            {
                MessageError("Please input money !", "Sell Product");
                return;
            }
            else if (receive < amount)
            {
                MessageError("The received money is less the Amount!", "Sell Product");
                return;
            }

            txtChange.Text = string.Format("{0:N2}",receive - amount);
            btnSell.Enabled = true;
        }

        void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductName.SelectedIndex == -1) return;
            int.TryParse(cboProductName.SelectedValue.ToString(), out proID);
            if (proID == 0) return;

            txtPrice.Text = productList[cboProductName.SelectedIndex].Price.ToString(); 

            #region /*To check if data is update when user click on DataGridView rows => Enable UPDATE button*/

            if (dgvSelected && dgvSale.Rows.Count > 0)
            {
                int index = dgvSale.CurrentRow.Index;
                if (int.Parse(cboProductName.SelectedValue.ToString()) != int.Parse(dgvSale.Rows[index].Cells[2].Value.ToString()) || int.Parse(cboImportID.SelectedValue.ToString()) != int.Parse(dgvSale.Rows[index].Cells[1].Value.ToString()))
                {
                    comboBoxChanged = true;
                    btnUpdate.Enabled = true;
                }
                else if (true)
                {
                    if (!textBoxChanged)
                    {
                        btnUpdate.Enabled = false;
                    }
                    comboBoxChanged = false;
                }
                btnAdd.Enabled = false;
            }
            #endregion
            else
            {
                btnAdd.Enabled = true;            
            }
        }

        private bool CheckProductForSale()
        {
            bool has = false;
            int productNumber = 0, importID = 0;
            int qty = 0;
            int.TryParse(txtQty.Text.Trim(),out qty);
            qty = (qty == 0) ? 1 : qty;
            int.TryParse(cboImportID.SelectedValue.ToString(),out importID);           

            try
            {
                if(con.State == ConnectionState.Closed)
                con.Open();

                cmd = new SqlCommand("CheckIsProductExists", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID",proID);
                cmd.Parameters.AddWithValue("@impID",importID);
                productNumber = Convert.ToInt32(cmd.ExecuteScalar());

                /*To substract Product Number in DataGridView (Add The Product Number before check in Database Table)*/
                foreach (DataGridViewRow row in dgvSale.Rows)
                {
                    if (dgvSelected && row.Index != dgvSale.CurrentRow.Index)
                    {
                        /*If user click any row to update data , we wont add include the Quantity of selected row */
                        continue;
                    }

                    if (int.Parse(row.Cells[1].Value.ToString()) == importID && int.Parse(row.Cells[2].Value.ToString()) == proID)
                    {
                        productNumber -= int.Parse(row.Cells[4].Value.ToString());
                    }
                }

                if (productNumber == 0)
                {
                    MessageBox.Show("This product is not exist","Sale");                   
                    txtQty.Clear();               
                    has = false;
                }
                else if (productNumber < qty && productNumber >= 0)
                {
                    MessageBox.Show(string.Format("We have {0} only {1} items in our Mart",cboProductName.Text,productNumber), "Sale");                                    
                    has = false;
                }
                else
                {
                    has = true;                                              
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Get Import Detail ID");
            }
            finally
            {
                try
                {
                    cmd.Dispose();
                    con.Close();
                }
                catch (NullReferenceException)
                {
            
                }
            }
            return has;
        }

        private void DoPress(object sender, KeyPressEventArgs e)
        {
            Input.InputNmber((TextBox)sender,e);
        }      
       
        private void DoClick(object sender, EventArgs e)
        {
            if (sender == btnAdd)
            {
                if (!CheckProductForSale()) return;

                double _amount = 0;
                int _currentNumber = dgvSale.Rows.Count + 1;
                string _name = cboProductName.Text.Trim();

                int _qty = 0, importID = 0;
                int.TryParse(cboImportID.SelectedValue.ToString(),out importID);
                int.TryParse(txtQty.Text.Trim(),out _qty);
                _qty = (_qty == 0) ? 1 : _qty;

                float _price = 0f, _discount = 0f, _tax = 0f;
                float.TryParse(txtPrice.Text.Trim(), out _price);                              
                float.TryParse(txtDiscount.Text.ToString(), out _discount);
                float.TryParse(txtTax.Text.ToString(), out _tax);               
                
                float subTotal = _qty * _price;
                float totalDiscount = subTotal * _discount / 100;
                float totalTax = subTotal * _tax/100;

                _amount =  subTotal - totalDiscount + totalTax;

                dgvSale.Rows.Add(_currentNumber, importID, proID, _name, _qty, _price, _discount, _tax, _amount);

                RefreshTotal();
                btnRemove.Enabled = true;
            }
            else if (sender == btnUpdate)
            {
                if (dgvSale.Rows.Count == 0 || !CheckProductForSale()) return;                
                int index = dgvSale.CurrentRow.Index;

                double _amount = 0;
                int _currentNumber = int.Parse(dgvSale.Rows[index].Cells[0].Value.ToString());
                string _proName = cboProductName.Text.Trim();

                int _qty = 0, importID = 0;
                int.TryParse(cboImportID.SelectedValue.ToString(), out importID); 
                int.TryParse(txtQty.Text.Trim(), out _qty);
                _qty = (_qty == 0) ? 1 : _qty;

                float _price = 0f, _discount = 0f, _tax = 0f;
                float.TryParse(txtPrice.Text.Trim(), out _price);
                float.TryParse(txtDiscount.Text.ToString(), out _discount);
                float.TryParse(txtTax.Text.ToString(), out _tax); 

                float subTotal = _qty * _price;
                float totalDiscount = subTotal * _discount / 100;
                float totalTax = subTotal * _tax / 100;

                _amount = subTotal - totalDiscount + totalTax;                
                dgvSale.Rows[index].SetValues(_currentNumber, importID, proID, _proName, _qty, _price, _discount, _tax, _amount);
                
                ResetControls();
                RefreshTotal();

                textBoxChanged = false;
                comboBoxChanged = false;
                dgvSelected = false;
                btnReset.Visible = false;

            }else if(sender == btnReset){
                ResetControls();
                RefreshTotal();
                btnReset.Visible = false;
                
                /*Set No data was changed*/
                textBoxChanged = false;
                comboBoxChanged = false;
                dgvSelected = false;
            }
            else if (sender == btnRemove)
            {
                if (dgvSale.Rows.Count > 0)
                {
                    int selectedIndex = dgvSale.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvSale.Rows.RemoveAt(selectedIndex);
                        if (dgvSale.Rows.Count == 0)
                        {
                            btnRemove.Enabled = false;
                            btnReset.Visible = false;
                        }
                        ResetControls();
                        RefreshTotal();
                        btnReset.Visible = false;
                    }
                }
            }
            else if (sender == btnCancel)
            {
                DialogResult result = MessageVerify("Do you want to cancel sale?","Sale");
                if (result == DialogResult.Yes)
                {
                    if (dgvSale.Rows.Count != 0)
                    {
                        dgvSale.Rows.Clear();
                        RefreshTotal();
                    }

                    if (dataTableSoldDetail != null)
                    {
                        dataTableSoldDetail.Rows.Clear();
                        dataTableSoldDetail.Dispose();
                    }
                    btnReset.Visible = false;
                    btnAdd.Enabled = false;
                    ResetControls();
                }                
            }
            else if (sender == btnSell)
            {                
                 DialogResult verifyResult = MessageVerify("Click Yes to Sell?","Sale");
                 if (verifyResult == DialogResult.Yes)
                 {
                     bool insertSold = InsertIntoSoldTable();
                     if (!insertSold)
                     {
                         /*If Inserting into Sold Table is error we return*/
                         MessageSuccess("Sale was saved unsuccessful", "Sell Product");
                         return;
                     }

                     bool insertDetails = false;

                     CreateDataTable();
                     /*Load Data into DataTable*/
                     int lastSoldID = Controller.GetLastAutoIncrement("Sold");

                     try
                     {
                         foreach (DataGridViewRow row in dgvSale.Rows)
                         {
                             DataRow newRow = dataTableSoldDetail.NewRow();
                             newRow["soldID"] = lastSoldID;
                             /*Product ID is hidden in DataGridView*/
                             newRow["impID"] = int.Parse(row.Cells[1].Value.ToString().Trim());
                             newRow["proID"] = int.Parse(row.Cells[2].Value.ToString().Trim());
                             newRow["qty"] = int.Parse(row.Cells[4].Value.ToString().Trim());
                             newRow["price"] = float.Parse(row.Cells[5].Value.ToString().Trim());
                             newRow["discount"] = float.Parse(row.Cells[6].Value.ToString().Trim());
                             newRow["tax"] = float.Parse(row.Cells[7].Value.ToString().Trim());
                             newRow["amount"] = double.Parse(row.Cells[8].Value.ToString().Trim());
                             dataTableSoldDetail.Rows.Add(newRow);
                         }

                         if (con.State == ConnectionState.Closed)
                             con.Open();
                         cmd = new SqlCommand("InsertSoldDetail", con);
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@SoldDetailType", dataTableSoldDetail);
                         if (cmd.ExecuteNonQuery() > 0)
                         {
                             insertDetails = true;
                         }
                     }
                     catch (Exception ex)
                     {
                         insertDetails = false;
                         MessageError(ex.Message, "Sold Details");
                     }
                     finally
                     {
                         try
                         {
                             con.Close();
                             cmd.Dispose();
                         }
                         catch (NullReferenceException) { }
                     }

                     /*Check if Insertion is error.*/
                     if (insertSold && insertDetails)
                     {
                         MessageSuccess("Sale was saved successful", "Sell Product");
                         btnSell.Enabled = false;
                     }
                     else if (!insertDetails) /*Insert into Sold Detail is error.*/
                     {
                         #region /*We code to delete any records which has been inserted successfully into SOLD AND SOLDDETAIL table*/

                         SqlCommand cmdDeleteSold = null, cmdDeleteDetails = null;
                         try
                         {
                             if (con.State == ConnectionState.Closed)
                                 con.Open();
                             cmdDeleteSold = new SqlCommand("delete from Sold where soldID = @soldID", con);
                             cmdDeleteSold.CommandType = CommandType.Text;
                             cmdDeleteSold.Parameters.AddWithValue("@soldID", lastSoldID);
                             cmdDeleteSold.ExecuteNonQuery();

                             cmdDeleteDetails = new SqlCommand("delete from SoldDetail where soldID = @soldID", con);
                             cmdDeleteDetails.CommandType = CommandType.Text;
                             cmdDeleteDetails.Parameters.AddWithValue("@soldID", lastSoldID);
                             cmdDeleteDetails.ExecuteNonQuery();
                         }
                         catch (Exception ex)
                         {
                             MessageError(ex.Message, "Sell Product");
                         }
                         finally
                         {
                             try
                             {
                                 con.Close();
                                 cmdDeleteSold.Dispose();
                                 cmdDeleteDetails.Dispose();
                             }
                             catch (NullReferenceException) { }
                         }
                         DialogResult result = MessageVerify("Sale was saved unsuccessful. Please click SELL again", "Sell Product");
                         if (result == DialogResult.Yes)
                         {
                             DoClick(btnSell, null);
                         }
                         #endregion
                     }
                 }                
            }               
        }

        private void ResetControls()
        {
            if (cboProductName.Items.Count > 0)
                cboProductName.SelectedIndex = -1;
            txtQty.Clear();
            txtPrice.Clear();
            txtAmount.Text = "0.00";
            txtReceived.Text = "0.00";
            txtChange.Text = "0.00";
            txtDiscount.Clear();
            txtTax.Clear();

            btnSell.Enabled = false;          
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private bool InsertIntoSoldTable()
        {
            bool success = false;
            double soldAmount = 0;
            double.TryParse(txtAmount.Text.Trim(),out soldAmount);
            try
            {
                if (con.State == ConnectionState.Closed)        
                con.Open();
                cmd = new SqlCommand("INSERT INTO Sold(soldDate,empID,amount) values(@soldDate,@empID,@amount)",con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@soldDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@empID", Program.empLogin.ID);
                cmd.Parameters.AddWithValue("@amount", soldAmount);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"Insert Sold");
                success = false;
            }
            finally
            {
                try
                {
                    cmd.Dispose();
                    con.Close();                   
                }
                catch (NullReferenceException)
                {                                       
                }
            }
            return success;
        }
       
        private void CreateDataTable()
        {
            dataTableSoldDetail = new DataTable();
            dataTableSoldDetail.Columns.Add("soldID",typeof(int));
            dataTableSoldDetail.Columns.Add("impID", typeof(int));            
            dataTableSoldDetail.Columns.Add("proID", typeof(int));
            dataTableSoldDetail.Columns.Add("qty", typeof(int));
            dataTableSoldDetail.Columns.Add("price", typeof(float));
            dataTableSoldDetail.Columns.Add("discount", typeof(float));
            dataTableSoldDetail.Columns.Add("tax", typeof(float));
            dataTableSoldDetail.Columns.Add("amount", typeof(double));
        }

        private void RefreshTotal()
        {
            int sum = 0;
            int lastCol = dgvSale.Columns.Count - 1;
            foreach (DataGridViewRow row in dgvSale.Rows)
            {
                sum += Convert.ToInt32(row.Cells[lastCol].Value);
                row.Cells[0].Value = row.Index + 1;
            }

            string total = string.Format("{0:N2}", sum);
            txtAmount.Text = total;
            lblTotalAmount.Text = total;
            lblTotalRow.Text = dgvSale.Rows.Count.ToString();
        }

        public void MessageSuccess(string des, string title)
        {
            MessageBox.Show(des, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MessageError(string des, string title)
        {
            MessageBox.Show(des, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void MessageWarning(string des, string title)
        {
            MessageBox.Show(des, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public DialogResult MessageVerify(string des, string title)
        {
            return MessageBox.Show(des, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
