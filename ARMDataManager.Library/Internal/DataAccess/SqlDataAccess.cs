using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ARMDataManager.Library.Internal.DataAccess
{
    // internal, so nothing outside this project can use this class
    [System.Runtime.InteropServices.Guid("91841340-DA0F-463D-A268-81CD8FD94649")]
    internal class SqlDataAccess : IDisposable
    {
        /*Uses Dapper -  a micro-ORM.
         * Is fast, uses stored procedures, and doesn't generate code, unlike Entity Framework
         */
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection
                    .Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Query to load our data. T is the model we want each row to be.
                List<T> rows =
                    connection
                        .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure)
                        .ToList();

                return rows;
            }
        }

        private string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public void StartTransaction(string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows =
                _connection
                    .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction)
                    .ToList();

            return rows;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}