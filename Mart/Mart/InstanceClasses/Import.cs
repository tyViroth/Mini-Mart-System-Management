using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.InstanceClasses
{
    class Import
    {
        public int Id { get; set; }
        public DateTime ImpDate { get; set; }
        public Supplier Sup { get; set; }
        public Employee Emp { get; set; }
        public double TotalAmount { get; set; }

        public Import(int id, DateTime impDate, Supplier sup, Employee emp, double totalAmount)
        {
            Id = id;
            ImpDate = impDate;
            Sup = sup;
            Emp = emp;
            TotalAmount = totalAmount;
        }
    }
}
