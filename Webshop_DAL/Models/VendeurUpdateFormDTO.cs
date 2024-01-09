using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Models
{
    public class VendeurUpdateFormDTO : UserUpdateFormDTO
    {
        public int TVA { get; set; }
    }
}
