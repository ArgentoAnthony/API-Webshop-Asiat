using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.CustomAttributes
{
    public class StrictlyPositiveAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int intValue && intValue <= 0)
                return new ValidationResult("La valeur doit etre supérieure à 0");
            else if(value is double doubleValue && doubleValue <= 0)
                return new ValidationResult("La valeur doit etre supérieure à 0");

            return ValidationResult.Success;
        }
    }
}
