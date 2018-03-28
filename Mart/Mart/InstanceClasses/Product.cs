using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.InstanceClass
{
    public class Product
    {
        public int proID { get; set; }
        public string proName { get; set; }
        public double price { set; get; }
        public int cateID { get; set; }
        public string proType { get; set; }
        public string description { get; set; }
        public byte[] photo { get; set; }
        public Product(int id,string name,double price,int cateid,string proType,string des,byte[] photo)
        {
            this.proID = id;
            this.proName = name;
            this.price = price;
            this.cateID = cateid;
            this.photo = photo;
            this.proType = proType;           
            this.description = des;          
        }
        public void SetProduct(int id, string name,double price, int cateid, string proType, string des, byte[] photo)
        {
            this.proID = id;
            this.proName = name;
            this.price = price;
            this.cateID = cateid;
            this.photo = photo;
            this.proType = proType;
            this.description = des;
        }
    }
}
