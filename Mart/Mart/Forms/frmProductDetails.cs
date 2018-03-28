using Mart.Intefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.InstanceClasses;
using Mart.InstanceClass;
using System.Text.RegularExpressions;

namespace Mart.Forms
{
    public partial class frmProductDetails : Form,IMessageType,IFunctionModel<Product>
    {
        #region
        private bool readOnly;
        private bool update;
        private bool insert;
        private int dataIndex = 0;
        private bool needUpdate=false;
        public bool SuccesUpdate=false;
        private string imagePath="";
        SqlConnection cnn = Connection.getConnection();
        SqlCommand cmd;
        SqlDataReader reader;
        Product pro;
        #endregion
        public frmProductDetails(int state,int index)
        {
            InitializeComponent();
            readOnly = (state==0);
            insert = (state == 1);
            update = (state == 2);
            dataIndex = index;
            this.Load += LoadForm;    
        }
        private void RegisterEvent()
        { 
            btnCancel.Click += ButtonAction;
            btnConfirm.Click += ButtonAction;
            txtProductName.TextChanged += TextChangeAction;
            txtPrice.TextChanged += TextChangeAction;
            txtPrice.KeyPress += txtPrice_KeyPress;
            txtType.TextChanged += TextChangeAction;
            txtDescription.TextChanged += TextChangeAction;
            cboCategory.SelectedIndexChanged += TextChangeAction;
            cboCategory.KeyPress += cboCategory_KeyPress;
            txtDescription.TextChanged += TextChangeAction;
            pictureProduct.Click += ChangPicture;
            lblpi.Click += ChangPicture;
        }

