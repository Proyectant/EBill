using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacuni.Shared.Models
{
    public class Product 
    {
        public string ID { get; set; } //unikatan
        public string titleProduct { get; set; } //naziv
        public double price { get; set; } //cena 
        public int contentProduct { get; set; } //kolicina 
      

        public Product() { }
        public Product (string Title, double Price)
        {
            titleProduct = Title;
            price = Price;
        }
        public Product(string Title, double Price, int Content) : this(Title, Price)
        {
            contentProduct = Content;
        }


    }
}
