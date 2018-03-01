using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Mart
{
     public class Connection
    { 
        public static SqlConnection getConnection()
        {
            string conString = @"Data Source=THOURA-PC;Initial Catalog=Mart;Integrated Security=True";
            return new SqlConnection(conString);
        }
    }
}
