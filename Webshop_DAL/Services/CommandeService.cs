using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace Webshop_DAL.Services
{
    public class CommandeService : BaseService<Product>, ICommandeService
    {
        public CommandeService(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<Product> GetCommands()
        {
            throw new NotImplementedException();
        }

        public bool BuyCommand(List<Product> products)
        {
            throw new NotImplementedException();
        }

        public override Product Mapper(IDataReader dataReader)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetCommands(int? v)
        {
            throw new NotImplementedException();
        }
    }
}
