using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException()
        {
        }
        public RoleNotFoundException(string message) : base(message)
        {
        }
    }

}
