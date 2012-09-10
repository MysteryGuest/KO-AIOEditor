using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MysteryGuest_INC
{
    class MySQLDatabaseConnection : DatabaseConnection
    {
        public MySQLDatabaseConnection()
        {
            _dbConnection = new MySqlConnection();
        }
    }
}