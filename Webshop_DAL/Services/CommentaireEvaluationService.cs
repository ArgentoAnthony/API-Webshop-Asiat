using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Models;
using Microsoft.Extensions.Configuration;
using Webshop_DAL.Interfaces;

namespace Webshop_DAL.Services
{
    public class CommentaireEvaluationService : BaseService<Commentaires>, ICommentaireEvaluationService
    {
        public CommentaireEvaluationService(IConfiguration config) : base(config)
        {
        }
        public override Commentaires Mapper(IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
        public bool RatingProduct(Evaluation rating, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                bool exist = false;
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(*) FROM Evaluation WHERE Id_User = @idUser AND Id_Produit = @idProduit";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduit", rating.id);

                    exist = (int)command.ExecuteScalar() == 1;
                }

                using (IDbCommand command = connection.CreateCommand())
                {
                    if (exist)
                        command.CommandText = $"UPDATE Evaluation SET Etoile = @rating WHERE Id_User = @idUser AND Id_Produit = @idProduit";
                    else
                        command.CommandText = $"INSERT INTO Evaluation VALUES(@idUser, @idProduit, @rating)";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduit", rating.id);
                    GenerateParameter(command, "rating", rating.RatingValue);

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public string LeaveComment(Commentaires commentaire, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                bool exist = false;
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(*) FROM Commentaires WHERE Id_User = @idUser AND Id_Produit = @idProduit";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduit", commentaire.Id);

                    exist = (int)command.ExecuteScalar() == 1;
                }
                if (exist)
                    throw new Exception("Vous avez déjà laissé un commentaire sur ce produit.");
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Commentaires OUTPUT INSERTED.Commentaire VALUES(@idUser, @idProduct,@comment)";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduct", commentaire.Id);
                    GenerateParameter(command, "@comment", commentaire.Commentaire);

                    IDataReader reader = command.ExecuteReader();
                    reader.Read();
                    return reader["Commentaire"].ToString();
                }
            }
        }
        public bool UpdateComment(Commentaires commentaire, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Commentaires SET Commentaire = @comm WHERE Id_User = @idUser AND Id_Produit =@idProduit";
                    GenerateParameter(command, "comm", commentaire.Commentaire);
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduit", commentaire.Id);

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public bool DeleteComment(int commentaires, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Commentaires WHERE Id_User = @idUser AND Id_Produit =@idProduit";
                    GenerateParameter(command, "idUser", id);
                    GenerateParameter(command, "idProduit", commentaires);
                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public IEnumerable<Commentaires> GetCommentsByProduct(int idProduct)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Commentaires WHERE Id_Produit = @idProduit";
                    GenerateParameter(command, "idProduit", idProduct);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return Mapper(reader);
                    }
                }
            }
        }

    }
}
