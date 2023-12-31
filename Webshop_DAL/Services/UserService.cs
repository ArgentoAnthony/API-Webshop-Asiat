using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Exceptions;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace Webshop_DAL.Services
{
    public class UserService : BaseService<User>, IUserService
    {

        public UserService(IConfiguration config) : base(config)
        {
        }

        public User Login(LoginUserFormDTO loginForm)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"SELECT * FROM Users WHERE Username = @username AND Password = @password";
                    GenerateParameter(command, "username", loginForm.Username);
                    GenerateParameter(command, "password", loginForm.Password);

                    using (IDataReader reader = command.ExecuteReader())
                    {

                        reader.Read();
                        return Mapper(reader);
                    }
                }
            }
        }

        public void Register(CreateUserDTO user)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText =
                        $"INSERT INTO Users " +
                        $"VALUES( @username, @mail, @password, @address, @birthDate, @iban, @tva, @role)";

                    GenerateParameter(command, "username", user.Username);
                    GenerateParameter(command, "mail", user.Email);
                    GenerateParameter(command, "password", user.Password);
                    GenerateParameter(command, "address", user.Address);
                    GenerateParameter(command, "birthDate", user.BirthDate);
                    GenerateParameter(command, "iban", user.Iban);
                    GenerateParameter(command, "tva", null);
                    GenerateParameter(command, "role", 1);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public User CreateVendeur(CreateVendeurDTO newVendeur)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO Users " +
                                          $"OUTPUT INSERTED.Id, INSERTED.Username, INSERTED.Mail,INSERTED.Password, INSERTED.Address, INSERTED.BirthDate, INSERTED.IBAN, INSERTED.TVA, INSERTED.Role " +
                                          $"VALUES ( @username, @mail, @password, @address, @birthDate, @iban, @tva, 4)";
                                          
                    GenerateParameter(command, "username", newVendeur.Username);
                    GenerateParameter(command, "mail", newVendeur.Email);
                    GenerateParameter(command, "password", newVendeur.Password);
                    GenerateParameter(command, "address", newVendeur.Address);
                    GenerateParameter(command, "birthDate", newVendeur.BirthDate);
                    GenerateParameter(command, "iban", newVendeur.Iban);
                    GenerateParameter(command, "tva", newVendeur.TVA);

                    using(IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Mapper(reader);
                        }
                        else
                        {
                            throw new Exception("Erreur création vendeur");
                        }
                    }
                }
            }
        }
        public bool Update(UserUpdateFormDTO userForm, int id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    UpdateUser(userForm, command);

                    command.CommandText += " WHERE Id = @id";
                    GenerateParameter(command, "id", id);

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }

        public bool Update(VendeurUpdateFormDTO vendeurUpdateForm, int id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    UpdateVendeur(vendeurUpdateForm, command);

                    command.CommandText += " WHERE Id = @id";
                    GenerateParameter(command, "id", id);

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }

        public bool Update(AdminUpdateFormDTO adminUpdateForm, int id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    UpdateAdmin(adminUpdateForm, command);

                    command.CommandText += " WHERE Id = @id";
                    GenerateParameter(command, "id", id);

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        private void UpdateUser(UserUpdateFormDTO user, IDbCommand command)
        {

            command.CommandText = $"UPDATE Users SET ";

            if (user.Username != null)
            {
                command.CommandText += $"Username = @username";
                GenerateParameter(command, "username", user.Username);
            }
            if (user.Password != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Password = @password";
                GenerateParameter(command, "password", user.Password);
            }
            if (user.Email != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Email = @email";
                GenerateParameter(command, "email", user.Email);
            }
            if (user.Address != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Address = @address";
                GenerateParameter(command, "address", user.Address);
            }
            if (user.BirthDate != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"BirthDate = @birthdate";
                GenerateParameter(command, "birthdate", user.BirthDate);
            }
            if (user.Iban != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Iban = @iban";
                GenerateParameter(command, "iban", user.Iban);
            }

        }

        private void UpdateVendeur(VendeurUpdateFormDTO vendeur, IDbCommand command)
        {

            UpdateUser(vendeur, command);
            if (vendeur.TVA != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Tva = @tva";
                GenerateParameter(command, "tva", vendeur.TVA);
            }

        }

        private void UpdateAdmin(AdminUpdateFormDTO user, IDbCommand command)
        {

            UpdateVendeur(user, command);
            if (user.Role != null)
            {
                if (command.CommandText != "UPDATE Users SET ")
                    command.CommandText += ", ";
                command.CommandText += $"Role = @role";
                GenerateParameter(command, "role", user.Role);
            }
        }
        public override User Mapper(IDataReader dataReader)
        {
            return new User
            {

                Id = (int)dataReader["Id"],
                Username = (string)dataReader["Username"],
                Password = (string)dataReader["Password"],
                Email = (string)dataReader["Mail"],
                Address = (string)dataReader["Address"],
                BirthDate = dataReader["BirthDate"] == DBNull.Value ? null : (DateTime)dataReader["BirthDate"],
                Iban = dataReader["IBAN"] == DBNull.Value ? null : (string)dataReader["IBAN"],
                Tva = dataReader["TVA"] == DBNull.Value ? null : (int)dataReader["TVA"],
                Role = GetRole((int)dataReader["Role"])
            };
        }
        protected static string GetRole(int v)
        {
            return v switch
            {
                1 => "Admin",
                2 => "Modo",
                3 => "User",
                4 => "Vendeur",
                _ => throw new RoleNotFoundException(),
            };
        }
    }
}
