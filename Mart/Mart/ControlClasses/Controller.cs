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
                MessageBox.Show("Fill combobox"+e.Message);
            }
            finally
            {
                da.Dispose();
                con.Close();
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
    }
}
