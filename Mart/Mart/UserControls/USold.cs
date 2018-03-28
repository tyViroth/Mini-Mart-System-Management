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
    public partial class USold : UserControl, IFunctionModel<sold>, IMessageType
    {
        Dictionary<string,Stock> dict = null;
        public List<sold> list = new List<sold>();
        public SqlCommand cmd;
        public SqlConnection con = Connection.getConnection();

        private static USold _instance;
        string Temp2 = null;
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
            EventState();

            tblSold.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //CmStock_load();
            CmProName_load();
            cbName.SelectedValueChanged += CbName_SelectedValueChanged;
            tblSold.AllowUserToResizeRows = false;
            cbStock.Enabled = false;
            EventRegister();

            EventKey();

        }

        private void EventState()
        {
            txtReceived.Enabled = false;
            txtChange.Enabled = false;
            btnSell.Enabled = false;
            btnRemove.Enabled = false;
            btnCalculate.Enabled = false;
        }

        private void EventKey()
        {
            txtQty.KeyPress += DoPress;
            txtPrice.KeyPress += DoPress;
            txtReceived.KeyPress += DoPress;
            txtChange.KeyPress += DoPress;
        }

        private void DoPress(object sender, KeyPressEventArgs e)
        {
            if(sender == txtQty || sender == txtPrice ||  sender ==txtReceived || sender == txtChange)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
      
        }

        private void CbName_SelectedValueChanged(object sender, EventArgs e)
        {      
            //int productID = (int)cbName.SelectedValue;
            //cbStock.Text = Temp2.ToString();           
            Stock st = dict[(string)cbName.SelectedValue];
            //MessageBox.Show(st.ProName+" ; "+st.ProID + " ; "+ st.ImpID );
            cbStock.Text = st.ImpID.ToString();
        }

        private void EventRegister()
        {
            btnAdd.Click += DoClick;
            btnSell.Click += DoClick;
            btnRemove.Click += DoClick;
            btnLoad.Click += DoClick;
            btnShowList.Click += DoClick;
        }

        private void CmProName_load()
        {
            try
            {
                /*
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select I.proID, P.proName, I.impID from Product as P inner join ImportDetail as I on P.proId = I.proID where I.impQty > I.soldQty", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
               

                cbName.DataSource = dt;
                cbName.DisplayMember = "proName";
                cbName.ValueMember = "proID";

                Temp2 = dt.Rows[0]["impID"].ToString();
                MessageBox.Show(Temp2);
                */
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select I.proID, P.proName, I.impID from Product as P inner join ImportDetail as I on P.proId = I.proID where I.impQty > I.soldQty", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dict = new Dictionary<string,Stock>();
                
                foreach (DataRow row in dt.Rows)
                {
                    dict.Add((string)row[1], new Stock((int)row[0], (string)row[1], (int)row[2]));              
                }

                BindingSource bs = new BindingSource(dict,null);

                cbName.DataSource = dict.Keys.ToList();

                //Temp2 = dt.Rows[0]["impID"].ToString();
                //MessageBox.Show(Temp2);
            }
            catch (SqlException ex)
            {
                MessageError(ex.Message, "Error Insert");
            }
            finally
            {
                con.Close();
            }
        }

        private void CmStock_load()
        {
            try
            {
                con.Open();

                SqlCommand command = new SqlCommand("Select impID from Import", con);
                int result = command.ExecuteNonQuery();
                SqlDataReader reader;
                reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("impID", typeof(string));
                dt.Load(reader);
                cbStock.ValueMember = "impID";
                cbStock.DataSource = dt;
            }
            catch (SqlException ex)
            {
                MessageError(ex.Message, "Error Insert");
            }
            finally
            {
                con.Close();
            }
        }

        private void DoClick(object sender, EventArgs e)
        {
            if (sender == btnAdd)
            {
                txtReceived.Enabled = true;
                txtChange.Enabled = true;
                btnSell.Enabled = true;
                btnRemove.Enabled = true;
                btnCalculate.Enabled = true;

                double _amount = 0;
                int _currentNumber = 1;
                string _name = cbName.Text;
                int _quantity = int.Parse(txtQty.Text);
                float _price = float.Parse(txtPrice.Text);
                string _date = date.Value.ToString("hh:mm:ss");
                float _discount = float.Parse(cbDiscount.Text.ToString());
                float _tax = float.Parse(cbTax.Text.ToString());
                //int productID = (int)cbName.SelectedValue;

                _amount = _quantity * _price;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(tblSold);
                row.Cells[0].Value = _currentNumber;
                row.Cells[1].Value = _name.ToString();
                row.Cells[2].Value = _quantity.ToString();
                row.Cells[3].Value = _price.ToString();
                row.Cells[4].Value = _amount.ToString();
                row.Cells[5].Value = _discount.ToString();
                row.Cells[6].Value = _date.ToString();
                row.Cells[7].Value = _tax.ToString();
                //row.Cells[8].Value = productID.ToString();
                _currentNumber++;
                tblSold.Rows.Add(row);
                
                //sold s = new sold(_name, _quantity, _price, _date, _discount, _tax);
                //list.Add(s);
                int sum = 0;
                for (int i = 0; i < tblSold.Rows.Count; ++i)
                {
                    sum += Convert.ToInt32(tblSold.Rows[i].Cells[4].Value);
                }
                txtAmount.Text = sum.ToString();                
            }
            else if (sender == btnRemove)
            {
                try
                {
                    int selectedIndex = tblSold.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        tblSold.Rows.RemoveAt(selectedIndex);
                        tblSold.Refresh(); // if needed
                        int sum = 0;
                        for (int i = 0; i < tblSold.Rows.Count; ++i)
                        {
                            sum += Convert.ToInt32(tblSold.Rows[i].Cells[4].Value);
                        }
                        txtAmount.Text = sum.ToString();
                    }

                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Unable to remove selected row at this time" + ex.Message);
                }
            }
            else if (sender == btnSell)
            {


                string tb_proId = null ;
                string tb_qty = null;
                string tb_price = null;
                string tb_amount = null;
                string tb_discount = null;
                string tb_date = null;
                string tb_tax= null;
                
                Boolean success = false;

                foreach (DataGridViewRow Datarow in tblSold.Rows)
                {
                    if (Datarow.Cells[2].Value != null && Datarow.Cells[3].Value != null && Datarow.Cells[4].Value != null && Datarow.Cells[5].Value != null && Datarow.Cells[6].Value != null && Datarow.Cells[7].Value != null && Datarow.Cells[8].Value != null)
                    {
                        //loop table
                        tb_proId = Datarow.Cells[8].Value.ToString();
                        tb_qty = Datarow.Cells[2].Value.ToString();
                        tb_price = Datarow.Cells[3].Value.ToString();
                        tb_amount = Datarow.Cells[4].Value.ToString();
                        tb_discount = Datarow.Cells[5].Value.ToString();
                        tb_date = Datarow.Cells[6].Value.ToString();
                        tb_tax = Datarow.Cells[7].Value.ToString();
                        //MessageBox.Show(tb_proId + tb_qty + tb_price + tb_amount + tb_discount + tb_date + tb_tax);
                        try
                        {
                            int auto = GetLastAutoIncrement("Sold");

                            con.Open();
                            cmd = new SqlCommand("Insert into SoldDetail(soldID, proID, qty, price, discount, tax, amount) VALUES(@soldID,@proID,@qty,@price,@discount,@tax,@amount)", con);
                            cmd.Parameters.AddWithValue("@soldID", auto);
                            cmd.Parameters.AddWithValue("@proID", tb_proId);
                            cmd.Parameters.AddWithValue("@qty", tb_qty);
                            cmd.Parameters.AddWithValue("@price", tb_price);
                            cmd.Parameters.AddWithValue("@discount", tb_discount);
                            cmd.Parameters.AddWithValue("@tax", tb_tax);
                            cmd.Parameters.AddWithValue("@amount", tb_amount);


                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                success = true;
                            }
                            else
                            {
                                success = false;
                            }
                        }

                        catch (SqlException ex)
                        {
                            MessageError(ex.Message, "Error Insert");
                        }
                        finally
                        {
                            con.Close();
                            cmd.Dispose();
                        }
                    }
                }
                if(success)
                {
                    MessageSuccess("Insert succeeded","Sold");
                }
            }
            else if (sender == btnLoad)
            {
                frmLoadSoldDetail load = new frmLoadSoldDetail();
                load.Show();
            }
            else if( sender == btnShowList) 
            {
                SoldInfo soldInfo = new SoldInfo();
                soldInfo.Visible = true;
            }
        }

        public int GetLastAutoIncrement(string tbName)
        {
            int auto = 0;
            try
            {
                con.Open();
                cmd = new SqlCommand("SELECT IDENT_CURRENT(@tb);", con);
                cmd.Parameters.AddWithValue("@tb",tbName.Trim());
                auto = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return auto;
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

        public void LoadData()
        {
            //throw new NotImplementedException();
        }

        public bool Insert(sold obj)
        {
            throw new NotImplementedException();
        }

        public bool Update(sold obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private void date_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
