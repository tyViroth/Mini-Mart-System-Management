using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Mart;

namespace Mart
{
    public class Controller
    {

        private static SqlConnection con = Connection.getConnection();
        private static SqlDataAdapter da;
        private static DataTable dt;
        private static SqlCommand cmd;
        
        /*public static void d(DataGridViewColumn col)
        {
            DataGridView data = new DataGridView();          
            col.Resizable = DataGridViewTriState.False;
        }*/        

        public static void NonSortableDataGridView(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        
        public static void SetImageLayout(DataGridViewColumn colImage)
        {
            try
            {
                 DataGridViewImageColumn img = new DataGridViewImageColumn();
                img = (DataGridViewImageColumn) colImage;
                img.ImageLayout = DataGridViewImageCellLayout.Stretch;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);    
            }
        }

        public static void AlignHeaderTextCenter(params DataGridViewColumn[] col)
        {
            foreach (DataGridViewColumn c in col)
            {
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }            
        }

        public static void FillComboBoxValue(ComboBox cbo,string fieldID,string fieldDes,string procedureName)
        {            
            try
            {                                
                con.Open();
                da = new SqlDataAdapter(procedureName,con);
                dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.DisplayMember = fieldDes;
                cbo.ValueMember = fieldID;                
            }
            catch (Exception e)
            {
                MessageBox.Show("Fill combobox: "+e.Message);
            }
            finally
            {                
                da.Dispose();
                con.Close();
            }
        }

        public static void FillComboBoxValue(ComboBox cbo, string fieldID, string fieldDes, string procedureName,int year)
        {
            cmd = null;
            try
            {
                con.Open();
                cmd = new SqlCommand(procedureName,con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@year",year);
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.DisplayMember = fieldDes;
                cbo.ValueMember = fieldID;                        
            }
            catch (Exception e)
            {
                MessageBox.Show("Fill combobox: " + e.Message);
            }
            finally
            {
                try
                {                    
                    da.Dispose();
                    con.Close();
                }
                catch (NullReferenceException ex){ }
            }
        }

        public static int GetLastAutoIncrement(string tbName)
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

        public static bool IsExistUsername(string value)
        {
            SqlDataReader sdr = null;
            bool has = false;
            try
            {
               
                con.Open();
                cmd = new SqlCommand("SELECT TOP 1 username FROM Employee WHERE username = @val;",con);
                cmd.Parameters.AddWithValue("@val",value);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows) has = true;
                else has = false;
            }
            catch (Exception e)
            {
                has = true;
                MessageBox.Show(e.Message,"Check Exist Value");
            }
            finally
            {
                sdr.Close();
                cmd.Dispose();
                con.Close();
            }
            return has;
        }

    }
}
