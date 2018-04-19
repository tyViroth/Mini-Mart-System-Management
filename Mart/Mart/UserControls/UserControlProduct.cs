using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Mart.Forms;
using Mart.Intefaces;

namespace Mart
{
    public partial class UProduct : UserControl, IMessageType
    {
        #region
        /*Connection Declaration*/
        private SqlConnection cnn = Connection.getConnection();
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataTable table;
        /*SQL Command*/

        /*Global Variable */
        private readonly String[] SearchBy = { "Product ID", "Product Name", "Price", "Category" };
        private static string searchText = "";
        private static int searchType = 1;
        private string filter = "";
        /*Form Declaration*/
        FormCategory categoryForm = null;
        #endregion
        private static UProduct _instance;
        public static UProduct Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UProduct();
                return _instance;
            }
        }
        public UProduct()
        {
            InitializeComponent();
            this.Load += UProduct_Load;
        }
        private void EventRegister()
        {
            btnAdd.Click += btnDoClick;
            btnDetails.Click += btnDoClick;
            btnSearch.Click += btnDoClick;
            btnUpdate.Click += btnDoClick;
            btnCategory.Click += btnDoClick;
            btnRefresh.Click += btnDoClick;
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.KeyDown += txtSearch_KeyDown;
            cboTypeSearch.SelectedIndexChanged += cboTypeSearch_SelectedIndexChanged;
            cboFilter.SelectedIndexChanged += DoFilter;
        }
        private void DoFilter(object sender, EventArgs e)
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand("FilterCategory", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cate", cboFilter.SelectedValue);
                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);
                dgvProduct.DataSource = null;
                dgvProduct.Rows.Clear();
                dgvProduct.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageError(ex.Message, "Error");
            }
            finally { cnn.Close(); cmd.Dispose(); }
            SetFooter();
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtSearch.Text))
            {
                btnSearch.PerformClick();
            }
        }
        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }
        private void UProduct_Load(object sender, EventArgs e)
        {
            cboTypeSearch.DataSource = SearchBy;
            Controller.FillComboBoxValue(cboFilter, "cateID", "cateName", "SELECT * FROM Category");
            LoadDatabase("FillProductData");
            SetFooter();
            EventRegister();
        }
        private void cboTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = cboTypeSearch.Text;
                searchType = (int)cboTypeSearch.SelectedIndex + 1;
            }
            catch (Exception)
            {}
        }
        private void btnDoClick(object sender, EventArgs e)
        {
            /*Check Selected Index*/
            int index = 0;
            if (dgvProduct.Rows.Count > 0)
            {
                index = (int)dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[0].Value;
            }
            if (sender == btnDetails)
            {
                if (index != 0)
                {
                    FormProductDetails proDetails = new FormProductDetails(0, index);
                    proDetails.ShowDialog();
                }
            }
            else if (sender == btnUpdate)
            {
                if (index != 0)
                {
                    FormProductDetails proDetails = new FormProductDetails(2, index);
                    proDetails.ShowDialog();
                    if (proDetails.SuccesUpdate == true)
                    {
                        MessageSuccess("Update Success", "Update");
                        if (!UpdateDataRow())
                        {
                            LoadDatabase("FillProducData");
                        }
                        
                    }
                    SetFooter();
                }
            }
            else if (sender == btnAdd)
            {
                if (index != 0)
                {
                    FormProductDetails frmAdd = new FormProductDetails(1, index);          
                    frmAdd.Created += frmAdd_Created;
                    frmAdd.ShowDialog();                    
                }
            }
            else if (sender == btnCategory)
            {
                if (categoryForm == null)
                {
                    categoryForm = new FormCategory();
                    categoryForm.Show();
                    categoryForm.FormClosed += cate_FormClosed;
                }
                else
                {
                    categoryForm.BringToFront();
                }

            }
            else if (sender == btnSearch)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    searchType = cboTypeSearch.SelectedIndex + 1;
                    searchText = txtSearch.Text;
                    if (searchType == 3)
                    {
                        SearchDatabase("SearchByPrice");
                    }
                    else
                    {
                        SearchDatabase("SearchProductData");
                    }
                    searchType = 1;
                    searchText = "";
                    txtSearch.Text = "";
                    SetFooter();
                }
            }
            else if (sender == btnRefresh)
            {
                LoadDatabase("FillProductData");
                SetFooter();
            }
        }

        void frmAdd_Created()
        {
            btnDoClick(btnRefresh, null);
        }

        private bool UpdateDataRow()
        {
            bool success=false;
            int index=(int)dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[0].Value;
            try
            {
                cnn.Open();
                cmd=new SqlCommand("SELECT  P.proID as [Product ID],P.proName as [Product Name],P.price as [Price],C.cateName as [Category],P.proType AS [Type] From Product as P inner join Category as C on P.cateID=C.cateID WHERE P.proID = @id",cnn);
                cmd.Parameters.AddWithValue("@id",index);
                SqlDataReader rd = cmd.ExecuteReader();                
                    while (rd.Read())
                    {
                        dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[1].Value = rd[1];
                        dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[2].Value = rd[2];
                        dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[3].Value = rd[3];
                        dgvProduct.Rows[dgvProduct.CurrentCell.RowIndex].Cells[4].Value = rd[4];
                        success = true;
                    }      
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"Error");
                success = false;
            }
            finally { cnn.Close(); cmd.Dispose(); }
            return success;
        }
        private void cate_FormClosed(object sender, FormClosedEventArgs e)
        {
            categoryForm = null;
            btnRefresh.PerformClick();
        }
        private void LoadDatabase(String command)
        {
            try
            {
                dgvProduct.DataSource = null;
                dgvProduct.Rows.Clear();
                cmd = new SqlCommand(command, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvProduct.DataSource = table;
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageError("Error loading database\n" + ex.Message,"Error Loading");
            }
            finally
            {
                cmd.Dispose();
                cnn.Close();
            }
        }
        private void SearchDatabase(String command)
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand(command, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (searchType == 3)
                { float price=0;
                        if (float.TryParse(searchText, out price))
                        {
                            cmd.Parameters.AddWithValue("@price", price);
                        }         
                }
                else
                {
                    cmd.Parameters.AddWithValue("@keyword", searchText);
                    cmd.Parameters.AddWithValue("@type", searchType);
                }
                cmd.Dispose();
            }
            catch (Exception)
            {
            }
            finally
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    table = new DataTable();
                    adapter.Fill(table);
                    dgvProduct.DataSource = null;
                    dgvProduct.Rows.Clear();
                    dgvProduct.DataSource = table;
                }
                catch (Exception)
                {
                    MessageError("Invalid input\nError Occur While Searching !", "Error");
                }
                cnn.Close();
            }
        }
        private void SetFooter()
        {        
            txtTotalItem.Text = dgvProduct.Rows.Count.ToString();
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
