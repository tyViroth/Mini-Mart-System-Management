using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Mart.InstanceClasses
{
    public class SellProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }

        public SellProduct(int id, string name, double price)
        {
            this.ProductID = id;
            this.ProductName = name;
            this.Price = price;
        }
    }
}
