using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace Webshop_DAL.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IConfiguration config) : base(config)
        {
        }
        public IEnumerable<Product> GetAll()
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"SELECT p.Id, p.[Name], p.[Quantity], p.[Price], p.[Description], p.[Image], c.[Name] as Category, u.[Username] as Vendeur " +
                                            $"FROM Articles p " +
                                            $"JOIN Category c on p.[Id_Category] = c.[Id] " +
                                            $"JOIN Users u on p.[Id_Vendeur] = u.[Id] ";

                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Mapper(reader);
                }
            }
        }

        public IEnumerable<Product> GetProductByCategory(int id)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT p.Id, p.[Name], p.[Quantity], p.[Price], p.[Description], p.[Image], c.[Name] as Category, u.[Username] as Vendeur "+
                                           $"FROM Articles p "+
                                           $"JOIN Category c ON p.[Id_Category] = c.[Id] "+
                                           $"JOIN Users u ON p.[Id_Vendeur] = u.[Id] "+
                                           $"WHERE c.Id = @id";
                    GenerateParameter(command, "id", id);

                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Mapper(reader);
                }
            }
        }

        public IEnumerable<Product> GetProductBySearch(string search)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT p.Id, p.[Name], p.[Quantity], p.[Price], p.[Description], p.[Image], c.[Name] as Category, u.[Username] as Vendeur " +
                                           $"FROM Articles p " +
                                           $"JOIN Category c ON p.[Id_Category] = c.[Id] " +
                                           $"JOIN Users u ON p.[Id_Vendeur] = u.[Id] " +
                                           $"WHERE p.Name LIKE '%' +@search + '%'";
                    GenerateParameter(command, "search", search);

                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Mapper(reader);
                }
            }
        }

        public override Product Mapper(IDataReader dataReader)
        {
            return new Product
            {
                Id = (int)dataReader["id"],
                Name = (string)dataReader["Name"],
                Quantity = (int)dataReader["Quantity"],
                Price = (double)dataReader["Price"],
                Description = dataReader["Description"] ==DBNull.Value ? null : (string)dataReader["Description"],
                Image = dataReader["Image"] == DBNull.Value ? null : (string)dataReader["Image"],
                Category = (string)dataReader["Category"],
                VendeurName = (string)dataReader["Vendeur"]
            };    
        }

        public bool VendeurCreateproduct(ProductFormDTO newProduct, int? id)
        {
            throw new NotImplementedException();
        }

        public bool VendeurDeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Product VendeurUpdateProduct(ProductFormDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
