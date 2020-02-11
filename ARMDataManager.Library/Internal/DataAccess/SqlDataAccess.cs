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

        // get connection string
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}