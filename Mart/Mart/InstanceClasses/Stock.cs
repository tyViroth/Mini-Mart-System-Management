using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.InstanceClasses
{
    class Stock
    {
        public Stock(int v1, string v2, int v3)
        {
            this.ProID = v1;
            this.ProName = v2;
            this.ImpID = v3;
        }

        public int ProID { get; set; }
        public string ProName { get; set; }
        public int ImpID { get; set; }


    }
}
