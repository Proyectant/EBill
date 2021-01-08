using ERacuni.Shared.DTO;
using ERacuni.Shared.Models;
using ERacuniProtoNameSpace;
using Google.Protobuf.WellKnownTypes;
using System;

namespace ERacuni.Shared.Convertors
{
    public class ConvertGRPC
    {
        public BillMsg Convert(Bill bill)
        {

            if (bill.titleBill == null)
            {
                bill.titleBill = string.Empty;
            }
            if (bill.customerAdress == null)
            {
                bill.customerAdress = string.Empty;
            }

            BillMsg billMsg = new BillMsg
            {
                Barcode = bill.barcode,
                TitleBill = System.Convert.ToString(bill.titleBill),
                CustomerAdress = System.Convert.ToString(bill.customerAdress),
                ShippingFee = bill.shippingFee,
                Ransom = bill.ransom,
                PostingDate = DateTime.SpecifyKind(bill.postingDate, DateTimeKind.Utc).ToTimestamp(),
                ReceiptDate = DateTime.SpecifyKind(bill.receiptDate, DateTimeKind.Utc).ToTimestamp(),
                WayOfSelling = System.Convert.ToString(bill.wayOfSelling)
            };
            return billMsg;
        }

        public Bill Convert(BillMsg billmsg)
        {
            Bill bill = new Bill
            {
                barcode = billmsg.Barcode,
                titleBill = billmsg.TitleBill,
                customerAdress = billmsg.CustomerAdress,
                shippingFee = billmsg.ShippingFee,
                ransom = billmsg.Ransom,
                postingDate = billmsg.PostingDate.ToDateTime(),
                receiptDate = billmsg.ReceiptDate.ToDateTime(),
                wayOfSelling = billmsg.WayOfSelling
            };
            return bill;
        }

        public ListForReportRequest Convert(DateAndWayOfSelling dw)
        {
            ListForReportRequest lst = new ListForReportRequest
            {
                Start = DateTime.SpecifyKind(dw.start, DateTimeKind.Utc).ToTimestamp(),
                End = DateTime.SpecifyKind(dw.end, DateTimeKind.Utc).ToTimestamp(),
                WayOfSelling = dw.wayOfSelling,
                TypeOfDate = dw.TypeOfDate

            };
            return lst;
        }

        public DateAndWayOfSelling Convert(ListForReportRequest lst)
        {
            DateAndWayOfSelling dateAndWayOfSelling = new DateAndWayOfSelling
            {
                start = lst.Start.ToDateTime(),
                end = lst.End.ToDateTime(),
                wayOfSelling = lst.WayOfSelling,
                TypeOfDate = lst.TypeOfDate
            };
            return dateAndWayOfSelling;
        }

        public Product Convert(ProductMSG productMSG)
        {
            Product product = new Product
            {
                ID = productMSG.IDMSG,
                titleProduct = productMSG.TitleProductMSG,
                price = productMSG.PriceMSG,
                contentProduct = productMSG.ContentProductMSG
            };
            return product;
        }

        public ProductMSG Convert(Product product)
        {
            if (product.titleProduct==null)
            {
                product.titleProduct = string.Empty;
            }

            ProductMSG productMSG = new ProductMSG
            {
                IDMSG = product.ID,
                TitleProductMSG = product.titleProduct,
                PriceMSG = product.price,
                ContentProductMSG = product.contentProduct
            };
            return productMSG;
        }
    }
}