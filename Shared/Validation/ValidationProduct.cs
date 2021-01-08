using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERacuni.Shared.Models;
using FluentValidation;

namespace ERacuni.Shared.Validation
{
    public class ValidationProduct : AbstractValidator<Product>
    {
        public ValidationProduct()
        {
            RuleFor(product => product.titleProduct).NotNull().WithMessage("Ne moze da bude null")/*.NotEmpty().WithMessage("Morate uneti barcode")*/;

        }

    }
}
