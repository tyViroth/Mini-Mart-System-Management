using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace Mart
{
     public class Connection
    {
         private Configuration config;

        public static SqlConnection getConnection()
        {           
            //string conString = @"Data Source=;Initial Catalog=Mart;Integrated Security=True";  
            Connection con = new Connection();
            return new SqlConnection(con.GetConnectionString("LocalConnection"));
        }

        public Connection()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetConnectionString(string key)
        {
            return config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public bool SaveConnectionString(string key,string value)
        {
            try
            {
                config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
                config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

    }
}