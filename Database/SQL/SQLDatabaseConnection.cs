using System.Data.SqlClient;

namespace MysteryGuest_INC
{
    class SQLDatabaseConnection : DatabaseConnection
    {
        public SQLDatabaseConnection(string serverName, string user, string pass, string databaseName) 
            : base(serverName, user, pass, databaseName)
        {
            _dbConnection = new SqlConnection(string.Format("Server={0}; UID={1}; PWD={2}; Database={3}", serverName, user, pass, databaseName));
        }
    }
}