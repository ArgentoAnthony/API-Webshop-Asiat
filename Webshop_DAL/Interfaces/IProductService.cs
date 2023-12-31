﻿using System;
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
        IEnumerable<Product> GetProductByCategory(int idCategory);
        IEnumerable<Product> GetAll();
        bool Createproduct(ProductFormDTO newProduct, int? id);
        Product UpdateProduct(ProductFormDTO product,int idProduct,int? id = null);
        bool DeleteProduct(int idProduct,int? id = null);
        IEnumerable<Product> GetAllVendeur(int? id);
        IEnumerable<ProductMiniature> GetRecommendedItems(int? id);
        void AddToRecommandation(int? id, int idCategory);
        int GetCategory(string category);
        Product GetOne(int id);
    }
}
