using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Models
{
    public class Product : ProductMiniature
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string VendeurName { get; set; }


    }
}
