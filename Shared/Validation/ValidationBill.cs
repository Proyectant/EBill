using ERacuni.Shared.Models;
using FluentValidation;

namespace ERacuni.Shared.Validation
{
    public class ValidationBill : AbstractValidator<Bill>
    {
        public ValidationBill()
        {
            RuleFor(bill => bill.barcode).NotNull().WithMessage("Ne moze da bude null")/*.NotEmpty().WithMessage("Morate uneti barcode")*/;
            //RuleFor(bill => bill.titleBill).NotNull();
            //RuleFor(bill => bill.ransom).NotNull();
            //RuleFor(bill => bill.shippingFee).NotNull();
            //RuleFor(bill => bill.wayOfSelling).NotNull().WithMessage("ne sme biti null").NotEmpty().WithMessage("ne sme biti prazno");
            //RuleFor(bill => bill.postingDate).NotNull();
            //RuleFor(bill => bill.receiptDate).NotNull();
            //RuleFor(bill => bill.customerAdress).NotNull();
       
        }
    }
}