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
                Category = dataReader["Category"].ToString(),
                VendeurName = dataReader["Vendeur"].ToString()
            };    
        }
        private ProductMiniature MapperMiniature(IDataReader dataReader)
        {
            return new ProductMiniature
            {
                Name = (string)dataReader["Name"],
                Price = (double)dataReader["Price"],
                Description = dataReader["Description"] == DBNull.Value ? null : (string)dataReader["Description"],
                Image = dataReader["Image"] == DBNull.Value ? null : (string)dataReader["Image"]
            };
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
        public int GetCategory(string category)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT Id FROM Category WHERE Name LIKE @name";
                    GenerateParameter(command, "name", category);

                    return (int)command.ExecuteScalar();
                }
            }
        }
        public IEnumerable<Product> GetProductByCategory(int idCategory)
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
                                           $"WHERE c.Id = @idCategory";
                    GenerateParameter(command, "idCategory", idCategory);


                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return Mapper(reader);
                    }
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
                    command.CommandText = $"SELECT p.Id, p.[Name], p.[Quantity], p.[Price], p.[Description], p.[Image], c.[Name] as Category, u.[Username] as Vendeur, p.Id_Category " +
                                           $"FROM Articles p " +
                                           $"JOIN Category c ON p.[Id_Category] = c.[Id] " +
                                           $"JOIN Users u ON p.[Id_Vendeur] = u.[Id] " +
                                           $"WHERE p.Name LIKE '%' +@search + '%'";
                    GenerateParameter(command, "search", search);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return Mapper(reader);
                        }
                    }
                }
            }
        }
        public bool Createproduct(ProductFormDTO newProduct, int? id)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Articles " +
                                          $"VALUES(@name, @quantity, @price, @description, @image, @category, @vendeur)";

                    GenerateParameter(command, "name", newProduct.Name);
                    GenerateParameter(command, "quantity", newProduct.Quantity);
                    GenerateParameter(command, "price", newProduct.Price);
                    GenerateParameter(command, "description", newProduct.Description);
                    GenerateParameter(command, "image", newProduct.Image);
                    GenerateParameter(command, "category", newProduct.Category);
                    GenerateParameter(command, "vendeur", id);

                    return command.ExecuteNonQuery() ==1;
                }
            }
        }
        public Product UpdateProduct(ProductFormDTO product, int IdProduct, int? id)
        {
            Product updatedProduct;
            int categoryId;
            int vendeurId;
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Articles  SET " +
                                          $"Name = @name, Quantity = @quantity, Price = @price, " +
                                          $"[Description] = @description, [Image] = @image, Id_Category = @category " +
                                          $"OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Quantity, INSERTED.Price, " +
                                          $"INSERTED.Description, INSERTED.Image, INSERTED.Id_Category AS Category, INSERTED.Id_Vendeur AS Vendeur "+
                                          $"WHERE Id = @idProduct ";
                    if (id is not null)
                    {
                        command.CommandText += "AND Id_Vendeur = @idVendeur";
                        GenerateParameter(command, "idVendeur", id);
                    }
                        

                    GenerateParameter(command,"name", product.Name);
                    GenerateParameter(command,"quantity", product.Quantity);
                    GenerateParameter(command,"price", product.Price);
                    GenerateParameter(command,"description", product.Description);
                    GenerateParameter(command,"image", product.Image);
                    GenerateParameter(command,"category", product.Category);
                    GenerateParameter(command,"idProduct", IdProduct);
                    

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            updatedProduct = Mapper(reader);

                            categoryId = Convert.ToInt32(reader["Category"]);
                            vendeurId = Convert.ToInt32(reader["Vendeur"]);
                        }
                        else
                            throw new Exception("Erreur premier reader");
                    }

                    using (IDbCommand categoryCommand = connection.CreateCommand())
                    {
                        categoryCommand.CommandText = $"SELECT Name FROM Category WHERE Id = @categoryId";
                        GenerateParameter(categoryCommand, "categoryId", categoryId);

                        object categoryName = categoryCommand.ExecuteScalar();
                        updatedProduct.Category = categoryName?.ToString();
                    }

                    using (IDbCommand vendeurCommand = connection.CreateCommand())
                    {

                        vendeurCommand.CommandText = $"SELECT [Username] FROM Users WHERE Id = @vendeurId";
                        GenerateParameter(vendeurCommand, "vendeurId", vendeurId);

                        object vendeurName = vendeurCommand.ExecuteScalar();
                        updatedProduct.VendeurName = vendeurName?.ToString();
                    }

                    return updatedProduct;
                }
            }
        }
        public bool DeleteProduct(int idProduct, int? id)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Articles " +
                                         $"WHERE Id = @idProduct ";

                    if(id is not null)
                    {
                        command.CommandText += $"AND Id_Vendeur = @IdVendeur";
                        GenerateParameter(command, "idVendeur", id);
                    }
                    GenerateParameter(command , "idProduct", idProduct);
                    

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public void AddToRecommandation(int? id, int idCategory)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"INSERT INTO Recommandations VALUES (@idUser, @idCategory)";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idCategory", idCategory);

                    command.ExecuteNonQuery();
                }
            }         
        }
        public IEnumerable<Product> GetAllVendeur( int? id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"SELECT p.Id, p.[Name], p.[Quantity], p.[Price], p.[Description], p.[Image], c.[Name] as Category, u.[Username] as Vendeur " +
                                            $"FROM Articles p " +
                                            $"JOIN Category c on p.[Id_Category] = c.[Id] " +
                                            $"JOIN Users u on p.[Id_Vendeur] = u.[Id] "+
                                            $"WHERE p.Id_Vendeur = @id";

                    GenerateParameter(command, "id", id);

                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Mapper(reader);
                }
            }
        }
        public IEnumerable<ProductMiniature> GetRecommendedItems(int? id)
        {
            List<int> idsCategory = new();
            List<ProductMiniature> products = new();
            Random rand = new();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT DISTINCT Id_Category FROM Recommandations WHERE Id_User = @idUser";
                    GenerateParameter(command, "idUser", id);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            idsCategory.Add((int)reader["Id_Category"]);
                    }
                }

                foreach (int idCategory in idsCategory)
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Name, Price, [Description], [Image] FROM Articles WHERE Id_Category = @idCategory " +
                        "ORDER BY NEWID() " +
                        "OFFSET 0 ROWS " +
                        "FETCH NEXT @rand ROWS ONLY";
                        GenerateParameter(command, "idCategory", idCategory);
                        GenerateParameter(command, "rand", rand.Next(1, 5));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                products.Add(MapperMiniature(reader));
                        }
                    }
                }
                return products;
            }
        }
    }
}
