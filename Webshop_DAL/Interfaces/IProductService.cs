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
        IEnumerable<Product> GetProductBySearch(string search, int? id);
        IEnumerable<Product> GetProductByCategory(int idCategory, int? id);
        IEnumerable<Product> GetAll();
        bool Createproduct(ProductFormDTO newProduct, int? id);
        Product UpdateProduct(ProductFormDTO product,int idProduct,int? id = null);
        bool DeleteProduct(int idProduct,int? id = null);
        IEnumerable<Product> GetAllVendeur(int? id);
        bool RatingProduct(Evaluation rating, int? id);
        string LeaveComment(Commentaires commentaire, int? id);
        bool UpdateComment(Commentaires commentaire, int? id);
        bool DeleteComment(int commentaire, int? id);
    }
}
