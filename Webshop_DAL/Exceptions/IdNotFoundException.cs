using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Exceptions
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException()
        {
        }
        public IdNotFoundException(string message) :base(message) 
        {
        }

    }
}
