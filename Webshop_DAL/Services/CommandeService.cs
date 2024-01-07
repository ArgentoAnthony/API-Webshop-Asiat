using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace Webshop_DAL.Services
{
    public class CommandeService : ProductService, ICommandeService
    {
        public CommandeService(IConfiguration config) : base(config)
        {
        }
        public void BuyCommand(List<Product> products, int? id)
        {
            Guid commandNumber = Guid.NewGuid();

            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Commande VALUES (@commandNumber, @idUser)";
                    GenerateParameter(command, "commandNumber", commandNumber);
                    GenerateParameter(command, "idUser", id);

                    command.ExecuteNonQuery();
                }
            }

            foreach(Product product in products)
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"INSERT INTO CommandList VALUES (@idProduct, @commandNumber)";
                        GenerateParameter(command, "idProduct", product.Id);
                        GenerateParameter(command, "commandNumber", commandNumber);

                        command.ExecuteNonQuery();
                    }
                }
            }        
        }

        public IEnumerable<Commande> GetCommands(int? id = null)
        {
            List<Guid> commandNumbers = new();
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT CommandNumber FROM Commande ";

                    if(id is not null)
                    {
                        command.CommandText += $" WHERE Id_User = @idUser";
                        GenerateParameter(command, "idUser", id);
                    }

                    using(IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())                       
                            commandNumbers.Add((Guid)reader["CommandNumber"]);                    
                    }
                }
            }

            foreach(Guid commandNumber in commandNumbers)
            {            
                yield return GetCommandByCommandNumber(commandNumber);
            }
        }

        public Commande GetCommandByCommandNumber(Guid commandNumber)
        {
            Commande commande = new()
            {
                CommandNumber = commandNumber,
                Products = new(),
            };
            List<int> idsProduct = new();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT IdProduct FROM CommandList WHERE CommandNumber = @commandNumber";
                    GenerateParameter(command, "commandNumber", commandNumber);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            idsProduct.Add((int)reader["IdProduct"]);
                    }
                }
            }
            foreach (int idProduct in idsProduct)
            {
                commande.Products.Add(GetOne(idProduct));
            }
            return commande;
        }

        public void DeleteCommande(Guid commandNumber)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM CommandList WHERE CommandNumber = @commandNumber";
                    GenerateParameter(command, "commandNumber", commandNumber);

                    command.ExecuteNonQuery();
                }
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Commande WHERE CommandNumber = @commandNumber";
                    GenerateParameter(command, "commandNumber", commandNumber);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
