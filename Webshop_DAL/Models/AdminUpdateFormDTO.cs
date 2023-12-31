using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Models
{
    public class AdminUpdateFormDTO: VendeurUpdateFormDTO
    {
        public int Id { get; set; }     
        public int? Role { get; set; }
    }
}
