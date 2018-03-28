using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Mart.Intefaces;
using Mart.InstanceClasses;

namespace Mart.Forms
{
    public partial class frmCategory : Form ,IMessageType
    {
        
        #region
        /*SQL*/
        private SqlConnection cnn = Connection.getConnection();
        private SqlCommand cmd;
        /*Global Variable*/
        bool update = false;
        string beforeupdate;
        #endregion

        public frmCategory()
        {
            InitializeComponent();
            RegisterEvent();
            Load();
        }

        private void Load()
        {         
            try
            {
                dgvCategory.DataSource = null;
                dgvCategory.Rows.Clear();
                cnn.Open();
                cmd = new SqlCommand("SELECT cateID AS [Category ID],cateName AS [Category Name] FROM Category",cnn);
                if ((int)cmd.ExecuteScalar() < 1)
                {
                    try
                    {
                        SqlCommand cmd1 = new SqlCommand(@"INSERT Category(cateID,cateName) VALUES(1,'Defualt')", cnn);
                    }
                    catch (Exception)
                    { 
                    
                    }
                
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCategory.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"Error");
            }
            finally {
                cnn.Close();
                cmd.Dispose();
            }
            FillFooter();
        }
        private void FillFooter()
        {
            try
            {
                txtRowCount.Text = dgvCategory.Rows.Count.ToString();
                if (dgvCategory.Rows.Count > 1)
                {
                    txtCurrentRow.Text = (dgvCategory.CurrentCell.RowIndex + 1).ToString();
                }
            }
            catch (Exception)
            {}  
        }

        public void RegisterEvent()
        {
            btnAdd.Click += btnDoClick;
            btnDelete.Click += btnDoClick;
            btnEdit.Click += btnDoClick;
            btnDone.Click += btnDoClick;
            btnCancel.Click += btnDoClick;  
            btnRefresh.Click +=btnDoClick;
            btnSearch.Click += btnDoClick;
            txtSearch.KeyDown += txtKeydown;
            dgvCategory.SelectionChanged += dgvCategory_SelectionChanged;
            txtCategoryName.KeyDown += txtKeydown;
            txtCategoryID.KeyDown += txtKeydown;
        }

        void dgvCategory_SelectionChanged(object sender, EventArgs e)
        {
            FillFooter();
        }

