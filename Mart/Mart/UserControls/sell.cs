using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart
{
    class sell
    {
        public string name { get; set; }
        public int quanity { get; set; }
        public string product_id { get; set; }
        public double discount { get; set; }
        public double tax { get; set; }

        public sell(string naem, int quantity, string product_id, double discount, double tax)
        {
            this.name = name;
            this.quanity = quanity;
            this.product_id = product_id;
            this.discount = discount;
            this.tax = tax;
        }
    }
}
