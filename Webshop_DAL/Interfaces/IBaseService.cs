using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_DAL.Interfaces
{
    public interface IBaseService<TModel>
    {
        void Delete(string tableName, int id);
        IEnumerable<TModel> GetAll(string tableName);
        TModel GetById(string tableName, int id);
    }
}