        private void txtKeydown(object sender, KeyEventArgs e)
        {
            if (sender == txtSearch)
            {
                          if (e.KeyCode == Keys.Enter)
                                    {
                                        btnSearch.PerformClick();
                                    }
            }
            else if (sender == txtCategoryName || sender==txtCategoryID)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnDone.PerformClick();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    btnCancel.PerformClick();
                }
            }
          
        }

       

        private int Existed(string catename)
        {
            int existed = 1;
           try
           {
               cnn.Open();
               cmd = new SqlCommand("SELECT COUNT(1) FROM Category WHERE cateName=@catename",cnn);
               cmd.Parameters.AddWithValue("@catename",catename);
               cmd.ExecuteNonQuery();
               existed = (int)cmd.ExecuteScalar();     
           }
           catch (Exception ex)
           {
               MessageError(ex.Message,"Error");
           }
           finally
           {
               cnn.Close();
               cmd.Dispose();
            }
           return existed;
        } 

        private bool Add(string cateName)
        {
            bool success = false;
            try
            {
                cnn.Open();       
                cmd = new SqlCommand("INSERT INTO Category(cateName) VALUES(@cateName)", cnn);
                cmd.Parameters.AddWithValue("@cateName", cateName);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageSuccess("New Category Insert", "Inserted");
                }
            }
            catch (Exception ex)
            {
                success = false;
                MessageError("Error Create New Category\n" + ex.Message, "Error Insert");
            }
            finally
            {
                cnn.Close();
                cmd.Dispose();
            }
            return success;
        }



        private void ShowUpdateDialog(int id)
        {
            DisableBackground(true);
            try
            {
                cnn.Open();
                cmd = new SqlCommand("SELECT cateID,cateName FROM Category WHERE cateID=@id",cnn);
                cmd.Parameters.AddWithValue("@id",id);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtCategoryID.Text = dr["cateID"].ToString();
                    txtCategoryName.Text = (string)dr["cateName"];
                }
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"");
            }
            finally { cnn.Close(); cmd.Dispose(); }
        }
        private bool Update(int id,string cateName)
        {
            bool success=false;
            try
            {
                cnn.Open();
                cmd = new SqlCommand("UPDATE Category SET cateName=@cateName WHERE cateID=@id",cnn);
                cmd.Parameters.AddWithValue("@cateName",cateName);
                cmd.Parameters.AddWithValue("@id",id);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageSuccess("Success Update !", "Update");
                }
            }
            catch (Exception ex)
            {
                MessageError("Error Update\n"+ex.Message,"Error Update");
                
            }
            finally {
                    cnn.Close(); 
                    cmd.Dispose();
                    }
            return success;
        }
        private int CheckDeleteAble(int index)
        {
            int count = 0;
            try
            {
                cnn.Open();
                cmd = new SqlCommand("SELECT COUNT(*) FROM Product WHERE cateID=@id",cnn);
                cmd.Parameters.AddWithValue("@id",index);
                cmd.ExecuteNonQuery();
                count = (int)cmd.ExecuteScalar();

            }
            catch (Exception)
            {

            }
            finally
            {
                cnn.Close();
                cmd.Dispose();
            }
            return count;
        }
        private bool Delete(int index)
        {
            bool success=false;
            try
            {
                cnn.Open();
                SqlCommand cmd=new SqlCommand("DELETE FROM Category WHERE cateID=@index",cnn);
                cmd.Parameters.AddWithValue("@index",index);
                if(cmd.ExecuteNonQuery()==1)
                {
                    success=true;
                }
                cmd.Dispose();
            }
            catch (Exception ex)
            {
              MessageError("Error Delete"+ex.Message,"Error Delete");
            }
            finally { cnn.Close(); }


            return success;
        }
        private void btnDoClick(object sender, EventArgs e)
        {
            
            int index=0;
            if (dgvCategory.Rows.Count > 0)
            {
                index = (int)dgvCategory.Rows[dgvCategory.CurrentCell.RowIndex].Cells[0].Value;
            }
            if (sender == btnAdd)
            {
                panelCategoryInput.Visible = true;
                DisableBackground(true);
                update = false;
                lblPanelTitle.Text = "New";
            }
            else if (sender == btnDelete)
            {
                if(index == 1 || CheckDeleteAble(index)>0)
                {
                    MessageError("This Category need to use in other table\nPlease check your product list","Delete Error");
                }
                else 
                {
                    if (MessageVerify("Are you sur to delete?\n Some data will not work correctly !", "Delete") == DialogResult.Yes)
                    {
                        Delete(index);
                        Load();
                    }             
                }              
            }
            else if (sender == btnEdit)
            {      
                panelCategoryInput.Visible = true;
                DisableBackground(true);
                lblPanelTitle.Text = "Update";
                update = true;
                ShowUpdateDialog(index);
                beforeupdate=txtCategoryName.Text;
            }
            else if (sender == btnDone)
            {
                panelCategoryInput.Visible = false;
                if (update) 
                {
                    if(string.IsNullOrEmpty(txtCategoryName.Text) || txtCategoryName.Text==beforeupdate)
                    {
                        MessageWarning("Do not Update \n No text input","Update");
                    }else
                    {
                        if (Existed(txtCategoryName.Text)<1)
                        {
                            Update(index, txtCategoryName.Text);
                        }
                        else 
                        {
                            MessageWarning("Can't Update !\n Existed Name","Update");
                        }
                    }
                }
                else {
                    if (string.IsNullOrEmpty(txtCategoryName.Text))
                    {
                        MessageWarning("Can't Add \n No text input","Add");
                    }
                    else
                    {
                        if (Existed(txtCategoryName.Text) < 1)
                        {
                            Add(txtCategoryName.Text);
                        }
                        else
                        {
                            MessageWarning("Category Name Already Exist !", "Error Occur");
                        }
                
                    }
                   
                }
                DisableBackground(false);
                txtCategoryName.Clear();
                txtCategoryID.Text="Auto Generate";
                Load();
            }
            else if (sender == btnCancel)
            {
                panelCategoryInput.Visible = false;
                DisableBackground(false);
                txtCategoryName.Clear();
                txtCategoryID.Text="Auto Generate";
            }
            else if (sender == btnRefresh)
            {
                try
                {
                    Load();
                }
                catch (Exception ex)
                {
                    MessageError("Error Loading\n" + ex.Message, "Error");
                }

            }
            else if (sender == btnSearch)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                 dgvCategory.DataSource = null;
                 dgvCategory.Rows.Clear();
                 try
                 {
                     cnn.Open();
                     try
                     {
                         cmd = new SqlCommand("SELECT * FROM Category WHERE cateID=@searchint OR cateName=@searchstring", cnn);
                         cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text);
                         cmd.Parameters.AddWithValue("@searchint", int.Parse(txtSearch.Text));
                     }
                     catch (Exception)
                     {
                         cmd = new SqlCommand("SELECT * FROM Category WHERE cateName=@searchstring", cnn);
                         cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text);
                     }
                     SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                     DataTable dt = new DataTable();
                     adapter.Fill(dt);
                     dgvCategory.DataSource = dt;
                 }
                 catch (Exception ex)
                 {
                     MessageError(ex.Message, "Error");
                 }
                 finally
                 {
                     cnn.Close();
                     cmd.Dispose();
                 }
                }
                

            }
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
            MessageBox.Show(des,title,MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }

        public DialogResult MessageVerify(string des, string title)
        {
            return MessageBox.Show(des, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        void DisableBackground(bool state)
        {
            btnAdd.Enabled = !state;
            btnDelete.Enabled = !state;
            btnEdit.Enabled = !state;
            btnRefresh.Enabled = !state;
            btnSearch.Enabled = !state;
            txtSearch.Enabled = !state;
            dgvCategory.Enabled = !state;
        }
    }
}
