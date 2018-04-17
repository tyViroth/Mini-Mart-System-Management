using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.InstanceClasses
{
    class ImportDetail
    {
        public int Id { get; set; }
       // public Produ Pro { get; set; }
        public Import Imp { get; set; }
        public double ImpQty { get; set; }
        public double ImpPrice { get; set; }
        public double Amount { get; set; }
        public double SoldQty { get; set; }

        public ImportDetail(int id, Import imp, double impQty, double impPrice, double amount, double soldQty)
        {
            Id = id;
            Imp = imp;
            ImpQty = ImpQty;
            ImpPrice = impPrice;
            Amount = amount;
            SoldQty = soldQty;
        }
    }
}
