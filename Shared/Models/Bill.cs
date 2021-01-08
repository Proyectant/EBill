using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacuni.Shared.Models
{
    public class Bill
    {
        public string barcode { get; set; }  //unikatan je za ovu klasu 
        public string titleBill { get; set; } //naziv racuna 
        public string customerAdress { get; set; } //adresa kupca 
        public double shippingFee { get; set; } //postarina 
        public double ransom { get; set; } //ukupna vrednost 
        public DateTime postingDate { get; set; } = DateTime.UtcNow;//datum slanja posiljke
        public DateTime receiptDate { get; set; }   //datum prijema posiljke 
        public List<Product> productsForBill { get; set; } = new List<Product>(); //lista artikala na racunu
        public string wayOfSelling { get; set; } = "Poštom"; //nacin prodaje (preko bankovnog racuna, preko oglasa ili od kuce)
      
        public Bill() { }
        public Bill(string Barcode,DateTime Posting, DateTime Receiving, double Ransom, string Way)
        {
            barcode = Barcode;
            postingDate = Posting;
            receiptDate = Receiving;
            ransom = Ransom;
            wayOfSelling = Way;
        }


        public Bill(string Barcode, DateTime Posting, DateTime Receiving, double Ransom, string Way, double Fee) : this(Barcode, Posting, Receiving, Ransom, Way)
        {
            shippingFee = Fee;
        }

        public override string ToString()
        {
            return this.barcode + this.titleBill;
        }

    }
}
