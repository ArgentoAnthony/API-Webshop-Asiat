using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.CustomAttributes;

namespace Webshop_DAL.Models
{
    public class ProductFormDTO
    {

        [Required]
        public string Name { get; set; }
        [Required]
        [StrictlyPositive]
        public int Quantity { get; set; }
        [Required]
        [StrictlyPositive]
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required]
        public int Category { get; set; }
    }
}
