using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ARMDataManager.Library.Internal.DataAccess
{
    // internal, so nothing outside this project can use this class
    internal class SqlDataAccess
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
            _transaction = _connection.BeginTransaction();
        }

        public void StopTransaction()
        {
            _transaction?.Commit();
        }

        // open connection/start transaction method
        // load using the transaction
        // save using the transaction
        // close connection
        // implement IDisposable (makes this wrappable in a using statement)
    }
}