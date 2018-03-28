using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mart.UserControls;

namespace Mart.ControlClasses
{
    public class sold
    {
        private string _name;
        private int _quantity;
        private double _price;
        private string _date;
        private string _discount;
        private string _tax;

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
       
        public sold(string Name, int Quantity, double Price, DateTime Date, string Discount, string Tax)
        {
            this.Name = Name;
            this.Quantity = Quantity;
            this.Price = Price;
            this.Date = Date;
            this.Discount = Discount;
            this.Tax = Tax;
        }

        public sold(string name, int quantity, double price, string date, string discount, string tax)
        {
            _name = name;
            _quantity = quantity;
            _price = price;
            _date = date;
            _discount = discount;
            _tax = tax;
        }

        public sold()
        {
        }

        public override string ToString()
        {
            return string.Format("name = {0}, qty = {1}, price = {2}, data= {3}, discount = {4}, tax = {5}", _name, _quantity, _price, _date, _discount, _tax);
        }
    }
}
