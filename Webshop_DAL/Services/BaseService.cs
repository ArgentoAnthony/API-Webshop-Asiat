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

namespace Webshop_DAL.Services
{
    public abstract class BaseService<TModel> : IBaseService<TModel>
    {

        protected readonly string _connectionString;

        public BaseService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");
        }

        protected void GenerateParameter(IDbCommand dbCommand, string parameterName, object? value)
        {

            IDataParameter parameter = dbCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            dbCommand.Parameters.Add(parameter);
        }

        public abstract TModel Mapper(IDataReader dataReader);

        public virtual void Delete(string tableName, int id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"DELETE FROM {tableName} WHERE Id = @id";
                    GenerateParameter(command, "id", id);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual IEnumerable<TModel> GetAll(string tableName)
        {

  
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"SELECT * FROM {tableName}";

                    using IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        yield return Mapper(reader);
                }
                connection.Close();
            }
        }

        public virtual TModel GetById(string tableName, int id)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {

                    command.CommandText = $"SELECT * FROM {tableName} WHERE Id = @id";
                    GenerateParameter(command, "id", id);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new IdNotFoundException();
                        return Mapper(reader);
                    }
                }
            }
        }
    }
}
