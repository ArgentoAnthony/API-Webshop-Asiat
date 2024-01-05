using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Models;

namespace Webshop_DAL.Interfaces
{
    public interface ICommandeService : IBaseService<Product>
    {
        IEnumerable<Product> GetCommands(int? v);
        bool BuyCommand(List<Product> products);
    }
}
