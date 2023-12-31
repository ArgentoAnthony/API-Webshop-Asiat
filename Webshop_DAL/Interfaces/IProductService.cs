using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Models;

namespace Webshop_DAL.Interfaces
{
    public interface IProductService : IBaseService<Product>
    {
        IEnumerable<Product> GetProductBySearch(string search);
        IEnumerable<Product> GetProductByCategory(int id);
        IEnumerable<Product> GetAll();
        bool VendeurCreateproduct(ProductFormDTO newProduct, int? id);
        Product VendeurUpdateProduct(ProductFormDTO product);
        bool VendeurDeleteProduct(int id);
    }
}