        void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            float fl= 1;
            try
            {
                if (txtPrice.Text.Trim() == fl.ToString().Trim())
                {
                    Input.InputNmber((TextBox)sender, e);                    
                }
            }
            catch(Exception ex)
            {
            
            }
                
        }
        private void ChangPicture(object sender, EventArgs e)
        {
            try
            {
                
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select picture";
                ofd.Filter = "Image *PNG, *JPG | *.PNG; *.JPG";
                DialogResult dialogResult = ofd.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    imagePath = ofd.FileName;
                    pictureProduct.Image = Image.FromFile(imagePath);
                    needUpdate = true;
                }
                needUpdate = true;
            }
            catch (Exception ex)
            {
                MessageError("Can't Change\n" + ex.Message, "Error Loading Image");   
            }
        }
        private void TextChangeAction(object sender, EventArgs e)
        {
            needUpdate = true;
        }
        private void ButtonAction(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                CloseForm();
            }
            else if (sender == btnConfirm)
            {
                if (insert)
                {
                    if (Checkrequirement())
                    {
                        SuccesUpdate = InsertData();
                        if (SuccesUpdate)this.Close();
                    }
                }
                else if (needUpdate && update)
                {
                    if (Checkrequirement())
                    {
                        SuccesUpdate = UpdateData(dataIndex);
                        if (SuccesUpdate) this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        private bool Checkrequirement()
        {
            float price = 0;
            string proName = "";
            string type = "";
            string desc = "";
            int cateid=0;
            byte[] photo = pro.photo;
            bool enough = true;
            try
            {
                /*Check Product Name TextBox*/
                if (string.IsNullOrEmpty(txtProductName.Text))
                {
                    enough = false;
                    errorP.SetError(txtProductName, "Can't Leave Empty");
                }
                else
                {
                    proName = txtProductName.Text;
                }
                /*Check Image*/
                if (!string.IsNullOrEmpty(imagePath))
                {
                    try
                    {
                        Image myImg = Image.FromFile(imagePath);
                        photo = ImageToByteArray(myImg);
                    }
                    catch (Exception)
                    {
                        enough = false;
                    }
                }
                /*Check PriceTextBox*/
                if (!string.IsNullOrEmpty(txtPrice.Text))
                {
                    if (float.TryParse(txtPrice.Text, out price) == false)
                    {
                        enough = false;
                    }
                }
                else 
                {
                    enough = false;
                    errorP.SetError(txtPrice,"Can't Leave Empty");
                }
                    
                /*Optional for Product Type and Description*/
                if (!string.IsNullOrEmpty(txtType.Text)) { type = txtType.Text; }
                if (!string.IsNullOrEmpty(txtDescription.Text)) { desc = txtDescription.Text; }
                /*get Category ID*/
                cateid=(int)cboCategory.SelectedValue;
            }
            catch (Exception)
            {
                enough = false;
            }
            if (enough)
            {
                pro = new Product(dataIndex, proName, price, cateid, type, desc, photo);
            }
            else 
            {
                MessageError("Some data is invalid","Error");
            }
            return enough;
        }
        private bool UpdateData(int dataIndex)
        {
            bool success = false;
            try
            {
                cnn.Open();
                cmd = new SqlCommand("UPDATE Product SET proName=@name,price=@price,cateID=@cate,proType=@type,proTypeDes=@description,photo=@photo WHERE proID=@index", cnn);
                cmd.Parameters.AddWithValue("@name", pro.proName);
                cmd.Parameters.AddWithValue("@price",pro.price);
                cmd.Parameters.AddWithValue("@cate",pro.cateID);
                cmd.Parameters.AddWithValue("@type", pro.proType);
                cmd.Parameters.AddWithValue("@description", pro.description);
                cmd.Parameters.AddWithValue("@photo",pro.photo);
                cmd.Parameters.AddWithValue("@index", dataIndex);
                if(cmd.ExecuteNonQuery()>0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"Error Update");
            }
            finally
            {
                cnn.Close();
                cmd.Dispose();
            }
            return success;
        }   
        private void CloseForm()
        {
            if (needUpdate)
            {
                if (MessageVerify("Your chang will not save !\nDiscard Change ?", "Update") == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else 
            {
                this.Close();            
            }
        }
        private void LoadForm(object sender, EventArgs e)
        {
            SetReadOnly(readOnly);
            Controller.FillComboBoxValue(cboCategory, "cateID", "cateName", "SELECT * FROM Category");
            if (!insert)
            {
                LoadDatabase();
            }
            else
            {
                    Image img = pictureProduct.Image;
                    byte[] image=ImageToByteArray(img);
                    pro = null;
                    pro = new Product(1, "None", 0, 1, "", "", image);        
            }
            
            if (this.readOnly == true)
            {
                btnCancel.Text = "Done";
            }
            RegisterEvent();
        }
        private bool InsertData()
        {
            bool succes = false;
            try
            {
                cnn.Open();
                cmd = new SqlCommand("Insert Product(proName,price,cateID,photo,proType,proTypeDes) VALUES(@name,@price,@cate,@photo,@type,@description)", cnn);
                cmd.Parameters.AddWithValue("@name", pro.proName);
                cmd.Parameters.AddWithValue("@price", pro.price);
                cmd.Parameters.AddWithValue("@cate", pro.cateID);
                cmd.Parameters.AddWithValue("@type", pro.proType);
                cmd.Parameters.AddWithValue("@description", pro.description);
                cmd.Parameters.AddWithValue("@photo",pro.photo);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    succes = true;
                }

            }
            catch (Exception ex)
            {
                MessageError("Error Insert\n" + ex.Message, "Insert");
            }
            finally
            {
                cnn.Close();
                cmd.Dispose();
            }
            return succes;
        }
        private void LoadDatabase()
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand("SELECT * FROM Product WHERE proID=@id",cnn);
                cmd.Parameters.AddWithValue("@id",dataIndex);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id=(int)reader["proID"];
                    string name=(string)reader["proName"];
                    double price=double.Parse(reader["price"].ToString());
                    int cateid = (int)reader["cateID"];
                    string type=reader["proType"].ToString();
                    string des = reader["proTypeDes"].ToString();
                    byte[] img = (byte[])reader["photo"];

                    pro=new Product(id,name,price,cateid,type,des,img);
                }            
                txtProductID.Text = pro.proID.ToString();
                txtProductName.Text = pro.proName;
                txtPrice.Text = pro.price.ToString();
                cboCategory.SelectedValue = pro.cateID;
                txtType.Text = pro.proType;
                txtDescription.Text = pro.description;
                pictureProduct.Image = Image.FromStream(new MemoryStream(pro.photo));
            }
            catch (Exception ex)
            {
                MessageError(ex.Message,"Error Load Image");              
            }
            finally
            {
                cnn.Close();
                cmd.Dispose();
            }
        }
        private void SetReadOnly(bool state)
        {
            this.txtProductID.ReadOnly = true;
            this.txtProductName.ReadOnly = state ;
            this.txtPrice.ReadOnly = state;
            this.txtType.ReadOnly = state;
            this.cboCategory.Enabled = !state;
            this.txtDescription.ReadOnly = state;
            this.pictureProduct.Enabled = !state;
            btnConfirm.Enabled = !state;
            btnConfirm.Visible = !state;
            lblpi.Visible = !state;
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
        }
        public bool Insert(Product obj)
        {
            return false;
        }
        public bool Update(Product obj)
        {
         bool success= false;

         return success;
        }
        public bool Delete(int id)
        {
            return false;
        }
    }
}