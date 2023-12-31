using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Models
{
    public class CreateVendeurDTO : CreateUserDTO
    {
        [Required]
        public int TVA { get; set; }

        [Required]
        [RegularExpression("\\bBE[0-9]{14}\\b")]
        public new string Iban { get; set; }
    }
}
