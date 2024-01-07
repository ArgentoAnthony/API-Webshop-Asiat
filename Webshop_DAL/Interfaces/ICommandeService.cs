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
        IEnumerable<Commande> GetCommands(int? id = null);
        void BuyCommand(List<Product> products, int? id);
        Commande GetCommandByCommandNumber(Guid commandNumber);
        void DeleteCommande(Guid commandNumber);
    }
}
