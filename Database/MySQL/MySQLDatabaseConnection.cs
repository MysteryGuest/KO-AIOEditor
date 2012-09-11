using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MysteryGuest_INC
{
    class MySQLDatabaseConnection : DatabaseConnection
    {
        public MySQLDatabaseConnection(string serverName, string user, string pass, string databaseName) 
            : base(serverName, user, pass, databaseName)
        {
            _dbConnection = new MySqlConnection();
        }
    }
}